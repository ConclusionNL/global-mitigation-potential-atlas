using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Models.GridViewModels;
using GMPA.Core.Models.Umbraco;
using Microsoft.AspNetCore.Html;

namespace GMPA.Core.Grid.Controls
{
    public class ParagraphGridControl : GridControl
    {
        public override string ViewPath => "~/Views/Partials/CustomGrid/Controls/Paragraph.cshtml";
        public override string Alias => "paragraph";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (Paragraph)gridControl.Component;

            return ViewModel(ViewPath, new ParagraphViewModel
            {
                Text = new HtmlString(component.Text.ToHtmlString())
            });
        }
    }
}
