using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.Repositories;
using SpyStore.Dal.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using SpyStore.Service.Filters;

namespace SpyStore.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        /// <summary>
        ///  Startup class
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // The DefaultContractResolver formatter reverts the casing of the results back to
            // Pascal casing. The second line adds indentations to the output.
            services.AddControllers(config =>  config.Filters.Add(new SpyStoreExceptionFilter(_env)))
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        options.SerializerSettings.Formatting = Formatting.Indented;
                    });

            // http://docs.asp.net/en/latest/security/cors.html
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });


            //NOTE: Did not disable mixed mode running here
            services.AddDbContextPool<StoreContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("SpyStore")));

            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SpyStore Service",
                    Version = "v1",
                    Description = "Service to support the SpyStore sample eCommerce site",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Freeware",
                        //Url = "https://en.wikipedia.org/wiki/Freeware"
                        Url = new Uri("http://localhost:23741/LICENSE.txt")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                /*
                  The using statement creates an instance of the IOC container; then, the next line
                  uses the container to get an instance of the StoreContext. The last line initializes the
                  database and the data.                  
                */
                using var serviceScope = app.ApplicationServices
                                            .GetRequiredService<IServiceScopeFactory>()
                                            .CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<StoreContext>();
                SampleDataInitializer.InitializeData(context);

            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpyStore Service v1");
            });
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll"); // has to go before UseMvc

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
