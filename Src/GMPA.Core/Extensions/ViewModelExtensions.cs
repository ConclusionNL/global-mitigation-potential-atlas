﻿using Arlanet.Umbraco.Grid;
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
            model.Content = content;
            #region countries
            var countries = home
                .Descendant<Countries>()
                .Descendants<Country>();

            model.Countries = new List<Country>();

            foreach (var country in countries)
            {
                model.Countries.Add(country);
            }

            model.Countries = model.Countries
                .OrderBy(a => a.Name)
                .ToList();

            #endregion
            
            ConstructGrid(model, content);
        }

        private static void ConstructGrid<T>(T viewModel, IPublishedContent node) where T : MainViewModel, new()
        {
            if (node is IGrid grid && viewModel is IGridViewModel gridViewModel)
            {
                gridViewModel.BlockGrid = ServiceLocator.BlockListGridRenderer.MapBlockListModel(grid.BlockGrid);
            }
        }
    }
}
