using GMPA.Core.Extensions;
using GMPA.Core.Models;
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
    public class CaseController : RenderController
    {
        public CaseController(
            ILogger<CaseController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor
        )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //Do nothing
        }

        public IActionResult Case()
        {
            var caseDocument = (Case)CurrentPage;

            var viewModel = new CaseViewModel
            {
                CaseModel = new CaseModel
                {
                    CaseName = caseDocument.Name,
                    SectorTag = caseDocument.SectorTag.FirstOrDefault(),
                    CaseDescription = caseDocument.CaseIntroduction?.ToHtmlString(),
                    CaseTags = caseDocument.Tags?.ToList()
                },

                SectorTag = caseDocument.SectorTag.FirstOrDefault(),
                CaseIntro = new HtmlString(caseDocument.CaseIntroduction?.ToHtmlString()),

                CaseCountryContextBlock = caseDocument.CaseCountryBlockList?
                    .Select(a => a.Content as CaseCountryContext)
                    .Select(b => new CaseViewModel.CaseCountryContextBlockList
                    {
                        Traction = b.Traction,
                        TractionDescription = b.DescriptionOfEnablingFactor?.ToHtmlString()
                    })
                    .ToList(),

                CasePolicyBlock = caseDocument.CasePolicyBlockList?
                    .Select(a => a.Content as CasePolicyBlock)
                    .Select(b => new CaseViewModel.CasePolicyBlockList
                    {
                        Title = b.Title,
                        InstrumentText = b.InstrumentsText?.ToHtmlString(),
                        PolicyDecision = b.PolicyDecision,
                        PolicyOrigin = b.PolicyOrigin,
                        TypeOfPolicyInstrument = b.TypeOfPolicyInstrumentDropdown,
                        Jurisdiction = b.Jurisdiction,
                        EnforcementLevel = b.EnforcementLevel,
                        SectoralCoverage = b.SectoralCoverage,
                        TargetGroup = b.TargetGroup,
                        InstitutionalRequirement = b.InstitutionalRequirement,
                        ImplementationStatus = b.ImplementationStatus,
                        MonitoringAndEvaluation = b.MonitoringAndEvaluation,
                        StartDate = new string(
                            $"{b.StartDateOfImplementation.Day}/" +
                            $"{b.StartDateOfImplementation.Month}" +
                            $"/{b.StartDateOfImplementation.Year}"),
                        EndDate = new string(
                            $"{b.EndDateOfImplementation.Day}/" +
                            $"{b.EndDateOfImplementation.Month}" +
                            $"/{b.EndDateOfImplementation.Year}")
                    })
                    .ToList(),

                CaseLockInBlock = caseDocument.CaseLockInBlockList?
                    .Select(a => a.Content as CaseLockIn)
                    .Select(b => new CaseViewModel.CaseLockInBlockList
                    {
                        NegativePositive = b.NegativePositive,
                        LockInType = b.LockInType,
                        BodyText = b.BodyText?.ToHtmlString()
                    })
                    .ToList(),

                TagsList = caseDocument.Tags.ToList()
            };

            viewModel.Build(caseDocument);

            return CurrentTemplate(viewModel);
        }
    }
}
