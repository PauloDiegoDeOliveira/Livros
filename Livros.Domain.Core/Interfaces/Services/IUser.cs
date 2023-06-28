using System.Security.Claims;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface IUser
    {
        string Name { get; }

        Guid GetUserId();

        string GetUserEmail();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim> GetClaimsIdentity();

        string GetUserRole();

        IEnumerable<string> GetUserClaims();
    }
}