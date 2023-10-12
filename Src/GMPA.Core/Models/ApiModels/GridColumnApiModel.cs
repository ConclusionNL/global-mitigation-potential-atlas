using GMPA.Core.Grid;

namespace GMPA.Core.Models.ApiModels
{
    public class GridColumnApiModel
    {
        public int Width { get; set; }
        public MyColumnSettings Settings { get; set; }
        public List<IGridControlApiModel> Controls { get; set; }
    }
}