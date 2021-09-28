using System.ComponentModel.DataAnnotations;

namespace SynopticProject_Project_E.Models
{
    public class UserCredentialModel
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string CardId { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string PIN { get; set; }
    }

    public class UserUploadModel : UserCredentialModel
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string MobileNumber { get; set; }
    }

    public class User : UserUploadModel
    {
        public object _id { get; set; }

        public bool IsAdmin { get; set; }
    }
}
