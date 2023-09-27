using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models.Umbraco;

namespace Marketing.Core.Grid.Controls
{
    public class HeaderGridControl : GridControl
    {
        public class HeaderViewModel
        {
            public string Title { get; set; }
        }

        public override string Alias => "componentHeader";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (ComponentHeader)gridControl.Component;

            return ViewModel(ViewPath, new HeaderViewModel
            {
                Title = component.Title
            });
        }
    }
}
