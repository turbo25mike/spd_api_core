using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business;
using Business.DataStore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

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
                Environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? Configuration["App:Environment"]
            });

            services.AddTransient<IDatabase, MySqlDatabase>();
            services.AddTransient<IWorkDatasource, WorkDatasource>();
            services.AddTransient<IWorkChatDatasource, WorkChatDatasource>();
            services.AddTransient<IWorkTagDatasource, WorkTagDatasource>();
            services.AddTransient<IMemberDatasource, MemberDatasource>();
            services.AddTransient<IOrgDatasource, OrgDatasource>();
            services.AddTransient<ITicketDatasource, TicketDatasource>();
            
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
#if DEBUG
                    ,RequireHttpsMetadata = false
#endif
                    ,Events = new JwtBearerEvents
                        {
                        OnTokenValidated = OnTokenValidated
                        }
                });


            app.UseMvc();
        }

        private async Task OnTokenValidated(TokenValidatedContext context)
        {
            if (context.SecurityToken is JwtSecurityToken token && context.Ticket.Principal.Identity is ClaimsIdentity identity)
            {
                identity.AddClaim(new Claim("access_token", token.RawData));
                var settings = new AppSettings {DB_Connection = Environment.GetEnvironmentVariable("APP_DB_CONNECTION") ?? Configuration["App:DB_Connection"]};
                var db = new MySqlDatabase(settings);
                var memberDatasource = new MemberDatasource(db);
                var userid = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var member = memberDatasource.Get(userid) ?? new Member();
                if (!member.UpdatedDate.HasValue || member.UpdatedDate.Value.AddDays(7) < DateTime.Now)
                {
                    var authInfo = await GetUserInfo(token.RawData);
                    member.Map(authInfo);

                    if (member.MemberID < 1)
                        member.MemberID = memberDatasource.Insert(member);
                    else
                        memberDatasource.Update(member);
                }
                
                identity.AddClaim(new Claim(nameof(member.MemberID), member.MemberID.ToString()));
            }
        }

        private async Task<Auth0User> GetUserInfo(string token)
        {
            var client = new HttpClient() { BaseAddress = new Uri(Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? Configuration["Auth0:Domain"]) };

            var request = new HttpRequestMessage(HttpMethod.Get, "tokeninfo")
            {
                Content = new StringContent("{\"id_token\":\"" + token + "\"}", Encoding.UTF8, "application/json")
            };

            var res = await client.SendAsync(request);
            return res.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Auth0User>(await res.Content.ReadAsStringAsync()) : null;
        }
    }
}
