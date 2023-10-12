using GMPA.Core.Models.ApiModels;
using GMPA.Core.Models.GridViewModels;
using GMPA.Core.Swagger;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace GMPA.Core.Umbraco.Composers
{
    public class SwaggerComposer : IComposer
    {
        //TODO: Solve this in a fully dynamic way...
        private static readonly Dictionary<Type, string> _gridControlMapping = new()
        {
            { typeof(IGridControlApiModel), "none" },
            { typeof(HeaderViewModel), "header" },
            { typeof(ParagraphViewModel), "paragraph" },
            { typeof(TagTitleViewModel), "tagTitle" }
        };

        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.UseOneOfForPolymorphism();

                options.SelectSubTypesUsing(baseType =>
                {
                    if (baseType == typeof(IGridControlApiModel))
                    {
                        return _gridControlMapping.Keys;
                    }

                    return Enumerable.Empty<Type>();
                });

                options.SelectDiscriminatorNameUsing(baseType =>
                {
                    if (baseType == typeof(IGridControlApiModel))
                    {
                        return "alias";
                    }

                    return null;
                });

                options.SelectDiscriminatorValueUsing(type =>
                {
                    _gridControlMapping.TryGetValue(type, out var value);

                    return value;
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GMPA - API"
                });

                options.DocInclusionPredicate((name, description) =>
                {
                    if (description.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return controllerActionDescriptor.ControllerTypeInfo.Namespace.EndsWith($".{name}");
                    }

                    return false;
                });

                options.SchemaFilter<EnumNamesSchemaFilter>();
            });
        }
    }
}
