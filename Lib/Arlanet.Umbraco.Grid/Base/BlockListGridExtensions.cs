using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arlanet.Umbraco.Grid.Base
{
    public static class BlockListGridExtensions
    {
        public static string GetSectionClasses(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetSectionClasses(gridRow);
        }

        public static string GetSectionStyle(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetSectionStyle(gridRow);
        }

        public static string GetContainerClasses(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetContainerClasses(gridRow);
        }

        public static string GetContainerStyle(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetContainerStyle(gridRow);
        }

        public static string GetRowClasses(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetRowClasses(gridRow);
        }

        public static string GetRowStyle(this BlockListGridRow gridRow)
        {
            return ServiceLocator.BlockListGridRenderer.GetRowStyle(gridRow);
        }

        public static string GetColumnClasses(this BlockListGridRow gridRow, BlockListGridColumn gridColumn)
        {
            return ServiceLocator.BlockListGridRenderer.GetColumnClasses(gridRow, gridColumn);
        }

        public static string GetColumnStyle(this BlockListGridRow gridRow, BlockListGridColumn gridColumn)
        {
            return ServiceLocator.BlockListGridRenderer.GetColumnStyle(gridRow, gridColumn);
        }

        public static IHtmlContent PreviewAttributes(this BlockListGridRow gridRow, BlockListGrid model)
        {
            return model.Preview
                ? new HtmlString($"data-preview data-preview-id=\"{gridRow.PreviewId}\" data-preview-type=\"row\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent PreviewAttributes(this BlockListGridColumn gridColumn, BlockListGrid model)
        {
            return model.Preview
                ? new HtmlString($"data-preview data-preview-id=\"{gridColumn.PreviewId}\" data-preview-type=\"column\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent PreviewAttributes(this BlockListGridControl gridControl, BlockListGrid model)
        {
            return model.Preview 
                ? new HtmlString($"data-preview data-preview-id=\"{gridControl.PreviewId}\" data-preview-type=\"control\"")
                : HtmlString.Empty;
        }

        public static IHtmlContent RenderGridControl(this IHtmlHelper htmlHelper, BlockListGridControl gridControl, bool preview = false)
        {
            try
            {
                var gridControlViewModel = ServiceLocator.BlockListGridControlResolver
                    .Get(gridControl.Render)
                    ?.Render(gridControl, preview);

                return gridControlViewModel == null ? 
                    HtmlString.Empty : 
                    htmlHelper.Partial(gridControlViewModel.View, gridControlViewModel.Model);
            }
            catch(Exception ex)
            {
                return new HtmlString($"<h1>{gridControl.Render}</h1><code><pre>{ex}</pre></code>");
            }
        }
    }
}
