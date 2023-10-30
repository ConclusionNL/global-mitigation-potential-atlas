using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Arlanet.Umbraco.Grid.Other
{
    public class BlockListGridServerVariablesHandler : INotificationHandler<ServerVariablesParsingNotification>
    {
        private readonly IOptions<BlockGridSettings> _blockListGridSettings;

        public BlockListGridServerVariablesHandler(IOptions<BlockGridSettings> blockListGridSettings)
        {
            _blockListGridSettings = blockListGridSettings;
        }

        public void Handle(ServerVariablesParsingNotification notification)
        {
            notification.ServerVariables.Add("Arlanet.Umbraco.Grid", new Dictionary<string, object>());
        }
    }
}
