//using Arlanet.Umbraco.Grid.Base;
//using Marketing.Core.Models.Umbraco;
//using Umbraco.Extensions;

//namespace Marketing.Core.Grid.Controls
//{
//    public class ContentPickerGridControl : GridControl
//    {
//        public class ContentPickerViewModel
//        {
//            public string View { get; set; }
//            public object ViewModel { get; set; }
//        }

//        public class ContentPickerDefaultViewModel
//        {
//            public string Title { get; set; }
//            public string Url { get; set; }
//        }

//        public override string Alias => "componentContentPicker";

//        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        {
//            var component = (ComponentContentPicker)gridControl.Component;

//            var viewModel = GetViewModel(component);

//            return ViewModel(ViewPath, viewModel);
//        }

//        private ContentPickerViewModel GetViewModel(ComponentContentPicker component)
//        {
//            var viewModel = new ContentPickerViewModel();

//            switch (component.Content.ContentType.Alias)
//            {
//                case Default.ModelTypeAlias:
//                    viewModel.View = Default.ModelTypeAlias;
//                    viewModel.ViewModel = GetDefaultViewModel((Default)component.Content);
//                    break;
//            }

//            return viewModel;
//        }

//        public static ContentPickerDefaultViewModel GetDefaultViewModel(Default content)
//        {
//            return new ContentPickerDefaultViewModel
//            {
//                Title = content.Name,
//                Url = content.Url()
//            };
//        }
//    }
//}
