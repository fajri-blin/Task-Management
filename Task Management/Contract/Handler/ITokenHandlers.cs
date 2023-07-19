using System.Security.Claims;

namespace Task_Management.Contract.Handler;

public interface ITokenHandlers
{
    string GenerateToken(IEnumerable<Claim> claims);
}
