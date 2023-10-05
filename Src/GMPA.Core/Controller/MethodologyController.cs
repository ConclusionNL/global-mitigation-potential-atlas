﻿using GMPA.Core.Extensions;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller
{
    public class MethodologyController : RenderController
    {
        public MethodologyController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
            , IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public IActionResult Methodology()
        {
            var aboutGmpa = (Methodology)CurrentPage;
            var viewModel = new MethodologyViewModel()
            {
                BodyText = new HtmlString(aboutGmpa.BodyText.ToHtmlString() ?? string.Empty)
            };

            viewModel.Build(CurrentPage);

            return CurrentTemplate(viewModel);
        }
    }
}