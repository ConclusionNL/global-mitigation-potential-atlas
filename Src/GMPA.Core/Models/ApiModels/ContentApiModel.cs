namespace GMPA.Core.Models.ApiModels
{
    public class ContentApiModel
    {
        public Guid Guid { get; set; }
        public string Type { get; set; }
        public GridApiModel Body { get; set; }
    }
}
