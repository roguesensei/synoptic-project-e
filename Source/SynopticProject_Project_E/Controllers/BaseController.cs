using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.Authentication;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SynopticProject_Project_E.Controllers
{
    /// <summary>
    /// Base controller to implement common methods
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Constant defining the length of a card ID
        /// </summary>
        protected const int CARD_ID_LENGTH = 16;
        private static Dictionary<string, UserSession> authenticatedUsers = new Dictionary<string, UserSession>();

        /// <summary>
        /// Returns an instance of the current user
        /// </summary>
        /// <returns>Current user</returns>
        public User GetCurrentUser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
            if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                    .Split(':', 2);

                return UserDAL.GetUser(credentials[0]);
            }
            return null;
        }

        /// <summary>
        /// Check if the user has permissions to peform an action
        /// </summary>
        /// <param name="cardId">User's card ID</param>
        /// <returns>Permission access/deny</returns>
        public bool CurrentUserHasPermission(string cardId)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }
            return user.CardId == cardId || user.IsAdmin;
        }

        /// <summary>
        /// Check if a user is authenticated
        /// </summary>
        /// <param name="user">User to check if authenticated</param>
        /// <returns>Authenticated or not</returns>
        public bool UserAuthenticated(User user)
        {
            return user != null && authenticatedUsers.ContainsKey(user.CardId);
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="user"></param>
        public void AuthenticateUser(User user)
        {
            if (!UserAuthenticated(user))
            {
                authenticatedUsers.Add(user.CardId, new UserSession(user));
            }
            else
            {
                var authUser = authenticatedUsers[user.CardId];
                authUser.sessionTimeStampUTC = DateTime.UtcNow;

                // Update entry
                authenticatedUsers[user.CardId] = authUser;
            }
        }

        /// <summary>
        /// Check if a user's session has expired
        /// </summary>
        /// <param name="user">User to check</param>
        /// <returns>Expired or not</returns>
        public bool UserSessionExpired(User user)
        {
            string cardId = user.CardId;

            if (authenticatedUsers[cardId].sessionTimeStampUTC.AddMinutes(5) <= DateTime.UtcNow)
            {
                // Session expired
                authenticatedUsers.Remove(cardId);
                return true;
            }

            var authUser = authenticatedUsers[cardId];
            authUser.sessionTimeStampUTC = DateTime.UtcNow;

            // Update entry
            authenticatedUsers[cardId] = authUser;

            return false;
        }

        /// <summary>
        /// Unauthenticate a user
        /// </summary>
        /// <param name="cardId">User's Card ID</param>
        public void UnauthenticateUser(string cardId)
        {
            authenticatedUsers.Remove(cardId);
        }

        private struct UserSession
        {
            public User user;
            public DateTime sessionTimeStampUTC;

            public UserSession(User user)
            {
                this.user = user;
                sessionTimeStampUTC = DateTime.UtcNow;
            }
        }
    }
}
