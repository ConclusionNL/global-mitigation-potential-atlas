using Arlanet.Umbraco.Grid.Base;

namespace Arlanet.Umbraco.Grid
{
    public static class ServiceLocator
    {
        public static BlockGridRenderer BlockListGridRenderer { get; private set; }
        public static BlockGridControlResolver BlockGridControlResolver { get; private set; }

        public static void Initialize(
            BlockGridRenderer blockListGridRenderer,
            BlockGridControlResolver blockGridControlResolver
        )
        {
            BlockListGridRenderer = blockListGridRenderer;
            BlockGridControlResolver = blockGridControlResolver;
        }
    }
}
