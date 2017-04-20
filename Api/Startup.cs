using System;
using Api.DataStore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            // Add framework services.
            services.AddMvc();

            ConfigureAppSettings(services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddSingleton<IAppSettings>(new AppSettings
            {
                DB_Connection = Environment.GetEnvironmentVariable("APP_DB_CONNECTION") ?? Configuration["App:DB_Connection"],
                Auth0_Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? Configuration["Auth0:Domain"],
                Environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? Configuration["App:Environment"]
            });

            services.AddTransient<IDatabase, MySqlDatabase>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("CorsPolicy");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var clientIDs = Environment.GetEnvironmentVariable("AUTH0_CLIENT_IDS") ?? Configuration["Auth0:ApiIdentifier"];
            var domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? Configuration["Auth0:Domain"];

            foreach (var id in clientIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                app.UseJwtBearerAuthentication(new JwtBearerOptions
                {
                    Audience = id,
                    Authority = domain
                });
            app.UseMvc();
        }
    }
}
