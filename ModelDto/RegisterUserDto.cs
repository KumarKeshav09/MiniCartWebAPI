using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MintCartWebApi.ModelDto
{
    public class RegisterUserDto
    {

        [Required(ErrorMessage = "User name is required")]
        [Column(TypeName = "nvarchar(255)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User role is required")]
        public int UserRole { get; set; }

        [Required(ErrorMessage = "User email is required")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Phone]
        public string? UserPhone { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Required(ErrorMessage = "User password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "User password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
