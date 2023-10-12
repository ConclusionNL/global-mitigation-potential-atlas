using GMPA.Core.Grid;

namespace GMPA.Core.Models.ApiModels
{
    public class GridRowApiModel
    {
        public MyRowSettings Settings { get; set; }
        public List<GridColumnApiModel> Columns { get; set; }
    }
}