//using Arlanet.Umbraco.Grid.Base;
//using Marketing.Core.Models.Umbraco;
//using Marketing.Core.Models.ViewModels;
//using Umbraco.Extensions;

//namespace Marketing.Core.Grid.Controls
//{
//    public class LinkGridControl : GridControl
//    {
//        public override string Alias => "linkDefault";
//        public override string ViewPath => "~/Views/Partials/Link.cshtml";

//        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        {
//            var compontent = (LinkDefault)gridControl.Component;
//            var linkBlock = new LinkViewModel()
//            {
//                Title = compontent.Title ?? "",
//                ExternalLink = compontent.ExternalUrl ?? "",
//                OpenInNewTab = compontent.OpenInNewTab.ToString(),
//            };

//            if(compontent.InternalPage is not null)
//            {
//                linkBlock.InternalLink = compontent.InternalPage.Url();
//            }

//            return ViewModel(ViewPath, linkBlock);
//        }
//    }
//}
