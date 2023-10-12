namespace GMPA.Core.Models.ViewModels
{
    public class CountryViewModel : MainViewModel
    {
        public CountryModel Country { get; set; }
        public List<Item> Accordion { get; set; }
    }

    public class Item
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
    }
}