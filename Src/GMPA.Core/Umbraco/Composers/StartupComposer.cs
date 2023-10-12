using GMPA.Core.Umbraco.Filters;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace GMPA.Core.Umbraco.Composers
{
    public class StartupComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.Configure<UmbracoPipelineOptions>(options =>
            {
                options.AddFilter(new StartupUmbracoPipelineFilter());
            });

            builder.Services.AddCors(options =>
            {
                //TODO: Make this production ready
                var defaultPolicy = new CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .Build();

                options.AddDefaultPolicy(defaultPolicy);
            });
        }
    }
}
