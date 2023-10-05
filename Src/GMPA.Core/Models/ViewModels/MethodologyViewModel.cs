using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace GMPA.Core.Models.ViewModels
{
    public class MethodologyViewModel : MainViewModel
    {
        public HtmlString BodyText { get; set; }
    }
}
