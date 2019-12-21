using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Softcode.Rms.Web.UI.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Serial No")]
        public int SerialNo { get; set; }

        public virtual ICollection<SubModule> SubModules { get; set; }

        public virtual ICollection<Softcode.Rms.Web.UI.Models.Report> Reports { get; set; }
    }
}
