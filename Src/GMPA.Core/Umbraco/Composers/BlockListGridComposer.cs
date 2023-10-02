using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Grid;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace Marketing.Core.Umbraco.Composing.Composers
{
    [ComposeAfter(typeof(Arlanet.Umbraco.Grid.Compositions.StartupComposer))]
    public class BlockListGridComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //builder.Services.AddUnique<BlockListGridRenderer, MyBlockListGridRenderer>();

            ////NOTE: Use AddSingleton to register multiple instances of the same type!
            //builder.Services.AddSingleton<GridControl, HeaderGridControl>();
            //builder.Services.AddSingleton<GridControl, MarkdownGridControl>();
            //builder.Services.AddSingleton<GridControl, CustomGridControl>();
            //builder.Services.AddSingleton<GridControl, ContentPickerGridControl>();
            //builder.Services.AddSingleton<GridControl, ImagePickerGridControl>();
            //builder.Services.AddSingleton<GridControl, ButtonGridControl>();
            //builder.Services.AddSingleton<GridControl, CardGridControl>();
            //builder.Services.AddSingleton<GridControl, CardRowGridControl>();
            //builder.Services.AddSingleton<GridControl, MasonryGridControl>();
            //builder.Services.AddSingleton<GridControl, BrandSliderGridControl>();
            //builder.Services.AddSingleton<GridControl, ReviewsGridControl>();
            //builder.Services.AddSingleton<GridControl, EmbedGridControl>();
            //builder.Services.AddSingleton<GridControl, SocialsGridControl>();
            //builder.Services.AddSingleton<GridControl, HighlightGridControl>();
            //builder.Services.AddSingleton<GridControl, FAQBlockGridControl>();
            //builder.Services.AddSingleton<GridControl, LinkGridControl>();
            //builder.Services.AddSingleton<GridControl, MarkdownCardGridControl>();
            //builder.Services.AddSingleton<GridControl, CaseCarouselGridControl>();
        }
    }
}
