using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller.Render
{
    public class MethodologyController : RenderController
    {
        public MethodologyController(
            ILogger<MethodologyController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor
        )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //Do nothing
        }

        public IActionResult Methodology()
        {
            var methodology = (Methodology)CurrentPage!;

            var viewModel = new MethodologyViewModel
            {
                BodyText = methodology.BodyText?.ToHtmlString() ?? string.Empty,

                MethodologyReferenceBlock = methodology.References
                    .Select(a => a.Content as MethodologyReferences)
                    .Select(b => new MethodologyViewModel.MethodologyReferenceBlockList
                    {
                        Title = b.ReferenceName,
                        Reference = b.ReferenceContent.ToHtmlString(),
                    })
                    .ToList(),

                MethodologySectionBlock = methodology.MethodologySections
                    .Select(a => a.Content as MethodologyBlock)
                    .Select(b => new MethodologyViewModel.MethodologySectionBlockList
                    {
                        SectionHeader = b.SectionHeader,
                        SectionBody = b.SectionBody.ToHtmlString(),
                        SectionId = b.SectionID
                    })
                    .ToList()
            };

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
