using System.ComponentModel.DataAnnotations;

namespace SynopticProject_Project_E.Models
{
    /// <summary>
    /// User credentials model
    /// </summary>
    public class UserCredentialModel
    {
        /// <summary>
        /// User's Card ID
        /// </summary>
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string CardId { get; set; }

        /// <summary>
        /// User's PIN
        /// </summary>
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string PIN { get; set; }
    }

    /// <summary>
    /// User upload model
    /// </summary>
    public class UserUploadModel : UserCredentialModel
    {
        /// <summary>
        /// User's Employee ID
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// User's surname
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// User's mobile number
        /// </summary>
        [Required]
        [StringLength(20)]
        public string MobileNumber { get; set; }
    }

    /// <summary>
    /// User database object
    /// </summary>
    public class User : UserUploadModel
    {
        /// <summary>
        /// MongoDB ID
        /// </summary>
        public object _id { get; set; }

        /// <summary>
        /// Is user admin
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
