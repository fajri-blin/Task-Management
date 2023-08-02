using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;
using Task_Management.Contract.Data;
using Task_Management.Contract.Handler;
using Task_Management.Data;
using Task_Management.Dtos.AccountDto;
using Task_Management.DTOs.AccountDto;
using Task_Management.Model.Data;
using Task_Management.Utilities;
using Task_Management.Utilities.Enum;

namespace Task_Management.Service;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenHandlers _tokenHandlers;
    private readonly IEmailHandler _emailHandler;
    private readonly BookingDbContext _bookingContext;

    public AccountService(IAccountRepository accountRepository,
                          IRoleRepository roleRepository,
                          ITokenHandlers tokenHandlers,
                          BookingDbContext bookingDbContext,
                          IEmailHandler emailHandler)
    {
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
        _tokenHandlers = tokenHandlers;
        _bookingContext = bookingDbContext;
        _emailHandler = emailHandler;
    }

    public string Login(LoginDto loginDto)
    {
        var getAccount = _accountRepository.GetEmailorUsername(loginDto.AccountLogin);
        if (getAccount == null) return "-1";

        if (!Hashing.ValidatePassword(loginDto.Password, getAccount!.Password)) return "-1";

        try
        {
            var getRoleName = from ar in _accountRepository.GetAll()
                              join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                              where ar.Guid == getAccount.Guid
                              select r.Name;
            var claims = new List<Claim>
            {
                new Claim("Guid", getAccount.Guid.ToString()),
                new Claim("Username", getAccount.Username),
                new Claim("Email", getAccount.Email),
                new Claim(ClaimTypes.Name, getAccount.Name),
                new Claim(ClaimTypes.Role, getRoleName.First())
            };


            var getToken = _tokenHandlers.GenerateToken(claims);
            return getToken;

        }
        catch
        {
            return "-1";
        }
    }

    public RegisterDto? Register(RegisterDto register)
    {
        using var transactions = _bookingContext.Database.BeginTransaction();
        try
        {
            var accountSet = new Account
            {
                Guid = Guid.NewGuid(),
                Username = register.Username,
                Name = register.Name,
                Email = register.Email,
                Password = Hashing.HashPassword(register.Password),
                OTP = 0,
                IsUsedOTP = false,
                ImageProfile = register.ImageProfile,
            };

            var roleName = Enum.GetName(typeof(RoleLevel), register.Role);
            var IsExist = _roleRepository.GetByName(roleName);
            if (IsExist is null)
            {
                var roleSet = new Role
                {
                    Guid = Guid.NewGuid(),
                    Name = roleName
                };

                var createRole = _roleRepository.Create(roleSet);
                if (createRole is null) return null;
                accountSet.RoleGuid = createRole.Guid;
            }
            else
            {
                accountSet.RoleGuid = IsExist.Guid;
            }
            var createAccount = _accountRepository.Create(accountSet);
            if (createAccount is null) return null;
            transactions.Commit();
            return register;
        }
        catch
        {
            transactions.Rollback();
            return null;
        }

    }

    public int ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var entity = _accountRepository.GetEmailorUsername(forgotPassword.Email);
        if (entity is null)
            return 0; // Email not found

        var account = _accountRepository.GetByGuid(entity.Guid);
        if (account is null)
            return -1;

        var otp = new Random().Next(111111, 999999);
        var isUpdated = _accountRepository.Update(new Account
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            Name = account.Name,
            Password = account.Password,
            OTP = otp,
            IsUsedOTP = false,
            ImageProfile = account.ImageProfile,
            RoleGuid = account.RoleGuid,
            ModifiedAt = DateTime.Now,
            CreatedAt = account.CreatedAt,
        });

        if (!isUpdated)
        {
            return -1;
        }

        _emailHandler.SendEmail(forgotPassword.Email,
                                "Forgot Password",
                                $"Your OTP is {otp}");

        return 1;
    }

    public int CheckOtp(CheckOtp checkOtp)
    {
        var isExist = _accountRepository.GetByEmailOtp(checkOtp.Email, checkOtp.OTP);
        if (isExist is null) return 0;
        TimeSpan timeDifference = DateTime.Now - isExist.ModifiedAt;
        double minutesDifference = timeDifference.TotalMinutes;

        if (minutesDifference >= 3)
        {
            return 1;
        }
        return 2;
    }

    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _accountRepository.GetEmailorUsername(changePasswordDto.Email);
        if (isExist is null)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.OTP != changePasswordDto.Otp)
        {
            return 0;
        }

        if (getAccount.IsUsedOTP == true)
        {
            return 1;
        }

        TimeSpan timeDifference = DateTime.Now - getAccount.ModifiedAt;
        double minutesDifference = timeDifference.TotalMinutes;

        if (minutesDifference >= 3)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            Username = getAccount.Username,
            Email = getAccount.Email,
            Name = getAccount.Name,
            Password = Hashing.HashPassword(changePasswordDto.NewPassword),
            OTP = getAccount.OTP,
            IsUsedOTP = true,
            ImageProfile = getAccount.ImageProfile,
            RoleGuid = getAccount.RoleGuid,
            ModifiedAt = DateTime.Now,
            CreatedAt = getAccount.CreatedAt,
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not updated
        }
        return 3;
    }

    public FileResult Photo(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);

        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Photos", account.ImageProfile);

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
        return new FileContentResult(bytes, contentType);
    }

    // Basic CRUD ===================================================
    public IEnumerable<AccountDto>? Get()
    {
        var entities = _accountRepository.GetAll();
        if (!entities.Any()) return null;
        var listAccount = new List<AccountDto>();

        foreach (var entity in entities)
        {
            listAccount.Add((AccountDto)entity);
        }
        return listAccount;
    }

    public AccountDto? Get(Guid guid)
    {
        var entity = _accountRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AccountDto)entity;

        return Dto;
    }

    public int Update(UpdateAccountDto updateAccountDto)
    {

        var getEntity = _accountRepository.GetByGuid(Guid.Parse(updateAccountDto.Guid));
        if (getEntity is null) return 0;


        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

            string fileData = null;

            string password = null;

            if (updateAccountDto.Password != null)
            {
                Hashing.HashPassword(updateAccountDto.Password);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (updateAccountDto.ImageProfile != null && updateAccountDto.ImageProfile.Length != 0)
            {
                var oldPhoto = Path.Combine(filePath, getEntity.ImageProfile);
                if (File.Exists(oldPhoto))
                {
                    File.Delete(oldPhoto);
                }
                var extension = "." + updateAccountDto.ImageProfile.FileName.Split('.')[updateAccountDto.ImageProfile.FileName.Split('.').Length - 1];
                fileData = DateTime.Now.Ticks.ToString() + extension;

                var newPhoto = Path.Combine(filePath, fileData);
                using (var stream = new FileStream(newPhoto, FileMode.Create))
                {
                    updateAccountDto.ImageProfile.CopyToAsync(stream);
                }
            }

            var account = new Account
            {
                Guid = Guid.Parse(updateAccountDto.Guid),
                Email = updateAccountDto.Email ?? getEntity.Email,
                Name = updateAccountDto.Name ?? getEntity.Name,
                OTP = getEntity.OTP,
                ImageProfile = fileData ?? getEntity.ImageProfile,
                IsUsedOTP = getEntity.IsUsedOTP,
                Password = password ?? getEntity.Password,
                Username = updateAccountDto.Username ?? getEntity.Username,
                CreatedAt = getEntity.CreatedAt,
                ModifiedAt = DateTime.Now,
            };
            if (updateAccountDto.RoleGuid != null)
            {
                var roleName = Enum.GetName(typeof(RoleLevel), updateAccountDto.RoleGuid);
                var IsExist = _roleRepository.GetByName(roleName);
                if (IsExist is null)
                {
                    var roleSet = new Role
                    {
                        Guid = Guid.NewGuid(),
                        Name = roleName
                    };

                    var createRole = _roleRepository.Create(roleSet);
                    if (createRole is null) return 0;
                    account.RoleGuid = createRole.Guid;
                }
                else
                {
                    account.RoleGuid = IsExist.Guid;
                }
            }
            else
            {
                account.RoleGuid = getEntity.RoleGuid;
            }

            _accountRepository.Update(account);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    public int ProfileUpdate(UpdateAccountDto updateAccountDto)
    {

        var getEntity = _accountRepository.GetByGuid(Guid.Parse(updateAccountDto.Guid));
        if (getEntity is null) return 0;


        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

            string fileData = null;

            string password = null;

            if (updateAccountDto.Password != null)
            {
                Hashing.HashPassword(updateAccountDto.Password);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (updateAccountDto.ImageProfile != null && updateAccountDto.ImageProfile.Length != 0)
            {
                var oldPhoto = Path.Combine(filePath, getEntity.ImageProfile);
                if (File.Exists(oldPhoto))
                {
                    File.Delete(oldPhoto);
                }
                var extension = "." + updateAccountDto.ImageProfile.FileName.Split('.')[updateAccountDto.ImageProfile.FileName.Split('.').Length - 1];
                fileData = DateTime.Now.Ticks.ToString() + extension;

                var newPhoto = Path.Combine(filePath, fileData);
                using (var stream = new FileStream(newPhoto, FileMode.Create))
                {
                    updateAccountDto.ImageProfile.CopyToAsync(stream);
                }
            }

            var account = new Account
            {
                Guid = Guid.Parse(updateAccountDto.Guid),
                Email = updateAccountDto.Email ?? getEntity.Email,
                Name = updateAccountDto.Name ?? getEntity.Name,
                OTP = getEntity.OTP,
                ImageProfile = fileData ?? getEntity.ImageProfile,
                IsUsedOTP = getEntity.IsUsedOTP,
                RoleGuid = getEntity.RoleGuid,
                Password = password ?? getEntity.Password,
                Username = updateAccountDto.Username ?? getEntity.Username,
                CreatedAt = getEntity.CreatedAt,
                ModifiedAt = DateTime.Now,
            };

            _accountRepository.Update(account);
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
        var entity = _accountRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _accountRepository.Delete(entity);
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
