using GMPA.Core.Grid.Enums;
using GMPA.Core.Models.ApiModels;

namespace GMPA.Core.Models.GridViewModels
{
    public class HeaderViewModel : IGridControlApiModel
    {
        public string Alias { get; set; }

        public string Title { get; set; }
        public HeaderSize HeaderSize { get; set; }
    }
}
