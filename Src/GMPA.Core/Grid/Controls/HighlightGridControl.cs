//using System.Collections.Generic;
//using Arlanet.Umbraco.Grid.Base;
//using Marketing.Core.MapDefinitions;
//using Marketing.Core.Models;
//using Marketing.Core.Models.Media;
//using Marketing.Core.Models.Umbraco;
//using Marketing.Core.Models.ViewModels.Grid;
//using Umbraco.Cms.Core.Mapping;

//namespace Marketing.Core.Grid.Controls
//{
//    public class HighlightGridControl : GridControl
//    {
//        private readonly IUmbracoMapper _mapper;
//        public override string Alias => "componentHighlight";
//        public override string ViewPath => "~/Views/Partials/Highlight.cshtml";

//        public HighlightGridControl(IUmbracoMapper mapper)
//        {
//            _mapper = mapper;
//        }

//        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        {
//            var component = (ComponentHighlight)gridControl.Component;

//            var sourceSets = new Dictionary<string, (int? Width, int? Height, AspectRatio Ratio)>
//            {
//                { Constants.Media.Breakpoints.ExtraSmall, (551, 551, AspectRatio.OneByOne) },
//                { Constants.Media.Breakpoints.Small, (516, 516, AspectRatio.OneByOne) },
//                { Constants.Media.Breakpoints.Medium, (648, 648, AspectRatio.OneByOne) }
//            };

//            var viewModel = new HighlightViewModel
//            {
//                Header = component.Header,
//                Body = component.Body,
//                ImageIsRight = component.ImageIsRight,
//                Buttons = new List<ButtonModel>()
//            };

//            if (component?.Image?.Content != null)
//            {
//                viewModel.Image = new ImageModel(component.Image.Content as Image, width: 648, height: 648, sourceSets: sourceSets, aspectRatio: AspectRatio.OneByOne);
//            }

//            foreach (var button in component.Buttons)
//            {
//                viewModel.Buttons.Add(_mapper.Map<ButtonModel>(button.Content));
//            }

//            return ViewModel(ViewPath, viewModel);
//        }
//    }
//}
