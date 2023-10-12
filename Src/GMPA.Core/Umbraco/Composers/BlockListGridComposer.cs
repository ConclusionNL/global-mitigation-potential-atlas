using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Grid;
using GMPA.Core.Grid.Controls;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace GMPA.Core.Umbraco.Composers
{
    [ComposeAfter(typeof(Arlanet.Umbraco.Grid.Compositions.StartupComposer))]
    public class BlockListGridComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<BlockGridRenderer, MyBlockGridRenderer>();

            ////NOTE: Use AddSingleton to register multiple instances of the same type!
            builder.Services.AddSingleton<GridControl, HeaderGridControl>();
            builder.Services.AddSingleton<GridControl, ParagraphGridControl>();
        }
    }
}
