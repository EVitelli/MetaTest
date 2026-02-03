using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Extensions
{
    public static class ControllerBaseExtension
    {

        public static string GetClaimEmailValue(this ControllerBase controller, ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type.Contains("email")).Value;
        }

        public static uint GetClaimIdValue(this ControllerBase controller, ClaimsPrincipal user)
        {
            return Convert.ToUInt32(user.Claims.FirstOrDefault(x => x.Type.ToLower() == "id").Value);
        }

        public static ETipoUsuario GetClaimRoleValue(this ControllerBase controller, ClaimsPrincipal user)
        {
            Enum.TryParse<ETipoUsuario>(user.Claims.FirstOrDefault(x => x.Type.Contains("role")).Value, out ETipoUsuario role);
            return role;
        }
    }
}
