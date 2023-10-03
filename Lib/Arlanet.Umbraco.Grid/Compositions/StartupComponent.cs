using Arlanet.Umbraco.Grid.Base;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;

namespace Arlanet.Umbraco.Grid.Compositions
{
    public class StartupComponent : IComponent
    {
        private readonly Lazy<BlockGridRenderer> _blockListGridRenderer;
        private readonly BlockListGridControlResolver _blockListGridControlResolver;
        private readonly IRuntimeState _runtimeState;

        public StartupComponent(
            Lazy<BlockGridRenderer> blockListGridRenderer,
            BlockListGridControlResolver blockListGridControlResolver,
            IRuntimeState runtimeState
        )
        {
            _blockListGridRenderer = blockListGridRenderer;
            _blockListGridControlResolver = blockListGridControlResolver;
            _runtimeState = runtimeState;
        }

        public void Initialize()
        {
            if (_runtimeState.Level == RuntimeLevel.Run)
            {
                ServiceLocator.Initialize(_blockListGridRenderer.Value, _blockListGridControlResolver);
            }
        }

        public void Terminate()
        {
            //Do nothing
        }
    }
}
