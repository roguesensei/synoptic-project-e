using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : BaseController
    {
        [HttpPost]
        public JsonResult Login([FromBody] UserCredentialModel model)
        {
            if (ModelState.IsValid)
            {
                var user = GetCurrentUser();
                if (user == null)
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpNotFound, "User not found, please register");
                }

                if (UserAuthenticated(user))
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk, $"Welcome back {user.FirstName}");
                }

                AuthenticateUser(user);
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk, $"Welcome {user.FirstName}");
            }
            return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "Invalid Credentials, please enter a valid PIN or register");
        }

        [HttpPost]
        public JsonResult Register([FromBody] UserUploadModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = UserDAL.GetUser(user.CardId);
                if (user != null)
                {
                    return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "User already exists on card. Please use a different card or contact your administrator");
                }

                return UserDAL.CreateUser(user) ?
                    StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk) :
                    StatusResponseGenerator.Generate(HttpStatusResponse.HttpInternalServerError);
            }

            return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "Invalid User object");
        }
    }
}
