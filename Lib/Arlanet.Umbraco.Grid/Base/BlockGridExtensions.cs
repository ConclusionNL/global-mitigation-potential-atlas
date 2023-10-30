using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arlanet.Umbraco.Grid.Base
{
    public static class BlockGridExtensions
    {
        public static string GetSectionClasses(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetSectionClasses(gridRow);
        }

        public static string GetSectionStyle(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetSectionStyle(gridRow);
        }

        public static string GetContainerClasses(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetContainerClasses(gridRow);
        }

        public static string GetContainerStyle(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetContainerStyle(gridRow);
        }

        public static string GetRowClasses(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetRowClasses(gridRow);
        }

        public static string GetRowStyle(this BlockGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetRowStyle(gridRow);
        }

        public static string GetColumnClasses(this BlockGridRow gridRow, BlockGridColumn gridColumn)
        {
            return ServiceLocator.BlockListGridRenderer.GetColumnClasses(gridRow, gridColumn);
        }

        public static string GetColumnStyle(this BlockGridRow gridRow, BlockGridColumn gridColumn)
        {
            return ServiceLocator.BlockListGridRenderer.GetColumnStyle(gridRow, gridColumn);
        }

        public static IHtmlContent PreviewAttributes(this BlockGridRow gridRow, BlockGrid model)
        {
            return model.Preview
                ? new HtmlString($"data-preview data-preview-id=\"{gridRow.PreviewId}\" data-preview-type=\"row\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent PreviewAttributes(this BlockGridColumn gridColumn, BlockGrid model)
        {
            return model.Preview
                ? new HtmlString($"data-preview data-preview-id=\"{gridColumn.PreviewId}\" data-preview-type=\"column\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent PreviewAttributes(this BlockGridControl gridControl, BlockGrid model)
        {
            return model.Preview
                ? new HtmlString($"data-preview data-preview-id=\"{gridControl.PreviewId}\" data-preview-type=\"control\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent RenderGridControl(this IHtmlHelper htmlHelper, BlockGridControl gridControl, bool preview = false)
        {
            try
            {
                var gridControlViewModel = ServiceLocator.BlockGridControlResolver
                    .Get(gridControl.Alias)
                    ?.Render(gridControl, preview);

                return gridControlViewModel == null
                    ? HtmlString.Empty
                    : htmlHelper.Partial(gridControlViewModel.View, gridControlViewModel.Model);
            }
            catch (Exception ex)
            {
                return new HtmlString($"<h1>{gridControl.Alias}</h1><code><pre>{ex}</pre></code>");
            }
        }
    }
}
