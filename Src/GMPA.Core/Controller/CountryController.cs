using GMPA.Core.Extensions;
using GMPA.Core.Models;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
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

            var list = new List<Item>();

            foreach (var accordionItem in country.AccordionItem )
            {
                var item = accordionItem.Content as AccordionItem;

                if (item == null)
                {
                    continue;
                }

                list.Add(new Item
                {
                    Title = item.Title,
                    BodyText = item.BodyText?.ToHtmlString()
                });
            }

            var viewModel = new CountryViewModel
            {
                Accordion = list,
                Country = new CountryModel
                {
                    Active = new List<string>(),
                    Continent = new List<string>()
                }
            };

            if (country == null)
            {
                return null;
            }

            if (country.Active != null)
            {
                // Get tag Active for country
                foreach (var isActive in country.Active)
                {
                    viewModel.Country.Active.Add(isActive);
                }
            }

            if (country.Continent != null)
            {
                //Get the tag representing the continent the country is located in
                foreach (var continent in country.Continent.Distinct())
                {
                    viewModel.Country.Continent.Add(continent);
                    Console.WriteLine(continent);
                }
            }

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}
