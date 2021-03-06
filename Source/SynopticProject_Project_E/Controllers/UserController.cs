using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.Authentication;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;

namespace SynopticProject_Project_E.Controllers
{
    /// <summary>
    /// User management controller
    /// </summary>
    [ApiController]
    [BasicAuth]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// /User/ endpoint
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

                if (!CurrentUserHasPermission(cardId))
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpForbidden, "You do not have permission to view this resource");
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

        /// <summary>
        /// /User/CreateAdmin
        /// </summary>
        /// <param name="model">Admin details</param>
        /// <returns>Response from the server</returns>
        [HttpPost]
        [Route("CreateAdmin")]
        public JsonResult CreateAdminUser([FromBody] UserUploadModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "Invalid user details");
            }

            if (UserAuthenticated(GetCurrentUser()))
            {
                if (UserSessionExpired(GetCurrentUser()))
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpAuthenticationTimeout, "Sessions has expired, please log in");
                }

                if (GetCurrentUser().IsAdmin)
                {
                    if (UserDAL.GetUser(model.CardId) == null)
                    {
                        return UserDAL.CreateUser(model, true) ?
                            StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk) :
                            StatusResponseGenerator.Generate(HttpStatusResponse.HttpInternalServerError);
                    }

                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "A user with the specified Card ID already exists");
                }

                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpForbidden, "You need to be an administrator to do that");
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
