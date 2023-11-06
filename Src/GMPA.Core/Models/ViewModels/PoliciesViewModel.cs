namespace GMPA.Core.Models.ViewModels
{
    public class PoliciesViewModel : MainViewModel
    {
        public class ClimateStrategy
        {
            public string PolicyDocumentName { get; set; }
            public string StartYear { get; set; }
            public string EndYear { get; set; }
            public string SourceLink { get; set; }
            public string KeyObjectivesTargets { get; set; }
            public string FromCountry { get; set; }
        }

        public class PolicyInventory
        {
            public string PolicyDocumentName { get; set; }
            public string Sector { get; set; }
            public string SubSector { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string SourceLink { get; set; }
            public string PolicyLevel { get; set; }
            public string PolicyOrigin { get; set; }
            public string PolicyInstruments { get; set; }
            public string Objectives { get; set; }
            public string FromCountry { get; set; }
        }

        public List<ClimateStrategy> ClimateStrategiesBlockList { get; set; }
        public List<PolicyInventory> PolicyInventoryBlockList { get; set; }
    }
}
