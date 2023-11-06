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
    public class PoliciesController : RenderController
    {
        public PoliciesController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
            , IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public IActionResult Policies()
        {
            var policy = (Policies)CurrentPage!;

            var viewModel = new PoliciesViewModel()
            {
                ClimateStrategiesBlockList = policy.ClimateStrategyPolicies
                    .Select(a => a.Content as ClimateStrategyBlock)
                    .Select(b => new PoliciesViewModel.ClimateStrategy
                    {
                        PolicyDocumentName = b.PolicyDocumentName,
                        StartYear = b.StartYear.ToString(),
                        EndYear = b.ValidTill.ToString(),
                        SourceLink = b.SourceLink,
                        KeyObjectivesTargets = b.KeyObjectivestargets?.ToHtmlString(),
                        FromCountry = b.ClimateStrategyCountry
                    })
                    .ToList(),

                PolicyInventoryBlockList = policy.PolicyInventory
                    .Select(a => a.Content as PolicyInventoryBlock)
                    .Select(b => new PoliciesViewModel.PolicyInventory
                    {
                        PolicyDocumentName = b.PolicyDocumentName,
                        Sector = b.Sector,
                        SubSector = b.SubSector,
                        StartDate = b.StartDate.ToString(),
                        EndDate = b.EndDate.ToString(),
                        SourceLink = b.SourceLink,
                        PolicyLevel = b.PolicyLevel,
                        PolicyOrigin = b.PolicyOrigin,
                        PolicyInstruments = b.PolicyInstruments,
                        Objectives = b.Objectives?.ToHtmlString(),
                        FromCountry = b.PolicyInventoryCountry
                    })
                    .ToList()
            };

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
