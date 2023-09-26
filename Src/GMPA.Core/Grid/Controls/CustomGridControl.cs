//using Arlanet.Umbraco.Grid.Base;
//using Microsoft.AspNetCore.Html;

//namespace Marketing.Core.Grid.Controls
//{
//    public class CustomGridControl : GridControl
//    {
//        public class CustomViewModel
//        {
//            public string Id { get; set; }
//            public IHtmlContent Css { get; set; }
//            public IHtmlContent Html { get; set; }
//            public IHtmlContent Javascript { get; set; }
//            public bool EnableShadowRoot { get; set; }
//        }

//        public override string Alias => "componentCustom";

//        //public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        //{
//        //    var component = (ComponentCustom)gridControl.Component;

//        //    return ViewModel(ViewPath, new CustomViewModel
//        //    {
//        //        Id = component.Key.ToString("N"),
//        //        Css = new HtmlString(component.Css),
//        //        Html = new HtmlString(component.Html),
//        //        Javascript = new HtmlString(component.Javascript),
//        //        EnableShadowRoot = component.EnableShadowRoot
//        //    });
//        //}
//    }
//}
