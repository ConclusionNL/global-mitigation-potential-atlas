//using System.Collections.Generic;
//using Arlanet.Umbraco.Grid.Base;
//using Marketing.Core.Models.Media;
//using Marketing.Core.Models.Umbraco;
//using Marketing.Core.Models.ViewModels.Grid;
//using Marketing.Core.Services;

//namespace Marketing.Core.Grid.Controls
//{
//    public class SocialsGridControl : SonicGridControl
//    {
//        public override string Alias => "componentSocials";
//        public override string ViewPath => "~/Views/Partials/SocialCta.cshtml";

//        public SocialsGridControl(IRequestService requestService) : base(requestService)
//        {

//        }

//        //public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        //{
//        //    Initialize();

//        //    var component = (ComponentSocials)gridControl.Component;

//        //    var viewModel = new SocialsViewModel
//        //    {
//        //        Header = component.Header,
//        //        SocialMediaItems = new List<SocialsItem>()
//        //    };

//        //    foreach (var socialsBlock in Home.Socials)
//        //    {
//        //        var item = (Socials)socialsBlock.Content;

//        //        var image = item.Image.Content as Image;

//        //        var sourceSets = new Dictionary<string, (int? Width, int? Height, AspectRatio Ratio)>
//        //        {
//        //            { Constants.Media.Breakpoints.ExtraSmall, (226, 200, AspectRatio.OneByOne) },
//        //            { Constants.Media.Breakpoints.Small, (200, 230, AspectRatio.OneByOne) },
//        //            { Constants.Media.Breakpoints.Medium, (250, 275, AspectRatio.OneByOne) },
//        //            { Constants.Media.Breakpoints.Large, (306, 275, AspectRatio.OneByOne) }
//        //        };

//        //        var socialsItem = new SocialsItem
//        //        {
//        //            Link = item.Link,
//        //            FooterHeader = item.Header,
//        //            FooterSubHeader = item.SubHeader,
//        //            Icon = IconClasses(item.Icon),
//        //            Image = new ImageModel(image)
//        //        };

//        //        viewModel.SocialMediaItems.Add(socialsItem);
//        //    }


//        //    return ViewModel(ViewPath, viewModel);
//        //}

//        //public string IconClasses(string icon)
//        //{
//        //    switch (icon)
//        //    {
//        //        case Constants.Content.Components.SocialsIcon.Facebook.Type:
//        //            icon = Constants.Content.Components.SocialsIcon.Facebook.Class;
//        //            break;
//        //        case Constants.Content.Components.SocialsIcon.Instagram.Type:
//        //            icon = Constants.Content.Components.SocialsIcon.Instagram.Class;
//        //            break;
//        //        case Constants.Content.Components.SocialsIcon.YouTube.Type:
//        //            icon = Constants.Content.Components.SocialsIcon.YouTube.Class;
//        //            break;
//        //        case Constants.Content.Components.SocialsIcon.LinkedIn.Type:
//        //            icon = Constants.Content.Components.SocialsIcon.LinkedIn.Class;
//        //            break;

//        //        default:
//        //            icon = string.Empty;
//        //            break;
//        //    }

//        //    return icon;
//        //} 

//    }
//}
