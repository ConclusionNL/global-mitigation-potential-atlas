using System.Collections.Generic;
using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models.Umbraco;
using Marketing.Core.Models.ViewModels.Cards;
using Marketing.Core.Models.ViewModels.Grid;
using Umbraco.Cms.Core.Mapping;

namespace Marketing.Core.Grid.Controls
{
    public class ReviewsGridControl : GridControl
    {
        private readonly IUmbracoMapper _mapper;
        public override string ViewPath => "~/Views/Partials/Review.cshtml";
        public override string Alias => "componentReviews";

        public ReviewsGridControl(IUmbracoMapper mapper)
        {
            _mapper = mapper;
        }

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (ComponentReviews)gridControl.Component;

            var testimonials = new List<CardViewModel>();

            foreach (var item in component.Testimonials)
            {
                var card = item.Content as ComponentCard;

                testimonials.Add(_mapper.Map<CardViewModel>(card));
            }

            return ViewModel(ViewPath, new ReviewViewModel
            {
                Testimonials = testimonials, 
                CustomerSatisfactory = component.CustomerSatisfactory, 
                NpsScore = component.NpsScore
            });
        }
    }
}
