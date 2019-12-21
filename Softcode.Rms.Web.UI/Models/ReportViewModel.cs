using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Softcode.Rms.Web.UI.Models
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DisplayName("Image")]
        public IFormFile Image { get; set; }

        //[DisplayName("Image")]
        public string ImageUrl { get; set; }

        public string ReportUrl { get; set; }
        [Required]
        [DisplayName("ISO No")]
        public string IsoNo { get; set; }

        [Required]
        public string Description { get; set; }


        [Required(ErrorMessage = "Select Module")]
        [DisplayName("Module")]
        public int ModuleId { get; set; }

        public Module Module { get; set; }

        [Required(ErrorMessage = "Select Sub-Module")]
        [DisplayName("Sub Module")]
        public int SubModuleId { get; set; }

        public SubModule SubModule { get; set; }
    }
}
