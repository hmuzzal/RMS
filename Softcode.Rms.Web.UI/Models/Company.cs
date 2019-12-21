
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Softcode.Rms.Web.UI.Models
{

    [Table("Company")]
    public class Company
    {
       
        public int CompanyId { get; set; }
        public string CompId { get; set; }
        public string Name { get; set; }
        public string AddressInfo { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }

        

    }
}
