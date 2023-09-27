using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models;
using Marketing.Core.Models.Umbraco;
using Umbraco.Cms.Core.Mapping;

namespace Marketing.Core.Grid.Controls
{
    public class ButtonGridControl : GridControl
    {
        private readonly IUmbracoMapper _mapper;
        public override string Alias => "componentButton";
        public override string ViewPath => "~/Views/Partials/Button.cshtml";

        public ButtonGridControl(IUmbracoMapper mapper)
        {
            _mapper = mapper;
        }

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (ComponentButton) gridControl.Component;

            return ViewModel(ViewPath, _mapper.Map<ButtonModel>(component));
        }

    }
}
