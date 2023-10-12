namespace GMPA.Web
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _config;

		public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
		{
			_env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
			_config = config ?? throw new ArgumentNullException(nameof(config));
		}

        public void ConfigureServices(IServiceCollection services)
		{
			services.AddUmbraco(_env, _config)
				.AddBackOffice()
				.AddWebsite()
				.AddDeliveryApi()
				.AddComposers()
				.Build();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
            }

            app.UseUmbraco()
				.WithMiddleware(u =>
				{
					u.UseBackOffice();
					u.UseWebsite();
				})
				.WithEndpoints(u =>
				{
					u.UseInstallerEndpoints();
					u.UseBackOfficeEndpoints();
					u.UseWebsiteEndpoints();
				});

            app.UseSpa(spa =>
                spa.UseProxyToSpaDevelopmentServer("https://localhost:3000")
            );
        }
	}
}
