using GMPA.Core.Models.ApiModels;
using Microsoft.AspNetCore.Html;

namespace GMPA.Core.Models.GridViewModels
{
    public class ParagraphViewModel : IGridControlApiModel
    {
        public string Alias { get; set; }

        public IHtmlContent Text { get; set; }
    }
}
