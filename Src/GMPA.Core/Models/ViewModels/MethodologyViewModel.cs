namespace GMPA.Core.Models.ViewModels
{
    public class MethodologyViewModel : MainViewModel
    {
        public class MethodologyReferenceBlockList
        {
            public string Title { get; set; }
            public string Reference { get; set; }
        }

        public class MethodologySectionBlockList
        {
            public string SectionHeader { get; set; }
            public string SectionBody { get; set; }
            public string SectionId { get; set; }
        }

        public string BodyText { get; set; }
        public List<MethodologyReferenceBlockList> MethodologyReferenceBlock { get; set; }
        public List<MethodologySectionBlockList> MethodologySectionBlock { get; set; }
    }
}
