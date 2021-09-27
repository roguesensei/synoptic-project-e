using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynopticProject_Project_E.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected const int CARD_ID_LENGTH = 16;
    }
}
