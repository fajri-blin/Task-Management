using System.Security.Claims;
using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Dtos.ProgressDto;
using Task_Management.DTOs.ProgressDto;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.Service;

public class ProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IAdditionalRepository _additionalRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly IAccountRepository _accountRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly BookingDbContext _bookingContext;

    public ProgressService(IProgressRepository ProgressRepository,
                           IAdditionalRepository additionalRepository,
                           IAccountProgressRepository accountProgressRepository,
                           BookingDbContext bookingDbContext,
                           IEmailHandler emailHandler,
                           IAccountRepository accountRepository, IAssignmentRepository assignmentRepository,
                           IHttpContextAccessor httpContextAccessor)
    {
        _progressRepository = ProgressRepository;
        _additionalRepository = additionalRepository;
        _accountProgressRepository = accountProgressRepository;
        _bookingContext = bookingDbContext;
        _emailHandler = emailHandler;
        _accountRepository = accountRepository;
        _assignmentRepository = assignmentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<ProgressDto> GetByAssignmentGuid(Guid guid)
    {
        var list = _progressRepository.GetByAssignmentForeignKey(guid);
        if (list == null) return null;

        var accountProgress = _accountProgressRepository.GetAll();
        var account = _accountRepository.GetAll();

        var baseList = list.Select(item => new ProgressDto
        {
            Guid = item.Guid,
            AssignmentGuid = item.AssignmentGuid,
            Description = item.Description,
            Status = item.Status,
            Additional = item.Additional,
            CheckMark = item.CheckMark,
            MessageManager = item.MessageManager,
            StaffGuid = accountProgress
            .Where(ap => ap.ProgressGuid == item.Guid)
            .Select(ap => ap.AccountGuid.Value)
            .ToList(),
            StaffName = account
            .Join(accountProgress,
                acc => acc.Guid,
                ap => ap.AccountGuid.Value,
                (acc, ap) => new { Account = acc, Progress = ap })
            .Where(x => x.Progress.ProgressGuid == item.Guid)
            .Select(x => x.Account.Name)
            .ToList()
        }).ToList();



        return baseList;
    }

    public int DeleteDeepProgress(Guid guid)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getProgress = _progressRepository.GetAnyRelatedByGuid(guid);
            if (getProgress is null) return -1;

            // Detach Progress from the Assignment entity to avoid cascading delete
            var assignment = _assignmentRepository.GetByGuid((Guid)getProgress.AssignmentGuid);
            if (assignment != null)
            {
                assignment.Progresses = null;
                _assignmentRepository.Update(assignment);
            }

            // Delete related AccountProgress entities
            var accountProgresses = getProgress.AccountProgress.ToList();
            foreach (var accountProgress in accountProgresses)
            {
                _accountProgressRepository.Delete(accountProgress);
            }

            // Delete related Additionals
            var additionals = getProgress.Additionals.ToList();
            foreach (var additional in additionals)
            {
                _additionalRepository.Delete(additional);
            }

            // Now you can delete the Progress
            _progressRepository.Delete(getProgress);

            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }




    public int UpdateStatus(UpdateStatusDto updateStatusDto)
    {

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getEntity = _progressRepository.GetByGuid(updateStatusDto.Guid);
            if (getEntity is null) return 0;

            var progress = new Progress
            {
                Guid = getEntity.Guid,
                Description = getEntity.Description,
                Status = updateStatusDto.Status,
                AssignmentGuid = getEntity.AssignmentGuid,
                Additional = updateStatusDto.Additional ?? getEntity.Additional ?? null,
                MessageManager = updateStatusDto.MessageManager ?? getEntity.MessageManager ?? null,
                CreatedAt = getEntity.CreatedAt,
                ModifiedAt = DateTime.Now
            };

            var user = _httpContextAccessor.HttpContext.User;
            var role = user.FindFirstValue(ClaimTypes.Role);

            var account = _accountRepository.GetAll();

            var assignment = _assignmentRepository.GetByGuid((Guid)getEntity.AssignmentGuid);
            var accountManager = account.FirstOrDefault(a => a.Guid == (Guid)assignment.ManagerGuid);
            var accountProgresses = _accountProgressRepository.GetByProgressForeignKey(getEntity.Guid);

            if (role == "Staff")
            {
                if (updateStatusDto.Status == StatusEnum.Done || updateStatusDto.Status == StatusEnum.Revision)
                {
                    progress.Status = getEntity.Status;
                }
                else
                {
                    progress.Status = updateStatusDto.Status;
                }

                if (updateStatusDto.Status == StatusEnum.Checking)
                {
                    var accountStaff = _accountRepository.GetByGuid((Guid)updateStatusDto.AccountGuid);
                    _emailHandler.SendEmail(accountManager.Email,
                                        $"No Reply - Task Review Required",
                                        $"<p>Dear {accountManager.Name}</p>" +
                                        $"<p>We hope this message finds you well. There is a task that requires your review for the project {assignment.Title}.</p>" +
                                        $"<div style=\"padding: 20px 0px;\">" +
                                        $"<h3>Task Details:</h3>" +
                                        $"<ul>" +
                                        $"<li><strong>Project:</strong> {assignment.Title}</li>" +
                                        $"<li><strong>Task:</strong> {progress.Description}</li>" +
                                        $"<li><strong>Due Date:</strong> {assignment.DueDate.ToString("dddd, dd-MM-yyyy")}</li>" +
                                        $"</ul>" +
                                        $"<p>The assigned staff, {accountStaff.Name ?? null}, has completed this task and awaits your review and approval to proceed.</p>" +
                                        $"<p>Please take the time to review the task details and provide your feedback or approval as necessary. Your timely action will contribute to the successful completion of the project.</p>" +
                                        $"<p>If you have any questions or need further information, please don't hesitate to reach out to the staff member or the relevant department.</p>" +
                                        $"<p>Thank you for your prompt attention to this matter. Your review is greatly appreciated.</p>" +
                                        $"</div>" +
                                        $"<p>Best regards,</p>" +
                                        $"<p>Metrodata<br>");
                }

            }
            else
            {
                if (updateStatusDto.Status == StatusEnum.Done)
                {
                    foreach (var accountProgress in accountProgresses)
                    {
                        var accountStaff = account.FirstOrDefault(a => a.Guid == (Guid)accountProgress.AccountGuid);
                        _emailHandler.SendEmail(accountStaff.Email,
                                        $"No Reply - Task Completed",
                                        $"<p>Dear {accountStaff.Name}</p>" +
                                        $"<p>We hope this message finds you well. We are pleased to inform you that the task assigned to you has been completed successfully!</p>" +
                                        $"<div style=\"padding: 20px 0px;\">" +
                                        $"<h3>Task Details:</h3>" +
                                        $"<ul>" +
                                        $"<li><strong>Project:</strong> {assignment.Title}</li>" +
                                        $"<li><strong>Task:</strong> {progress.Description}</li>" +
                                        $"<li><strong>Due Date:</strong> {assignment.DueDate.ToString("dddd, dd-MM-yyyy")}</li>" +
                                        $"<li><strong>Completion Date:</strong> {DateTime.Now.ToString("dddd, dd-MM-yyyy")}</li>" +
                                        $"</ul>" +
                                        $"<p>Your dedication and effort in accomplishing this task are highly commendable. The work you've done will undoubtedly contribute significantly to the overall success of the project.</p>" +
                                        $"<p>If you have any further tasks or require any assistance, please don't hesitate to reach out to your supervisor or the relevant department.</p>" +
                                        $"<p>Once again, thank you for your hard work and commitment. We appreciate your valuable contributions to the team.</p>" +
                                        $"</div>" +
                                        $"<p>Best regards,</p>" +
                                        $"<p>Metrodata<br>");
                    }

                    progress.CheckMark = true;
                }
                else if (updateStatusDto.Status == StatusEnum.Revision)
                {
                    foreach (var accountProgress in accountProgresses)
                    {
                        var accountStaff = account.FirstOrDefault(a => a.Guid == (Guid)accountProgress.AccountGuid);
                        _emailHandler.SendEmail(accountStaff.Email,
                                        $"No Reply - Task Revision Required",
                                        $"<p>Dear {accountStaff.Name}</p>" +
                                        $"<p>We hope this message finds you well. The task assigned to you requires some revision.</p>" +
                                        $"<div style=\"padding: 20px 0px;\">" +
                                        $"<h3>Task Details:</h3>" +
                                        $"<ul>" +
                                        $"<li><strong>Project:</strong> {assignment.Title}</li>" +
                                        $"<li><strong>Task:</strong> {progress.Description}</li>" +
                                        $"<li><strong>Due Date:</strong> {assignment.DueDate.ToString("dddd, dd-MM-yyyy")}</li>" +
                                        $"</ul>" +
                                        $"<p>Upon review, we have identified areas that need improvement. We kindly request you to address the feedback and make the necessary revisions accordingly.</p>" +
                                        $"<p>If you have any questions or need further clarifications regarding the required changes, please don't hesitate to reach out to your supervisor or the relevant department.</p>" +
                                        $"<p>Thank you for your attention to this matter. We appreciate your commitment to delivering high-quality work for the success of the project.</p>" +
                                        $"</div>" +
                                        $"<p>Best regards,</p>" +
                                        $"<p>Metrodata<br>");
                    }
                }

            }

            _progressRepository.Update(progress);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    // Basic CRUD ===================================================
    public IEnumerable<ProgressDto>? Get()
    {
        var entities = _progressRepository.GetAll();
        if (!entities.Any()) return null;
        var listProgress = new List<ProgressDto>();

        foreach (var entity in entities)
        {
            listProgress.Add((ProgressDto)entity);
        }
        return listProgress;
    }

    public ProgressDto? Get(Guid guid)
    {
        var entity = _progressRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (ProgressDto)entity;

        return Dto;
    }

    public ProgressDto? Create(NewProgressDto Progress)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _progressRepository.Create(Progress);
            transaction.Commit();
            return (ProgressDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(ProgressDto Progressdto)
    {

        var getEntity = _progressRepository.GetByGuid(Progressdto.Guid);
        if (getEntity is null) return 0;

        Progress Progress = (Progress)Progressdto;
        Progress.ModifiedAt = DateTime.Now;
        Progress.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _progressRepository.Update(Progress);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    public int Delete(Guid guid)
    {
        var entity = _progressRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _progressRepository.Delete(entity);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }
    // End Basic CRUD =========================================

}
