using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AccountProgressDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AccountProgressService
{
    private readonly IAccountProgressRepository _accountProgressRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly IAccountRepository _accountRepository;
    private readonly IProgressRepository _progressRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly BookingDbContext _bookingContext;

    public AccountProgressService(IAccountProgressRepository accountProgressRepository, BookingDbContext bookingDbContext, IEmailHandler emailHandler, IAccountRepository accountRepository, IProgressRepository progressRepository, IAssignmentRepository assignmentRepository)
    {
        _accountProgressRepository = accountProgressRepository;
        _bookingContext = bookingDbContext;
        _emailHandler = emailHandler;
        _accountRepository = accountRepository;
        _progressRepository = progressRepository;
        _assignmentRepository = assignmentRepository;
    }

    public IEnumerable<AccountProgressDto> GetByProgressGuid(Guid guid)
    {
        var list = _accountProgressRepository.GetByProgressForeignKey(guid);
        if (list is null) return null;

        var baseList = new List<AccountProgressDto>();
        foreach (var item in list)
        {
            baseList.Add((AccountProgressDto)item);
        }

        return baseList;
    }

    public IEnumerable<AccountProgressDto> GetByAccountGuid(Guid guid)
    {
        var list = _accountProgressRepository.GetByAccountGuid(guid);
        if (list is null) return null;

        var baseList = new List<AccountProgressDto>();
        foreach (var item in list)
        {
            baseList.Add((AccountProgressDto)item);
        }

        return baseList;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AccountProgressDto>? Get()
    {
        var entities = _accountProgressRepository.GetAll();
        if (!entities.Any()) return null;
        var listAccountProgress = new List<AccountProgressDto>();

        foreach (var entity in entities)
        {
            listAccountProgress.Add((AccountProgressDto)entity);
        }
        return listAccountProgress;
    }

    public AccountProgressDto? Get(Guid guid)
    {
        var entity = _accountProgressRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AccountProgressDto)entity;

        return Dto;
    }

    public AccountProgressDto? Create(NewAccountProgressDto AccountProgress)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var accountStaff = _accountRepository.GetByGuid(AccountProgress.AccountGuid);
            var progress = _progressRepository.GetByGuid((Guid)AccountProgress.ProgressGuid);
            var assignment = _assignmentRepository.GetByGuid((Guid)progress.AssignmentGuid);
            var manager = _accountRepository.GetByGuid((Guid)assignment.ManagerGuid);
            if (progress is null || assignment is null || manager is null)
            {
                return null;
            }
            var created = _accountProgressRepository.Create(AccountProgress);
            _emailHandler.SendEmail(accountStaff.Email,
                                "New Assignment - Congratulations!",
                                $"<p>Dear {accountStaff.Name}</p>" +
                                $"<p>We are thrilled to inform you that you have been assigned a new task within our organization! Congratulations on this new opportunity, which we believe will showcase your talents and expertise.</p>" +
                                $"<div style=\"padding: 20px 0px;\">" +
                                $"<h3 style=\"color: #0066cc;\">Task Details:</h3>" +
                                $"<p><strong>Task :</strong> {progress.Description}</p>" +
                                $"<p><strong>Start Date:</strong> {progress.CreatedAt.ToString("dddd, dd-MM-yyyy")}</p>" +
                                $"<p><strong>Due Date:</strong> {assignment.DueDate.ToString("dddd, dd-MM-yyyy")}</p>" +
                                $"<p>Please take some time to review the task details thoroughly and familiarize yourself with the objectives and expectations. If you have any questions or need further clarifications, do not hesitate to reach out to your supervisor or the relevant department.</p>" +
                                $"<p>We have full confidence in your abilities to excel in this new responsibility and contribute positively to the team's success. Your dedication and hard work have been exemplary, and we know you will approach this task with the same level of commitment and professionalism.</p>" +
                                $"<p>Once again, congratulations on your new assignment! We look forward to witnessing your continued growth and success in your expanded role.</p>" +
                                $" </div>" +
                                $"<p>Best regards,</p>" +
                                $"<p>{manager.Name}<br>" +
                                $"Metrodata</p>");

            transaction.Commit();
            return (AccountProgressDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AccountProgressDto AccountProgressdto)
    {

        var getEntity = _accountProgressRepository.GetByGuid(AccountProgressdto.Guid);
        if (getEntity is null) return 0;

        AccountProgress AccountProgress = (AccountProgress)AccountProgressdto;
        AccountProgress.ModifiedAt = DateTime.Now;
        AccountProgress.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _accountProgressRepository.Update(AccountProgress);
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
        var entity = _accountProgressRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _accountProgressRepository.Delete(entity);
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
