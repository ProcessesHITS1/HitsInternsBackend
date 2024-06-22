using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Interns.Auth.Attributes.HasRole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HasRoleAttribute : Attribute, IAuthorizationFilter
    {
        public static class UserRoles
        {
            public const string STUDENT = "ROLE_STUDENT";
            public const string SCHOOL_REPRESENTATIVE = "ROLE_SCHOOL_REPRESENTATIVE";
            public const string ADMIN = "ROLE_ADMIN";
        }

        private readonly string[] _acceptedRoles;

        public HasRoleAttribute(params string[] roles)
        {
            _acceptedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAnyAcceptedRole = _acceptedRoles.Any(context.HttpContext.User.IsInRole);
            if (!hasAnyAcceptedRole)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }
    }
}
