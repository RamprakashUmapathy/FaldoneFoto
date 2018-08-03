using DevExpress.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Kasanova.FaldoneFoto.Infrastructure.Enums;
using Kasanova.Common.ApplicationCore.Interfaces;
using Infrastructure.Logging;
using Web.Services;

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
            services.AddScoped<ICategoryRepository, CategoryRepositoryCache>();
            services.AddScoped<IArticleRepository, ArticleRepositoryCache>();

            services.AddMvc();
            services.AddDevExpressControls();
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

                routes.MapRoute(name: "CategoryApi",
                                template: "api/categories",
                                defaults: new
                                {
                                    controller = "Categories",
                                    action = "GetCategoriesAsync"
                                });

                routes.MapRoute(name: "CategoryByIdApi",
                                template: "api/categories/{categoryId}",
                                defaults: new
                                {
                                    controller = "Categories",
                                    action = "GetCategoriesByIdAsync"
                                });

                routes.MapRoute(name: "FamilyApi",
                    template: "api/categories/{categoryid}/families",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetFamiliesAsync"
                    });

                routes.MapRoute(name: "FamilyByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetFamiliesByIdAsync"
                    });

                routes.MapRoute(name: "SeriesApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetSeriesAsync"
                    });

                routes.MapRoute(name: "SeriesByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetSeriesByIdAsync"
                    });

                routes.MapRoute(name: "Level1Api",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetLevel1Async"
                    });

                routes.MapRoute(name: "Level1ByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetLevel1ByIdAsync"
                    });

                routes.MapRoute(name: "Level2Api",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetLevel2Async"
                    });

                routes.MapRoute(name: "Level2ByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetLevel2ByIdAsync"
                    });

                routes.MapRoute(name: "StyleApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetStylesAsync"
                    });

                routes.MapRoute(name: "StylleByIdApi",
                    template: "api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles/{styleid}",
                    defaults: new
                    {
                        controller = "Categories",
                        action = "GetStylesByIdAsync"
                    });


            });
        }
    }
}
