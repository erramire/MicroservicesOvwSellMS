using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoS.Sell.API.Application.StartupExtensions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using PoS.Sell.API.EventHandlers;

namespace PoS.Sell.API
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
            services.AddControllers();

            // Register cloud resources via extension methods
            //services.RegisterMessageBroker(Configuration);
            services.RegisterNoSqlStore(Configuration);

            // Resgister concrete dependencies
            services.AddScoped<Application.Domain.ISellBusiness, Application.Domain.SellBusiness>();
            

            services.AddMvc(config => { config.Filters.Add(typeof(Application.Filters.SellCustomExceptionFilter)); }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Sell Services API",
                    Version = "v1",
                    Description =
                        "Sell API service for PoS."                    
                });

                //Set the comments path for the Swagger JSON and UI.

               //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
               // var xmlPath = Path.Combine(basePath, "PoS.Sell.API.xml");
               // c.IncludeXmlComments(xmlPath);
            });

            services.AddApiVersioning(x =>
            {
                // Allows for API to return a version in the response header
                x.ReportApiVersions = true;
                // Default version for clients not specifying a version number
                x.AssumeDefaultVersionWhenUnspecified = true;
                // Specifies version to which to default. This is the version
                // to which you are routed if no version is specified
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddApplicationInsightsTelemetry("1e815d30-352a-429f-9453-df45109beeb4");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, System.IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Selling" +
                " Services API V1"); });

            //ConfigureEventBus(app, serviceProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app, System.IServiceProvider serviceProvider)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<CC.EventBus.EventBus.IEventBus>();
            eventBus.ServiceProvider = serviceProvider;
            eventBus.Subscribe(CC.EventBus.Events.MessageEventEnum.ProductChangedEvent, typeof(ProductChangedEventHandler));
            eventBus.Subscribe(CC.EventBus.Events.MessageEventEnum.SellStatusChangedEvent,
                typeof(PoS.Sell.API.EventHandlers.SellPaidEventHandler));
        }
    }
}
