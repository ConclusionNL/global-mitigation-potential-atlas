using GMPA.Core.Models.ApiModels;

namespace GMPA.Core.Models.GridViewModels
{
    public class TagTitleViewModel : IGridControlApiModel
    {
        public string Alias { get; set; }

        public string TagTitle { get; set; }
    }
}
