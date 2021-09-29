using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.Authentication;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Extensions;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;

namespace SynopticProject_Project_E.Controllers
{
    /// <summary>
    /// User management controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Get a user by Card ID
        /// </summary>
        /// <param name="cardId">Card ID</param>
        /// <returns>User object or null</returns>
        [HttpGet]
        public JsonResult Get(string cardId)
        {
            if (UserAuthenticated(GetCurrentUser()))
            {
                if (UserSessionExpired(GetCurrentUser()))
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpAuthenticationTimeout, "Sessions has expired, please log in");
                }

                if (string.IsNullOrEmpty(cardId) || cardId.Length != CARD_ID_LENGTH)
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest);
                }

                if (CurrentUserHasPermission(cardId))
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpForbidden);
                }

                var user = UserDAL.GetUser(cardId);

                if (user == null)
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpNotFound);
                }

                return new JsonResult(user);
            }

            return StatusResponseGenerator.Generate(HttpStatusResponse.HttpUnauthorized, "Please Log in");
        }

        // Temp - Debug method
        //[Route("token")]
        //public JsonResult GetUserToken(string cardId, string pin)
        //{
        //    if (!UserDAL.IsValidUser(cardId, pin))
        //    {
        //        return StatusResponseGenerator.Generate(HttpStatusResponse.HttpUnauthorized, "Invalid User");
        //    }

        //    string token = $"{cardId}:{pin}";

        //    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk, token.ToBase64());
        //}

    }
}
