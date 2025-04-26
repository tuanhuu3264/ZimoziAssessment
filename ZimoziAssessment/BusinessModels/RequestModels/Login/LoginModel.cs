using System.ComponentModel.DataAnnotations;

namespace Zimozi.Assessment.BusinessModels.RequestModels.Login
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
