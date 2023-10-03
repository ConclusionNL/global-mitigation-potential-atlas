using Arlanet.Umbraco.Grid.Base;
using Arlanet.Umbraco.Grid.Other;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Arlanet.Umbraco.Grid.Compositions
{
    public class StartupComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Components().Append<StartupComponent>();

            builder.Services.AddSingleton<BlockGridRenderer>();
            builder.Services.AddSingleton<BlockListGridControlResolver>();

            builder.Services
                .AddOptions<BlockListGridSettings>()
                .Bind(builder.Config.GetSection(Constants.BlockListGridConfigurationKey))
                .ValidateDataAnnotations();

            builder.AddNotificationHandler<ServerVariablesParsingNotification, BlockListGridServerVariablesHandler>();
        }
    }
}
