using IdentityServer4.Models;
using System;

namespace Softcode.Rms.Web.UI.Models
{
    public class ErrorViewModel
    {
        public ErrorMessage Error { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}