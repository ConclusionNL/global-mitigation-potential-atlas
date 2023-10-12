using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Grid.Enums;
using GMPA.Core.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GMPA.Core.Grid
{
    public class MyBlockGridRenderer : BlockGridRenderer
    {
        public override string GetSectionClasses(BlockGridRow gridRow)
        {
            var settings = (MyRowSettings)gridRow.Settings;

            var classes = new List<string> { "grid-row" };
            classes.AddRange(settings.CssClasses);

            switch (settings.PaddingTop)
            {
                case Padding.None:
                    break;
                case Padding.Single:
                    classes.Add("whitespace-top");
                    break;

                case Padding.Double:
                    classes.Add("whitespace-top--double");
                    break;

                case Padding.Big:
                    classes.Add("whitespace-top--big");
                    break;
            }

            switch (settings.PaddingBottom)
            {
                case Padding.None:
                    break;
                case Padding.Single:
                    classes.Add("whitespace-bottom");
                    break;

                case Padding.Double:
                    classes.Add("whitespace-bottom--double");
                    break;

                case Padding.Big:
                    classes.Add("whitespace-bottom--big");
                    break;
            }

            return string.Join(" ", classes);
        }

        public override string GetSectionStyle(BlockGridRow gridRow)
        {
            var settings = (MyRowSettings)gridRow.Settings;

            if (settings.TextColor != null)
            {
                return $"color: {settings.TextColor};";
            }

            return string.Empty;
        }

        public override string GetContainerClasses(BlockGridRow gridRow)
        {
            var settings = (MyRowSettings)gridRow.Settings;

            var classes = new List<string>
            {
                settings.IsFullWidth
                    ? "container-fluid"
                    : "container"
            };

            if (settings.IsFullHeight)
            {
                classes.Add("full-height");
            }

            if (settings.VerticalAlignment != Alignment.None)
            {
                classes.Add("d-flex");
            }

            return string.Join(" ", classes);
        }

        public override string GetRowClasses(BlockGridRow gridRow)
        {
            var settings = (MyRowSettings)gridRow.Settings;

            var classes = new List<string> { "row" };

            if (settings.VerticalAlignment != Alignment.None)
            {
                classes.Add("h-100");
            }

            switch (settings.VerticalAlignment)
            {
                case Alignment.Start:
                    classes.Add("mb-auto");
                    break;
                case Alignment.Center:
                    classes.Add("m-auto");
                    break;
                case Alignment.End:
                    classes.Add("mt-auto");
                    break;
            }

            // We control row whitespace on section level
            switch (settings.PaddingTop)
            {
                default:
                    classes.Add("pt-3");
                    break;
            }

            switch (settings.PaddingBottom)
            {
                default:
                    classes.Add("pb-3");
                    break;
            }

            return string.Join(" ", classes);
        }

        public override string GetColumnClasses(BlockGridRow gridRow, BlockGridColumn gridColumn)
        {
            var rowSettings = (MyRowSettings)gridRow.Settings;
            var columnSettings = (MyColumnSettings)gridColumn.Settings;

            var classes = new List<string>
            {
                "col-12",
                $"col-md-{gridColumn.Width}"
            };

            classes.AddRange(columnSettings.CssClasses);

            if (columnSettings.HorizontalAlignment != Alignment.None)
            {
                switch (columnSettings.HorizontalAlignment)
                {
                    case Alignment.Start:
                        classes.Add("text-start");
                        break;
                    case Alignment.Center:
                        classes.Add("text-center");
                        break;
                    case Alignment.End:
                        classes.Add("text-end");
                        break;
                }
            }
            else
            {
                switch (rowSettings.HorizontalAlignment)
                {
                    case Alignment.Start:
                        classes.Add("text-start");
                        break;
                    case Alignment.Center:
                        classes.Add("text-center");
                        break;
                    case Alignment.End:
                        classes.Add("text-end");
                        break;
                }
            }

            switch (columnSettings.VerticalAlignment)
            {
                case Alignment.Start:
                    classes.Add("mb-auto");
                    break;
                case Alignment.Center:
                    classes.Add("m-auto");
                    break;
                case Alignment.End:
                    classes.Add("mt-auto");
                    break;
            }

            return string.Join(" ", classes);
        }

        public override BlockGridRow CreateRow(List<BlockGridColumn> columns, IPublishedElement settings)
        {
            var baseRow = base.CreateRow(columns, settings);

            var mySettings = (RowSettings)settings;

            baseRow.Settings = new MyRowSettings
            {
                CssClasses = mySettings.CssClasses?.ToList() ?? new List<string>(),
                HorizontalAlignment = string.IsNullOrWhiteSpace(mySettings.HorizontalAlignment)
                    ? Alignment.None
                    : Enum.Parse<Alignment>(mySettings.HorizontalAlignment)
            };

            /*baseRow.Settings = new MyRowSettings
            {
                Background = new MyBackgroundSettings
                {
                    BackgroundColor = mySettings.BackgroundColor,
                    BackgroundImageUrl = mySettings.BackgroundImage.MediaUrl(),
                    BackgroundBlur = mySettings.BackgroundBlur
                },

                TextColor = string.IsNullOrWhiteSpace(mySettings.TextColor)
                    ? null
                    : mySettings.TextColor,
                CssClasses = mySettings.CssClasses,

                IsFullWidth = mySettings.IsFullWidth,
                IsFullHeight = mySettings.IsFullHeight,

                Anchor = mySettings.Anchor,
                HorizontalAlignment = string.IsNullOrWhiteSpace(mySettings.HorizontalAlignment)
                    ? Alignment.None
                    : Enum.Parse<Alignment>(mySettings.HorizontalAlignment),
                VerticalAlignment = string.IsNullOrWhiteSpace(mySettings.VerticalAlignment)
                    ? Alignment.None
                    : Enum.Parse<Alignment>(mySettings.VerticalAlignment),
                PaddingTop = string.IsNullOrWhiteSpace(mySettings.PaddingTop)
                    ? Padding.Double
                    : Enum.Parse<Padding>(mySettings.PaddingTop),
                PaddingBottom = string.IsNullOrWhiteSpace(mySettings.PaddingBottom)
                    ? Padding.Double
                    : Enum.Parse<Padding>(mySettings.PaddingBottom)
            };*/

            return baseRow;
        }

        public override BlockGridColumn CreateColumn(int width, IPublishedElement settings, List<BlockGridControl> controls)
        {
            var baseColumn = base.CreateColumn(width, settings, controls);

            var mySettings = (ColumnSettings)settings;

            baseColumn.Settings = new MyColumnSettings
            {
                CssClasses = mySettings.CssClasses?.ToList() ?? new List<string>()
            };

            /*var mySettings = (ColumnSettings)settings;

            baseColumn.Settings = new MyColumnSettings
            {
                CssClasses = mySettings.CssClasses,
                HorizontalAlignment = string.IsNullOrWhiteSpace(mySettings.HorizontalAlignment)
                    ? Alignment.None
                    : Enum.Parse<Alignment>(mySettings.HorizontalAlignment),
                VerticalAlignment = string.IsNullOrWhiteSpace(mySettings.VerticalAlignment)
                    ? Alignment.None
                    : Enum.Parse<Alignment>(mySettings.VerticalAlignment)
            };*/

            return baseColumn;
        }
    }
}
