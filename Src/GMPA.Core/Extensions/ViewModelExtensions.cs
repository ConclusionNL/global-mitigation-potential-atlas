using Arlanet.Umbraco.Grid;
using GMPA.Core.Models;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GMPA.Core.Extensions
{
    //TODO: ViewModelFactory
    public static class ViewModelExtensions
    {
        public static void Build(this MainViewModel model, IPublishedContent content)
        {
            var home = content.AncestorOrSelf<Home>();

            model.PageTitle = content.Name;

            #region countries
            var countries = home
                .Descendant<Countries>()
                .Descendants<Country>();

            model.Countries = new List<CountryModel>();

            foreach (var country in countries)
            {
                model.Countries.Add(new CountryModel
                {
                    Name = country.Name,
                    Continent = country.Continent?.ToList(),
                    Active = country.Active,
                    Url = country.Url(mode: UrlMode.Absolute),
                });
            }

            model.Countries = model.Countries
                .OrderBy(a => a.Name)
                .ToList();
            #endregion

            #region cases

            var cases = home
                .Descendant<Cases>()
                .Descendants<Case>();

            model.Cases = new List<CaseModel>();

            foreach (var caseItem in cases)
            {
                model.Cases.Add(new CaseModel
                {
                    CaseName = caseItem.Name,
                    CaseDescription = caseItem.CaseIntroduction?.ToHtmlString(),
                    CaseTags = caseItem.Tags?.ToList()
                });
            }
            #endregion

            ConstructGrid(model, content);
        }

        private static void ConstructGrid<T>(T viewModel, IPublishedContent node) where T : MainViewModel, new()
        {
            if (node is IGrid grid && viewModel is IGridViewModel gridViewModel)
            {
                gridViewModel.BlockGrid = ServiceLocator.BlockListGridRenderer.MapBlockGridModel(grid.BlockGrid);
            }
        }
    }
}
