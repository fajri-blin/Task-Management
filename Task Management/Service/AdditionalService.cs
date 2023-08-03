using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AdditionalDto;
using Task_Management.DTOs.NewAdditionalDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AdditionalService
{
    private readonly IAdditionalRepository _additionalRepository;
    private readonly BookingDbContext _bookingContext;

    public AdditionalService(IAdditionalRepository AdditionalRepository, BookingDbContext bookingDbContext)
    {
        _additionalRepository = AdditionalRepository;
        _bookingContext = bookingDbContext;
    }

    public IEnumerable<AdditionalDto> GetByProgressGuid(Guid guid)
    {
        var list = _additionalRepository.GetByProgressForeignKey(guid);
        if (list == null) return null;

        var baseList = new List<AdditionalDto>();
        foreach (var item in list)
        {
            baseList.Add((AdditionalDto)item);
        }
        return baseList;
    }

    public FileResult DownloadFile(Guid guid)
    {
        var additional = _additionalRepository.GetByGuid(guid);

        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional", additional.FileData);

        if (!File.Exists(filepath))
        {
            return null;
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filepath, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        var bytes = System.IO.File.ReadAllBytes(filepath);
        return new FileContentResult(bytes, contentType)
        {
            FileDownloadName = additional.FileName
        };
    }

    // Basic CRUD ===================================================
    public IEnumerable<AdditionalDto>? Get()
    {
        var entities = _additionalRepository.GetAll();
        if (!entities.Any()) return null;
        var listAdditional = new List<AdditionalDto>();

        foreach (var entity in entities)
        {
            listAdditional.Add((AdditionalDto)entity);
        }
        return listAdditional;
    }

    public AdditionalDto? Get(Guid guid)
    {
        var entity = _additionalRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AdditionalDto)entity;

        return Dto;
    }

    public async Task<IEnumerable<AdditionalDto>> Create(NewAdditionalDto newAdditionalDto)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var createdList = new List<AdditionalDto>();
            foreach (var file in newAdditionalDto.FileName)
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileData = DateTime.Now.Ticks.ToString() + extension;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var exacPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional", fileData);
                using (var stream = new FileStream(exacPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var additonal = new Additional
                {
                    Guid = new Guid(),
                    ProgressGuid = newAdditionalDto.ProgressGuid,
                    FileName = file.FileName,
                    FileData = fileData,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                var created = _additionalRepository.Create(additonal);

                createdList.Add((AdditionalDto)created);
            }
            transaction.Commit();
            return createdList;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public async Task<int> Update(NewAdditionalDto additionaldto)
    {
        var additionals = _additionalRepository.GetByProgressForeignKey((Guid)additionaldto.ProgressGuid);
        if (additionals is null) return 0;



        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            foreach (var additional in additionals)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional", additional.FileData);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                _additionalRepository.Delete(additional);
            }
            foreach (var file in additionaldto.FileName)
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileData = DateTime.Now.Ticks.ToString() + extension;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var exacPath = Path.Combine(filePath, fileData);
                using (var stream = new FileStream(exacPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var additonal = new Additional
                {
                    Guid = new Guid(),
                    ProgressGuid = additionaldto.ProgressGuid,
                    FileName = file.FileName,
                    FileData = fileData,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                _additionalRepository.Create(additonal);

            }
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
        var additional = _additionalRepository.GetByGuid(guid);
        if (additional == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Additional", additional.FileData);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            _additionalRepository.Delete(additional);
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
