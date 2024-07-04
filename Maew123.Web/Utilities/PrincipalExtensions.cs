using System.Security.Principal;

namespace Maew123.Web.Utilities
{
    public static class PrincipalExtensions
    {
        //ต้องมีทุกrole ที่กล่าว
        public static bool IsInAllRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.All(r => principal.IsInRole(r));
        }

        //มี 1 ใน roles ทั้งหมด
        public static bool IsInAnyRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(r => principal.IsInRole(r));
        }

        //วิธีใช้
        // user must be assign to all of the roles  
        //if(User.IsInAllRoles("Admin","Manager","YetOtherRole"))
        //{
        //    // do something
        //} 

        //// one of the roles sufficient
        //if(User.IsInAnyRoles("Admin","Manager","YetOtherRole"))
        //{
        //    // do something
        //} 
            }
}
