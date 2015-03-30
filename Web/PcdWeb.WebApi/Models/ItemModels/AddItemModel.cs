using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models.ItemModels
{
    public class AddItemModel
    {
        [Required]
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string VideoFormat { get; set; }
        [Required]
        public string ItemType { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Discounted { get; set; }
    }
}
