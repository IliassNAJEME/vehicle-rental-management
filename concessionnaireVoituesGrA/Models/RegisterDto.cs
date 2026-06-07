using System.ComponentModel.DataAnnotations;

namespace concessionnaireVoituesGrA.Models
{
    public class RegisterDto
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
