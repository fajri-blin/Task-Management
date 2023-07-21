using System.Security.Claims;
using Task_Management.Contract.Data;
using Task_Management.Contract.Handler;
using Task_Management.Data;
using Task_Management.DTOs.AccountDto;
using Task_Management.Model;
using Task_Management.Model.Data;
using Task_Management.Utilities;

namespace Task_Management.Service;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenHandlers _tokenHandlers;
    private readonly BookingDbContext _bookingContext;

    public AccountService(IAccountRepository accountRepository, 
                          IAccountRoleRepository accountRoleRepository, 
                          IRoleRepository roleRepository,
                          ITokenHandlers tokenHandlers,
                          BookingDbContext bookingDbContext)
    {
        _accountRepository = accountRepository;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;
        _tokenHandlers = tokenHandlers;
        _bookingContext = bookingDbContext;
    }

    public string Login(LoginDto loginDto)
    {
        var getAccount = _accountRepository.GetEmailorUsername(loginDto.AccountLogin);
        if (getAccount == null) return "-1";

        if (!Hashing.ValidatePassword(loginDto.Password, getAccount!.Password)) return "-1";

        try
        {
            var claims = new List<Claim>
            {
                new Claim("Username", getAccount.Username),
                new Claim("Email", getAccount.Email),
            };

            var getAccountRole = _accountRoleRepository.GetAccountRolesByAccountGuid(getAccount.Guid);
            var getRoleNameByAccountRole = from ar in getAccountRole
                                           join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                           select r.Name;
            claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));

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
                ImageProfile = register.ImageProfile
            };
            var createAccount = _accountRepository.Create(accountSet);
            if (createAccount is null) return null;

            var IsExist = _roleRepository.GetByName(register.Role);
            if (IsExist is null)
            {
                var roleSet = new Role
                {
                    Guid = Guid.NewGuid(),
                    Name = register.Role
                };

                var createRole = _roleRepository.Create(roleSet);
                if (createRole is null) return null;

                var accountRoleSetNew = new AccountRole
                {
                    Guid = Guid.NewGuid(),
                    AccountGuid = createAccount.Guid,
                    RoleGuid = createRole.Guid
                };
                var createNewAccountRole = _accountRoleRepository.Create(accountRoleSetNew);
                if (createNewAccountRole is null) return null;
            }
            else
            {
                var accountRoleSet = new AccountRole
                {
                    Guid = Guid.NewGuid(),
                    AccountGuid = createAccount.Guid,
                    RoleGuid = IsExist.Guid
                };

                var createAccountRole = _accountRoleRepository.Create(accountRoleSet);
                if (createAccountRole is null) return null;

            }
            transactions.Commit();
            return register;
        }
        catch
        {
            transactions.Rollback();
            return null;
        }

    }

// Basic CRUD ===================================================
public IEnumerable<AccountDto>? Get()
    {
        var entities = _accountRepository.GetAll();
        if (!entities.Any()) return null;
        var listAccount = new List<AccountDto>();

        foreach ( var entity in entities)
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

    public AccountDto? Create(NewAccountDto account)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _accountRepository.Create(account);
            transaction.Commit();
            return (AccountDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AccountDto accountdto)
    {

        var getEntity = _accountRepository.GetByGuid(accountdto.Guid);
        if (getEntity is null) return 0;

        Account account = (Account) accountdto;
        account.ModifiedAt = DateTime.Now;
        account.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

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
        if(entity == null) return -1;

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
