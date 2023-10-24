using GMPA.Core.Extensions;
using GMPA.Core.Models;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

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

            var viewModel = new CountryViewModel
            {
                Country = new CountryModel
                {
                    Active = new bool(),
                    Continent = new List<string>()
                },

                NormalBlockList = country.NormalTextBlockList?
                    .Select(a => a.Content as CountryDetailsNormalContentBlockList)
                    .Select(b => new CountryViewModel.NormalBlockListItem
                    {
                        Title = b.Title,
                        BodyText = b.BodyText.ToHtmlString(),
                        Url = b.Link,
                        ImageUrl = b.Image?.Url()
                    })
                    .ToList(),

                CollapsibleBlockList = country.AccordionItem?
                .Select(a => a.Content as AccordionItem)
                .Select(b => new CountryViewModel.CollapsibleBlockListItem
                {
                    Title = b.Title,
                    BodyText = b.BodyText.ToHtmlString(),
                    ImageUrl = b.Image?.Url()
                })
            .ToList()
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
