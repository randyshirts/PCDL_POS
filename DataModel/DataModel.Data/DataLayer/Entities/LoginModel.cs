using System.ComponentModel.DataAnnotations;

namespace Abp.Modules.Core.Mvc.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool UseRefreshToken { get; set; }
    }
}
