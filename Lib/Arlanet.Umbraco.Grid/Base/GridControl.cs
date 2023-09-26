using System;

namespace Arlanet.Umbraco.Grid.Base
{
    public abstract class GridControl
    {
        public const string BASE_VIEWPATH = "~/Views/Partials/CustomGrid/Controls/";

        public virtual string Alias { get; }
        public virtual string ViewPath { get; }

        protected GridControl()
        {
            string alias = GetType().Name.Substring(
                0, 
                GetType().Name.IndexOf("GridControl", StringComparison.InvariantCulture)
            );

            Alias = alias;
            ViewPath = $"{alias}.cshtml";
        }
        
        public abstract GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false);

        protected GridControlViewModel ViewModel(string view, object model = null)
        {
            return new GridControlViewModel
            {
                View = view.StartsWith("~") ? view : BASE_VIEWPATH + view,
                Model = model
            };
        }
    }
}