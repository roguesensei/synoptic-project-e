using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : BaseController
    {
        [HttpGet]
        public JsonResult Get(string cardId)
        {
            if (string.IsNullOrEmpty(cardId) || cardId.Length != CARD_ID_LENGTH)
            {
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpUnauthorized, "Please Swipe Card");
            }

            var user = UserDAL.GetUser(cardId);

            if (user == null)
            {
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpNotFound, "User not found, please register");
            }

            return StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk, $"Welcome back {user.FirstName}, please enter your PIN");
        }
    }
}
