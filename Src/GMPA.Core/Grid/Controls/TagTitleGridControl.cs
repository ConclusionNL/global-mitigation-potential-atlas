using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Models.GridViewModels;
using GMPA.Core.Models.Umbraco;

namespace GMPA.Core.Grid.Controls
{
    public class TagTitleGridControl : GridControl
    {
        public override string ViewPath => "~/Views/Partials/CustomGrid/Controls/TagTitle.cshtml";
        public override string Alias => "tagTitle";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (TagTitle)gridControl.Component;

            return ViewModel(ViewPath, new TagTitleViewModel
            {
                TagTitle = component.TagTitleText
            });
        }
    }
}
