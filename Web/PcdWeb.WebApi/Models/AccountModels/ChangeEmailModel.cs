using System.ComponentModel.DataAnnotations;

namespace PcdWeb.Models.AccountModels
{
    public class ChangeEmailModel
    {
        [Required]
        [EmailAddress]
        public string CurrentEmail { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
