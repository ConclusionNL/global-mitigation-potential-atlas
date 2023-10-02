//using Arlanet.Umbraco.Grid.Base;
//using Marketing.Core.Models.Umbraco;
//using Marketing.Core.Models.ViewModels;
//using Marketing.Core.Models.ViewModels.Grid;

//namespace Marketing.Core.Grid.Controls
//{
//    public class FAQBlockGridControl : GridControl
//    {
//        public override string Alias => "fAQGrid";
//        public override string ViewPath => "~/Views/Partials/FAQGridBlock.cshtml";

//        public FAQBlockGridControl()
//        {

//        }

//        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        {
//            var faqGrid = new FAQGridViewModel();

//            var compontent = (FAqgrid)gridControl.Component;

//            // Loop through the different FaqBlocks
//            foreach (var block in compontent.Blocks)
//            {
//                var faqBlock = (FAqblock)block.Content;
//                if(faqBlock is null) { continue; }
                
                
//                var faqBlockViewModel = new FAQBlockViewModel();
                
//                // Loop through the questions in a faq block
//                foreach(var faq in faqBlock.Questions)
//                {
//                    var content = faq.Content as FAQ;
//                    FAQViewModel faqViewModel = new()
//                    {
//                        Question = content.Question.ToString(),
//                        Answer = content.Answer
//                    };
//                    faqBlockViewModel.Questions.Add(faqViewModel);
                    
//                }
//                faqBlockViewModel.ButtonText = faqBlock.ButtonText;
//                faqBlockViewModel.Header = faqBlock.Header;

//                faqGrid.Blocks.Add(faqBlockViewModel);
//            }
//            return ViewModel(ViewPath, faqGrid);
//        }
//    }
//}
