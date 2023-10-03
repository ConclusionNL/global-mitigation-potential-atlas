﻿using GMPA.Core.Extensions;
using GMPA.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller;

public class CasesController : RenderController
{
    public CasesController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine
        , IUmbracoContextAccessor umbracoContextAccessor) 
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
    }
    
    public IActionResult Cases()
    {
        var viewModel = new CasesViewModel();
            
        viewModel.Build(CurrentPage);

        return CurrentTemplate(viewModel);
    }
}