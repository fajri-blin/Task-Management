﻿using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AssignmentDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AssignmentService
{
    private readonly IAssignmentRepository _assignemtnRepository;
    private readonly IProgressRepository _progressRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;
    private readonly IAdditionalRepository _additionalRepository;
    private readonly BookingDbContext _bookingContext;

    public AssignmentService(IAssignmentRepository TaskRepository, IProgressRepository progressRepository, BookingDbContext bookingDbContext)
    {
        _assignemtnRepository = TaskRepository;
        _progressRepository = progressRepository;
        _bookingContext = bookingDbContext;
    }

    public int DeleteDeepAssignment(Guid guid)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getAssignment = _assignemtnRepository.GetByGuid(guid);
            if (getAssignment != null) return -1;

            var getListProgress = _progressRepository.GetByAssignmentForeignKey(getAssignment.Guid);
            if (getListProgress != null)
            {
                foreach (var progress in getListProgress)
                {
                    var getListAccountProgress = _accountProgressRepository.GetByProgressForeignKey(progress.Guid);
                    if (getListAccountProgress != null)
                    {
                        foreach (var accountProgress in getListAccountProgress)
                        {
                            _accountProgressRepository.Delete(accountProgress);
                        }
                    }
                    var getListAdditional = _additionalRepository.GetByProgressForeignKey(progress.Guid);
                    if (getListAdditional != null)
                    {
                        foreach (var additional in getListAdditional)
                        {
                            _additionalRepository.Delete(additional);
                        }
                    }
                    _progressRepository.Delete(progress);
                }
            }
            _assignemtnRepository.Delete(getAssignment);
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
    public IEnumerable<AssignmentDto>? Get()
    {
        var entities = _assignemtnRepository.GetAll();
        if (!entities.Any()) return null;
        var listTask = new List<AssignmentDto>();

        foreach (var entity in entities)
        {
            listTask.Add((AssignmentDto)entity);
        }
        return listTask;
    }

    public AssignmentDto? Get(Guid guid)
    {
        var entity = _assignemtnRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AssignmentDto)entity;

        return Dto;
    }

    public AssignmentDto? GetByManager(Guid guid)
    {
        var entity = _assignemtnRepository.GetByManager(guid);
        if (entity is null) return null;

        var Dto = (AssignmentDto)entity;

        return Dto;
    }

    public AssignmentDto? Create(NewAssignmentDto Task)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _assignemtnRepository.Create(Task);
            transaction.Commit();
            return (AssignmentDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AssignmentDto assignmentDto)
    {

        var getEntity = _assignemtnRepository.GetByGuid(assignmentDto.Guid);
        if (getEntity is null) return 0;

        Assignment assignment = (Assignment)assignmentDto;
        assignment.ModifiedAt = DateTime.Now;
        assignment.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _assignemtnRepository.Update(assignment);
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
        var entity = _assignemtnRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _assignemtnRepository.Delete(entity);
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
