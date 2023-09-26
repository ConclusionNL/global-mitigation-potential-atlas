using Arlanet.Umbraco.Grid.Base;

namespace Arlanet.Umbraco.Grid
{
    public static class ServiceLocator
    {
        public static BlockListGridRenderer BlockListGridRenderer { get; private set; }
        public static BlockListGridControlResolver BlockListGridControlResolver { get; private set; }

        public static void Initialize(
            BlockListGridRenderer blockListGridRenderer,
            BlockListGridControlResolver blockListGridControlResolver
        )
        {
            BlockListGridRenderer = blockListGridRenderer;
            BlockListGridControlResolver = blockListGridControlResolver;
        }
    }
}
