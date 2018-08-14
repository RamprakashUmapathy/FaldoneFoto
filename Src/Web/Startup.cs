using DevExpress.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Kasanova.Common.ApplicationCore.Interfaces;
using Infrastructure.Logging;
using AutoMapper;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Web.ViewModels;
using Microsoft.AspNetCore.Localization;

namespace DevExpressStarterProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Cache
            services.AddMemoryCache();

            //Inject config info using Singleton
            services.AddSingleton<IConfiguration>(Configuration);

            // Logging
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            // IoC for repositories
            services.AddScoped<IUnitOfWork, DataContext>();
            services.AddScoped<IKeyItemValueRepository, KeyItemValueRepository>();
            services.AddScoped<IArticleRepository, ArticleRepositoryCache>();

            services.AddMvc();

            //AutoMapper
            services.AddAutoMapper();

            //DevExpress Controls
            services.AddDevExpressControls();

            //Culture
            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var defCulture = new RequestCulture("it-IT", "it-IT");
                opt.DefaultRequestCulture = new RequestCulture("it-IT", "it-IT");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDevExpressControls();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(name: "ShopSignApi",
                                template: "api/shopsigns",
                                defaults: new
                                {
                                    controller = "Articles",
                                    action = "GetShopSignsAsync"
                                });

                routes.MapRoute(name: "CategoryApi",
                                template: "api/shopsigns/{shopsignid}/categories",
                                defaults: new
                                {
                                    controller = "Articles",
                                    action = "GetCategoriesAsync"
                                });

                routes.MapRoute(name: "CategoryByIdApi",
                                template: "api/shopsigns/{shopsignid}/categories/{categoryId}",
                                defaults: new
                                {
                                    controller = "Articles",
                                    action = "GetCategoriesByIdAsync"
                                });

                routes.MapRoute(name: "FamilyApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetFamiliesAsync"
                    });

                routes.MapRoute(name: "FamilyByIdApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetFamiliesByIdAsync"
                    });

                routes.MapRoute(name: "SeriesApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetSeriesAsync"
                    });

                routes.MapRoute(name: "SeriesByIdApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetSeriesByIdAsync"
                    });

                routes.MapRoute(name: "Level1Api",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetLevel1Async"
                    });

                routes.MapRoute(name: "Level1ByIdApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetLevel1ByIdAsync"
                    });

                routes.MapRoute(name: "Level2Api",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetLevel2Async"
                    });

                routes.MapRoute(name: "Level2ByIdApi",
                    template: "api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetLevel2ByIdAsync"
                    });

                routes.MapRoute(name: "StyleApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetStylesAsync"
                    });

                routes.MapRoute(name: "StyleByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles/{styleid}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetStylesByIdAsync"
                    });

                routes.MapRoute(name: "ArticleApi",
                    template: "api/articles/{shopsignid}/{categoryid}/{familyid?}/{seriesid?}/{level1id?}/{level2id?}/{styleid?}",
                    defaults: new
                    {
                        controller = "Articles",
                        action = "GetArticlesAsync"
                    });

            });
        }
    }
}
