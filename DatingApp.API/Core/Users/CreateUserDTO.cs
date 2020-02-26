using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Core.Users
{
    public class CreateUserDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string UserName { get; set; }
        [StringLength(20, MinimumLength = 7)]
        public string PassWord { get; set; }
    }
}