using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackEnd.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "ClaimsPrincipal is null.");

            var claim = user.Claims.SingleOrDefault(x => 
                x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));

            if (claim == null || string.IsNullOrEmpty(claim.Value))
                throw new NullReferenceException("The given name claim is not found or is empty.");

            return claim.Value;
        }
    }
}