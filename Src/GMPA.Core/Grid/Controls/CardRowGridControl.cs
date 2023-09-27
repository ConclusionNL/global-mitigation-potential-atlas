using System.Collections.Generic;
using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models.Umbraco;
using Marketing.Core.Models.ViewModels.Cards;
using Marketing.Core.Models.ViewModels.Grid;
using Umbraco.Cms.Core.Mapping;

namespace Marketing.Core.Grid.Controls
{
    public class CardRowGridControl: GridControl
    {
        private readonly IUmbracoMapper _mapper;
        public override string Alias => "componentCardRow";
        public override string ViewPath => "~/Views/Partials/CardRow.cshtml";

        public CardRowGridControl(IUmbracoMapper mapper)
        {
            _mapper = mapper;
        }

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var cardRow = new CardRowViewModel
            {
                Cards = new List<CardViewModel>()
            };

            var component = (ComponentCardRow)gridControl.Component;

            foreach (var item in component.Cards)
            {
                var card = item.Content as ComponentCard;

                cardRow.Cards.Add(_mapper.Map<CardViewModel>(card));
            }

            cardRow.Slider = component.Slider;

            var className = Variant(component.Variant);
            cardRow.Variant = className;


            return ViewModel(ViewPath, cardRow);
        }

        public string Variant(string variant)
        {
            var className = new string("");

            switch (variant)
            {
                case Constants.Content.Components.Card.Variants.USP.Type:
                    className = Constants.Content.Components.Card.Variants.USP.Row;
                    break;

                case Constants.Content.Components.Card.Variants.Expertise.Type:
                    className = Constants.Content.Components.Card.Variants.Expertise.Row;
                    break;

                case Constants.Content.Components.Card.Variants.Case.Type:
                    className = Constants.Content.Components.Card.Variants.Case.Row;
                    break;

                case Constants.Content.Components.Card.Variants.Review.Type:
                    className = Constants.Content.Components.Card.Variants.Review.Row;
                    break;

                default:
                    className= "card";
                    break;
            }

            return className;
        }
    }
}
