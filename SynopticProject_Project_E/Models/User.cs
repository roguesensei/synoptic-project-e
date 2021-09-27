using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynopticProject_Project_E.Models
{
    public class User
    {
        [StringLength(16, MinimumLength = 16)]
        public string CardId { get; set; }

        public ushort EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }
    }
}
