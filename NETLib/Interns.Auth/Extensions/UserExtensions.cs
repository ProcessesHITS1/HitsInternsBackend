using System.Security.Claims;
using Interns.Auth.Attributes.HasRole;

namespace Interns.Auth.Extensions
{
    public static class UserExtensions
    {

        public static bool IsStudent(this ClaimsPrincipal user)
        {
            return user.IsInRole(HasRoleAttribute.UserRoles.STUDENT);
        }
        public static bool IsStaff(this ClaimsPrincipal user)
        {
            return user.IsInRole(HasRoleAttribute.UserRoles.ADMIN)||user.IsInRole(HasRoleAttribute.UserRoles.SCHOOL_REPRESENTATIVE);
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
