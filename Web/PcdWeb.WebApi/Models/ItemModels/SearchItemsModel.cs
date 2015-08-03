using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models.ItemModels
{
    public class SearchItemsModel
    {
        public string ItemStatus { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EmailAddress { get; set; }
    }
}
