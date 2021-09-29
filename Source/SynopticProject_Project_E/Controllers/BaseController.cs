using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected const int CARD_ID_LENGTH = 16;
        private Dictionary<string, UserSession> authenticatedUsers = new Dictionary<string, UserSession>();

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

        public bool CurrentUserHasPermission(string cardId)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }
            return user.CardId == cardId || user.IsAdmin;
        }

        public bool UserAuthenticated(User user)
        {
            return user != null && authenticatedUsers.ContainsKey(user.CardId);
        }

        public void AuthenticateUser(User user)
        {
            if (!UserAuthenticated(user))
            {
                authenticatedUsers.Add(user.CardId, new UserSession(user));
            }
        }

        public bool UserSessionExpired(User user)
        {
            string cardId = user.CardId;

            if (authenticatedUsers[cardId].sessionTimeStampUTC.AddMinutes(5) >= DateTime.UtcNow)
            {
                // Session expired
                authenticatedUsers.Remove(cardId);
                return true;
            }
            return false;
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
