using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGridControlResolver
    {
        private readonly Dictionary<string, GridControl> _gridControls;

        public BlockListGridControlResolver(IServiceProvider serviceProvider)
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

            return _gridControls.TryGetValue(name.ToLower(), out GridControl control) 
                ? control 
                : null;
        }
    }
}