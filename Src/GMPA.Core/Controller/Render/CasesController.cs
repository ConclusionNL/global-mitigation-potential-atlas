using GMPA.Core.Extensions;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller.Render
{
    public class CasesController : RenderController
    {
        public CasesController(
            ILogger<CasesController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor
        )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //Do nothing
        }

        public IActionResult Cases()
        {
            var viewModel = new CasesViewModel();

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}