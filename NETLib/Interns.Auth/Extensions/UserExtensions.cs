using System.Security.Claims;

namespace Interns.Auth.Extensions
{
    public static class UserRoles
    {
        public const string STUDENT = "ROLE_STUDENT";
        public const string SCHOOL_REPRESENTATIVE = "ROLE_SCHOOL_REPRESENTATIVE";
        public const string ADMIN = "ROLE_ADMIN";
    }
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

        public static string? GetNameOrDefault(this ClaimsPrincipal user)
        {
            var nameClaim = user.Claims.FirstOrDefault(x => x.Type == "fullName");
            return nameClaim?.Value;
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            string name = user.GetNameOrDefault() ?? throw new ArgumentNullException();
            return name;
        }
        //        var claims = User.Claims;
        //         var roles = claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
    }
}
