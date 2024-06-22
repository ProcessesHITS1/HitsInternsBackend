using System.Security.Claims;

namespace Interns.Auth.Extensions
{
    public static class UserExtensions
    {

        public static bool isStudent(this ClaimsPrincipal user)
        {
            return false;
        }
        public static Guid? GetIdOrDefault(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(x => x.Type == "id");
            return idClaim != null ? Guid.Parse(idClaim.Value) : null;
        }

        public static Guid GetId(this ClaimsPrincipal user)
        {
            Guid id = user.GetIdOrDefault() ?? throw new ArgumentNullException("Id is null or not present");
            return id;
        }
    }
}
