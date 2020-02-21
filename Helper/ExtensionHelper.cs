using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace WatsonTracker.Helper
{
    public 
        static class ExtensionHelper
    {
        public static string GetFullName(this IIdentity user)
        {
            var claimsUser = (ClaimsIdentity)user;
            var claim = claimsUser.Claims.FirstOrDefault(c => c.Type == "FullName");
            return claim?.Value;
        }
    }
}