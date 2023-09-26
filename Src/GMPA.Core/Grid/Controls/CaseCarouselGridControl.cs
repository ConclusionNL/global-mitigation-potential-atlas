//using Arlanet.Umbraco.Grid.Base;
//using Microsoft.Extensions.Logging;
//using Umbraco.Extensions;

//namespace Marketing.Core.Grid.Controls
//{
//    public class CaseCarouselGridControl : GridControl
//    {
//        public override string Alias => "caseCarousel";
//        public override string ViewPath => "~/Views/Partials/CaseCarousel.cshtml";
//        private readonly ILogger<CaseCarouselGridControl> _logger;

//        public CaseCarouselGridControl(ILogger<CaseCarouselGridControl> logger)
//        {
//            _logger = logger;
//        }

//        //public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        //{
//        //    //List<CaseViewModel> selectedCases = new();
//        //    //var component = (CaseCarousel)gridControl.Component;

//        //    //foreach (var contentPicker in component.Cases)
//        //    //{
//        //    //    var selectedContent = (ComponentContentPicker)contentPicker.Content;
//        //    //    try
//        //    //    {
//        //    //        var selectedCase = (Case)selectedContent.Content;
//        //    //        if (selectedCase is null) { continue; }
//        //    //        selectedCases.Add(new CaseViewModel()
//        //    //        {
//        //    //            SubTitle = selectedCase.Subtitle,
//        //    //            Description = selectedCase.DescriptionCase.ToString(),
//        //    //            PageTitle = selectedCase.Name,
//        //    //            PageUrl = selectedCase.Url(),
//        //    //            CompanyLogoImageUrl = selectedCase.CompanyLogo.Url(),
//        //    //            HeaderImageUrl = selectedCase.HeroImage.Url(),
//        //    //        });
//        //    //    }
//        //    //    catch (InvalidCastException ex)
//        //    //    {
//        //    //        _logger.LogError("An invalid content type was used in the case carousel: {TYPE}", selectedCases.GetType());
//        //    //    }

//        //    //}
//        //    //return ViewModel(ViewPath, selectedCases);
//        //}
//    }
//}
