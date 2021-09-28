using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.Authentication;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Extensions;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    [BasicAuth]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        [HttpGet]
        public JsonResult Get(string cardId)
        {
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
