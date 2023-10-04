using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Html;

namespace GMPA.Core.Grid.Controls
{
    public class SelectionTestGridControl : GridControl
    {
        public override string ViewPath => "~/Views/Partials/CustomGrid/Controls/SelectionTest.cshtml";
        public override string Alias => "selectionTest";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (SelectionTest)gridControl.Component;

            return ViewModel(ViewPath, new SelectionTestViewModel
            {
                SelectionTest = new HtmlString(component.TestSelect)
            });
        }
    }
}
