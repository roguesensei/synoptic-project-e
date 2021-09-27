using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private const int CARD_ID_LENGTH = 16;

        [HttpGet]
        public JsonResult Get(string cardId)
        {
            if (string.IsNullOrEmpty(cardId) || cardId.Length != CARD_ID_LENGTH)
            {
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest);
            }

            var user = UserDAL.GetUser(cardId);

            if (user == null)
            {
                return StatusResponseGenerator.Generate(HttpStatusResponse.HttpNotFound, "User not found, please register");
            }

            return new JsonResult(user);
        }

        [HttpPost]
        public JsonResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                return UserDAL.CreateUser(user) ? 
                    StatusResponseGenerator.Generate(HttpStatusResponse.HttpOk) : 
                    StatusResponseGenerator.Generate(HttpStatusResponse.HttpInternalServerError);
            }

            return StatusResponseGenerator.Generate(HttpStatusResponse.HttpBadRequest, "Invalid User object");
        }
    }
}
