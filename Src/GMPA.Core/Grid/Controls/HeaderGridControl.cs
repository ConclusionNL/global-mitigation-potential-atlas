using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Models.Umbraco;

namespace Marketing.Core.Grid.Controls
{
    public class HeaderGridControl : GridControl
    {
        public class HeaderViewModel
        {
            public string Title { get; set; }
        }

        public override string Alias => "header";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (Header)gridControl.Component;

            return ViewModel(ViewPath, new HeaderViewModel
            {
                Title = component.Title
            });
        }
    }
}
