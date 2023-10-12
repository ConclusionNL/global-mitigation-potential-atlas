using Arlanet.Umbraco.Grid.Base;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;

namespace Arlanet.Umbraco.Grid.Compositions
{
    public class StartupComponent : IComponent
    {
        private readonly Lazy<BlockGridRenderer> _blockListGridRenderer;
        private readonly BlockGridControlResolver _blockGridControlResolver;
        private readonly IRuntimeState _runtimeState;

        public StartupComponent(
            Lazy<BlockGridRenderer> blockListGridRenderer,
            BlockGridControlResolver blockGridControlResolver,
            IRuntimeState runtimeState
        )
        {
            _blockListGridRenderer = blockListGridRenderer;
            _blockGridControlResolver = blockGridControlResolver;
            _runtimeState = runtimeState;
        }

        public void Initialize()
        {
            if (_runtimeState.Level == RuntimeLevel.Run)
            {
                ServiceLocator.Initialize(_blockListGridRenderer.Value, _blockGridControlResolver);
            }
        }

        public void Terminate()
        {
            //Do nothing
        }
    }
}
