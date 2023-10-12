using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller.Render
{
    public class AboutTheGmpaController : RenderController
    {
        public AboutTheGmpaController(
            ILogger<AboutTheGmpaController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor
        )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //Do nothing
        }

        public IActionResult AboutTheGmpa()
        {
            var aboutGmpa = (AboutTheGmpa)CurrentPage!;

            var viewModel = new AboutTheGmpaViewModel
            {
                BodyText = new HtmlString(aboutGmpa.BodyText?.ToHtmlString() ?? string.Empty)
            };

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
