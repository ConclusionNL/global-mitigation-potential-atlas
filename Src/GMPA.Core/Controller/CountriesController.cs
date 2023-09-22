using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller
{
    public class CountriesController : RenderController
    {
        public CountriesController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
            , IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public IActionResult Countries()
        {
            var countries = (Countries)CurrentPage;
            var viewModel = new CountriesViewModel()
            {
                Countries = new List<Country>()
            };

            foreach (var country in countries.Children.OfType<Country>())
            {
                viewModel.Countries.Add(country);
                viewModel.Countries = viewModel.Countries.OrderBy(a => a.Name).ToList();
                Console.WriteLine(country);
            }
            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
