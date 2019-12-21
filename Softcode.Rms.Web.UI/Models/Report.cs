using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Softcode.Rms.Web.UI.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Report URL")]
        public string ReportUrl { get; set; }
        [Required]
        [DisplayName("ISO No")]
        public string IsoNo { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
   
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }


        [Required]

        public int SubModuleId { get; set; }

        public virtual SubModule SubModule { get; set; }
    }
}
