using System.ComponentModel.DataAnnotations;

namespace PcdWeb.Models.AccountModels
{
    public class ChangeEmailModel
    {
        [Required]
        public string CurrentEmail { get; set; }

        [Required]
        public string NewEmail { get; set; }
    }
}
