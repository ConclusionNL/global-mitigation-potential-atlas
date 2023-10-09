using GMPA.Core.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GMPA.Core.Models.ViewModels
{
	public class MainViewModel
	{
		public string PageTitle { get; set; }

        [Obsolete]
		public IPublishedContent Content { get; set; }
        public List<CountryModel> Countries { get; set; }

    }
}