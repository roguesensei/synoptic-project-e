using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SynopticProject_Project_E.Authentication
{
    public class BasicAuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                            .Split(':', 2);
                        if (credentials.Length == 2 && IsAuthorized(credentials[0], credentials[1]))
                        {
                            return;
                        }
                    }
                }

                SetUnauthorized(context);
            }
            catch (FormatException exc)
            {
                SetUnauthorized(context);
            }
        }

        public bool IsAuthorized(string cardId, string pin)
        {
            return UserDAL.IsValidUser(cardId, pin);
        }

        private void SetUnauthorized(AuthorizationFilterContext context)
        {
            context.Result = StatusResponseGenerator.Generate(HttpStatusResponse.HttpUnauthorized, "User unauthorized or not found. Please register.");
        }
    }
}
