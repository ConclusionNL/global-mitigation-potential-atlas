using Microsoft.AspNetCore.Html;

namespace GMPA.Core.Models.ViewModels
{
    public class CaseViewModel : MainViewModel
    {
        public class CaseCountryContextBlockList
        {
            public string Traction { get; set; }
            public string TractionDescription { get; set; }
        }

        public class CasePolicyBlockList
        {
            public string Title { get; set; }
            public string InstrumentText { get; set; }
            public string ImpactLevel { get; set; }
            public string ImpactEvaluation { get; set; }
            public string ImpactIndicator { get; set; }
            public string PolicyDecision { get; set; }
            public string PolicyOrigin { get; set; }
            public string TypeOfPolicyInstrument { get; set; }
            public string Jurisdiction { get; set; }
            public string EnforcementLevel { get; set; }
            public string SectoralCoverage { get; set; }
            public List<string> TargetGroup { get; set; }
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

        public class CaseReferenceBlockList
        {
            public string ReferenceName { get; set; }
            public string ReferenceUrl { get; set; }
        }

        public CaseModel CaseModel { get; set; }
        public string SectorTag { get; set; }
        public HtmlString CaseIntro { get; set; }
        public List<CaseCountryContextBlockList> CaseCountryContextBlock { get; set; }
        public List<CasePolicyBlockList> CasePolicyBlock { get; set; }
        public List<CaseLockInBlockList> CaseLockInBlock { get; set; }
        public List<CaseReferenceBlockList> CaseReferenceBlock { get; set; }
        public List<string> TagsList { get; set; }
    }
}
