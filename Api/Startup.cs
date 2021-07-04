using Api.HostedServices;
using Data.Contexts;
using Domain.Handlers;
using Domain.Services.Telemetry;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Startup
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            _configurationManager = ConfigurationManager.CreateForWebAndService(webHostEnvironment.ContentRootPath, webHostEnvironment.EnvironmentName);
            _configuration = _configurationManager.Configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configurationManager);
            AddDb(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            AddServices(services);
            AddCQRS(services);
            AddHostedServices(services);
        }

        public void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<TelemetryPushService>();
        }

        private void AddCQRS(IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTelemetryBehavior<,>));
            services.RegisterRequestHandlers();
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<ITelemetryService, TelemetryService>();
        }

        private void AddDb(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_configurationManager.ConnectionString.Db,
                                                                                        b => b.MigrationsAssembly("Data")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton(x => _configurationManager.ConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
