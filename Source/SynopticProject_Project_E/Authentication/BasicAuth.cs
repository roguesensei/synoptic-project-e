using Microsoft.AspNetCore.Mvc.Filters;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SynopticProject_Project_E.Authentication
{
    /// <summary>
    /// Attribute to require Basic Authorization
    /// </summary>
    public class BasicAuthAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called when a user attempts to authorize, validates the authorization header
        /// </summary>
        /// <param name="context">Authorization context</param>
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

        /// <summary>
        /// Checks if a user's credentials are vaild
        /// </summary>
        /// <param name="cardId">User's card ID</param>
        /// <param name="pin">User's PIN</param>
        /// <returns>Authorized or not</returns>
        private bool IsAuthorized(string cardId, string pin)
        {
            return UserDAL.IsValidUser(cardId, pin);
        }

        /// <summary>
        /// Sets the context result to be an unauthorized result
        /// </summary>
        /// <param name="context"></param>
        private void SetUnauthorized(AuthorizationFilterContext context)
        {
            context.Result = StatusResponseGenerator.Generate(HttpStatusResponse.HttpUnauthorized, "User unauthorized or not found. Please log in or register.");
        }
    }
}
