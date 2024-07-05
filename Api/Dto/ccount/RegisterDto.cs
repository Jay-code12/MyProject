using System.ComponentModel.DataAnnotations;

namespace Api.Dto.ccount
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "please enter lenght above 3 and below 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "please enter lenght above 3 and below 15")]
        public string LastName { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "please enter lenght above 3 and below 15")]
        public string Password { get; set; }
    }
}
