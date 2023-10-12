using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Grid.Enums;
using GMPA.Core.Models.GridViewModels;
using GMPA.Core.Models.Umbraco;

namespace GMPA.Core.Grid.Controls
{
    public class HeaderGridControl : GridControl
    {
        public override string ViewPath => "~/Views/Partials/CustomGrid/Controls/Header.cshtml";
        public override string Alias => "header";

        public override GridControlViewModel Render(BlockGridControl gridControl, bool preview = false)
        {
            var component = (Header)gridControl.Component;

            return ViewModel(ViewPath, new HeaderViewModel
            {
                Title = component.Title,
                HeaderSize = string.IsNullOrWhiteSpace(component.HeaderSize)
                    ? HeaderSize.Large
                    : Enum.Parse<HeaderSize>(component.HeaderSize)
            });
        }
    }
}