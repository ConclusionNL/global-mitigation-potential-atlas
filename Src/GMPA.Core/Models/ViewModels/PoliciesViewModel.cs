namespace GMPA.Core.Models.ViewModels
{
    public class PoliciesViewModel
    {
        public class ClimateStrategy
        {
            public string PolicyDocumentName { get; set; }
            public string StartYear { get; set; }
            public string EndYear { get; set; }
            public string SourceLink { get; set; }
            public string KeyObjectivesTargets { get; set; }
        }

        public class PolicyInventory
        {
            public string PolicyDocumentName { get; set; }
            public string Sector { get; set; }
            public string SubSector { get; set;}
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string SourceLink { get; set; }
            public string PolicyLevel { get; set; }
            public string PolicyOrigin { get; set; }
            public string Objectives { get; set; }
        }

        public List<ClimateStrategy> ClimateStrategies { get; set; }
        public List<PolicyInventory> PolicyInventories { get; set; }
    }
}
