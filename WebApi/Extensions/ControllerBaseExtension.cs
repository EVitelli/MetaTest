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
    }
}
