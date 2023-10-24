using GMPA.Core.Extensions;
using GMPA.Core.Models;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller.Render
{
    public class CountryController : RenderController
    {
        public CountryController(
            ILogger<CountryController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor
        )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //Do nothing
        }

        public IActionResult Country()
        {
            var country = (Country)CurrentPage!;

            var list = new List<Item>();

            if (country.AccordionItem != null)
            {
                foreach (var accordionItem in country.AccordionItem)
                {
                    if (accordionItem.Content is not AccordionItem item)
                    {
                        continue;
                    }

                    list.Add(new Item
                    {
                        Title = item.Title,
                        BodyText = item.BodyText?.ToHtmlString()
                    });
                }
            }

            var viewModel = new CountryViewModel
            {
                Accordion = list,
                Country = new CountryModel
                {
                    Active = new bool(),
                    Continent = new List<string>()
                }
            };

            viewModel.Country.Active = country.Active;
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
