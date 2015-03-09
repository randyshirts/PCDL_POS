﻿using System.ComponentModel.DataAnnotations;

namespace PcdWeb.Web.Models
{
    public class LoginModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
