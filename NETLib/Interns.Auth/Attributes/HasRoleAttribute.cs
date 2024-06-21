using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Interns.Auth.Attributes
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

        private readonly string _requiredRole;

        public HasRoleAttribute(string role)
        {
            _requiredRole = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isInRole = context.HttpContext.User.IsInRole(_requiredRole.ToString());
            if (!isInRole)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }
    }
}
