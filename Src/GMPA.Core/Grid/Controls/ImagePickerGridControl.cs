//using Arlanet.Umbraco.Grid.Base;
//using Umbraco.Extensions;

//namespace Marketing.Core.Grid.Controls
//{
//    public class ImagePickerGridControl : GridControl
//    {
//        //public class ImagePickerViewModel
//        //{
//        //    public string Id { get; set; }
//        //    public List<ImageModel> Images { get; set; }
//        //}

//        //public override string Alias => "componentImagePicker";

//        //public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        //{
//        //    var component = (ComponentImagePicker)gridControl.Component;

//        //    var viewModel = new ImagePickerViewModel
//        //    {
//        //        Images = new List<ImageModel>(),
//        //        Id = System.Guid.NewGuid().ToString("N")
//        //    };

//        //    var sourceSets = new Dictionary<string, (int? Width, int? Height, AspectRatio Ratio)>
//        //    {
//        //        { Media.Breakpoints.ExtraSmall, (456, 342, AspectRatio.FourByThree) },
//        //        { Media.Breakpoints.Medium, (576, 324, AspectRatio.SixteenByNine) },
//        //        { Media.Breakpoints.Large, (775, 436, AspectRatio.SixteenByNine) },
//        //        { Media.Breakpoints.ExtraLarge, (923, 519, AspectRatio.SixteenByNine) },
//        //        { Media.Breakpoints.ExtraExtraLarge, (1073, 604, AspectRatio.SixteenByNine) },
//        //    };

//        //    foreach (var imageItem in component.Images.Select(x => (ImageItem)x.Content))
//        //    {
//        //        if (!(imageItem.Image?.Content is Image image))
//        //        {
//        //            continue;
//        //        }

//        //        var imageModel = new ImageModel(image, 660, 371, AspectRatio.SixteenByNine, sourceSets: sourceSets);

//        //        viewModel.Images.Add(imageModel);
//        //    }

//        //    return ViewModel(ViewPath, viewModel);
//        //}
//    }
//}
