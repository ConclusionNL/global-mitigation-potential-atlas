using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller
{
    public class CaseController : RenderController
    {
        public CaseController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
            , IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public IActionResult Case()
        {
            var caseDocument = (Case)CurrentPage;

            var viewModel = new CaseViewModel();
            viewModel.Build(caseDocument);

            return CurrentTemplate(viewModel);
        }
    }
}
