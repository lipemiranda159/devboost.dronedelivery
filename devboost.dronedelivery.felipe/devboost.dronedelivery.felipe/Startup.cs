using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using devboost.dronedelivery.felipe.Services;
using devboost.dronedelivery.felipe.Services.Interfaces;
using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Facade;
using devboost.dronedelivery.felipe.DTO.Repositories.Interfaces;
using devboost.dronedelivery.felipe.DTO.Repositories;

namespace devboost.dronedelivery.felipe
{
    public class Startup
    {
        private const string SWAGGERFILE_PATH = "./swagger/v1/swagger.json";
        private const string API_VERSION = "v1";
        private const string LOCALHOST = "http://localhost:80";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IDroneRepository, DroneRepository>();
            services.AddSingleton<IPedidoDroneRepository, PedidoDroneRepository>();
            services.AddSingleton<IPedidoService, PedidoService>();
            services.AddSingleton<IDroneService, DroneService>();
            services.AddSingleton<ICoordinateService, CoordinateService>();
            services.AddSingleton<IPedidoFacade, PedidoFacade>();
            services.AddSingleton<IDroneFacade, DroneFacade>();

            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG)), ServiceLifetime.Singleton);
            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = ProjectConsts.PROJECT_NAME, Version = API_VERSION });
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ProjectConsts.XML_EXTENSION;
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
            }
            // Swagger
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.RoutePrefix = string.Empty;
                   c.SwaggerEndpoint(SWAGGERFILE_PATH, ProjectConsts.PROJECT_NAME + API_VERSION);
               });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
