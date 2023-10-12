using Microsoft.Extensions.DependencyInjection;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridControlResolver
    {
        private readonly Dictionary<string, GridControl> _gridControls;

        public BlockGridControlResolver(IServiceProvider serviceProvider)
        {
            var gridControls = serviceProvider
                .GetServices<GridControl>()
                .ToList();

            _gridControls = gridControls.ToDictionary(x => x.Alias.ToLower());
        }

        public GridControl Get(string name)
        {
            if (name == null)
            {
                return null;
            }

            return _gridControls.TryGetValue(name.ToLower(), out var control) 
                ? control 
                : null;
        }
    }
}