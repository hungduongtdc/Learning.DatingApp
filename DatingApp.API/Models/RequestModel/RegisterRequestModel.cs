using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models.RequestModel
{
    public class RegisterRequestModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}