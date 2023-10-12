using Arlanet.Umbraco.Grid.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Umbraco.Cms.Core.Models.Blocks;

namespace GMPA.Core.Models.ViewModels
{
    public class CaseViewModel : MainViewModel
    {
        public string sectorTag { get; set; }
        public HtmlString caseIntro { get; set; }
        public List<CaseCountryContextBlockList> CaseCountryContextBlock { get; set; }
        public List<CasePolicyBlockList> CasePolicyBlock { get; set; }
        public List<CaseLockInBlockList> CaseLockInBlock { get; set; }
        public List<string> TagsList { get; set; }
    }

    public class CaseCountryContextBlockList
    {
        public string Traction { get; set; }
        public string TractionDescription { get; set; }
    }

    public class CasePolicyBlockList
    {
        public string Title { get; set; }
        public string InstrumentText { get; set; }
        public string PolicyDecision { get; set; }
        public string PolicyOrigin { get; set; }
        public string TypeOfPolicyInstrument { get; set; }
        public string Jurisdiction { get; set; }
        public string EnforcementLevel { get; set; }
        public string SectoralCoverage { get; set; }
        public string TargetGroup { get; set; }
        public string InstitutionalRequirement { get; set; }
        public string ImplementationStatus { get; set; }
        public string MonitoringAndEvaluation { get; set; }
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
    }

    public class CaseLockInBlockList
    {
        public bool NegativePositive { get; set; }
        public string LockInType { get; set; }
        public string BodyText { get; set; }
    }
}
