using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller
{
    public class CountryController : RenderController
    {
        public CountryController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
            , IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public IActionResult Country()
        {
            var country = (Country)CurrentPage;
            var viewModel = new CountryViewModel()
            {
                Active = new List<string>(),
                Continents = new List<string>(),
            };

            // Get tag Active for country
            foreach (var isActive in country.Active) viewModel.Active.Add(isActive);

            //Get the tag representing the continent the country is located in
            foreach (var continent in country.Continent) viewModel.Continents.Add(continent);

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
