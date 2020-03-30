using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpyStore.Mvc.Support;
using NLog;
using System.IO;
using SpyStore.Mvc.Extensions;
using SpyStore.Mvc.GlobalErrorHandling.Extensions;
using SpyStore.Mvc.LoggerService.Contracts;

namespace SpyStore.Mvc
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;            
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<ServiceSettings>(Configuration.GetSection("ServiceSettings"));
            services.ConfigureLoggerService();
            services.ConfigureHttpClientService();

            ConfigureWebOptimizer(services);

        }     

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (_env.IsDevelopment() || _env.EnvironmentName == "Local")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.ConfigureExceptionHandler(logger);
            //app.ConfigureCustomExceptionMiddleware();

            // minimize and bundle.
            app.UseWebOptimizer();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureWebOptimizer(IServiceCollection services)
        {
            if (_env.IsDevelopment() || _env.EnvironmentName == "Local")
            {
                // code disables all bundling and minification. 
                services.AddWebOptimizer(false,false);

                //services.AddWebOptimizer(options =>
                //{
                //    options.MinifyCssFiles(); //Minifies all CSS files
                //    //options.MinifyJsFiles(); //Minifies all JS files
                //    options.MinifyJsFiles("js/site.js");
                //    options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/**/*.js");
                //    //options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/validators.js", "js/validations/errorFormatting.js");
                //});
            }
            else
            {
                services.AddWebOptimizer(options =>
                {
                    options.MinifyCssFiles(); //Minifies all CSS files
                    //options.MinifyJsFiles(); //Minifies all JS files
                    options.MinifyJsFiles("js/site.js");
                    options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/**/*.js");
                    //options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/validators.js", "js/validations/errorFormatting.js");
                });
            }
        }

    }
}
