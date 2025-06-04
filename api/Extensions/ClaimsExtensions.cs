using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" ||
                x.Type == "given_name" ||
                x.Type == ClaimTypes.GivenName);

            if (claim == null)
                throw new Exception("Username claim not found.");

            return claim.Value;
         }


    }
}