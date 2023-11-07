namespace GMPA.Core.Models.ViewModels
{
    public class MainViewModel
    {
        public string FavIcon { get; set; }
        public string PageTitle { get; set; }

        public List<CountryModel> Countries { get; set; }
        public List<CaseModel> Cases { get; set; }
    }
}
