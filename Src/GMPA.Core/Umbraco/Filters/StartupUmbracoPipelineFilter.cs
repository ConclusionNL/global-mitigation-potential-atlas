using Microsoft.AspNetCore.Builder;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace GMPA.Core.Umbraco.Filters
{
    public class StartupUmbracoPipelineFilter : UmbracoPipelineFilter
    {
        public StartupUmbracoPipelineFilter()
            : base(nameof(StartupUmbracoPipelineFilter))
        {
#if DEBUG
            PreRouting = builder =>
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GMPA API - v1");
                });
            };
#endif

            PostPipeline = builder =>
            {
                builder.UseCors();
            };
        }
    }
}