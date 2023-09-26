//using Arlanet.Umbraco.Grid.Base;

//namespace Marketing.Core.Grid.Controls
//{
//    public class MasonryGridControl : GridControl
//    {
//        public override string ViewPath => "~/Views/Partials/Masonry.cshtml";
//        public override string Alias => "componentMasonry";

//        public MasonryGridControl()
//        {

//        }

//        //public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
//        //{
//        //    var component = (ComponentMasonry)gridControl.Component;

//        //    var cards = new List<CardViewModel>();

//        //    foreach (var item in component.Cards)
//        //    {
//        //        var cardComponent = (ComponentCard)item.Content;

//        //        var cardViewModel = new CardViewModel();

//        //        //Masonry is always an Expertise block
//        //        CardMapDefinition.MapCard(ref cardViewModel, cardComponent.CardTitle, cardComponent.CardSubTitle,
//        //            Constants.Content.Components.Card.Variants.Expertise.Type, cardComponent.CardImage,
//        //            cardComponent.CardButton, cardComponent.CardExcerpt);

//        //        cards.Add(cardViewModel);
//        //    }

//        //    return ViewModel(ViewPath, new MasonryViewModel { Cards = cards });
//        //}
//    }
//}
