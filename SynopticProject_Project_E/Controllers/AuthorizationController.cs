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
    public class AuthorizationController : ControllerBase
    {
        private const int CARD_ID_LENGTH = 16;

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
