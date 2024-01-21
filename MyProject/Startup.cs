using Autofac;
using Business;
using Core;
using Core.Database.Context;
using Core.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Filters;
using FluentValidation.AspNetCore;

namespace MyProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
                    ValidAudience = "my-project",
                    ValidIssuer = "my-project",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("asqwezxnbansdqweqwhZadaqwWeqeqSajhdhahqwerEYWEWURbBBASDH12DASthruterasdq123afkjzxczczas"))
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
                opt.Filters.Add(new ExceptionFilter());
                opt.Filters.Add(new ValidationFilter());
            })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .AddFluentValidation(opt =>
                {
                    opt.RunDefaultMvcValidationAfterFluentValidationExecutes  = false;
                });
            services.AddRouting(cfg => { cfg.LowercaseUrls = true; });
            services.AddDbContext<MySqlDatabase>(cfg =>
            {
                cfg.EnableDetailedErrors(true);
                cfg.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                cfg.UseMySql(Configuration["database:connectionString"],
                    new MySqlServerVersion(new Version(8, 0, 11)),
                    dbOptions =>
                    {
                        dbOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(nameof(UserRole.Admin), policy => policy.RequireRole(nameof(UserRole.Admin)));
                opt.AddPolicy(nameof(UserRole.User), policy => policy.RequireRole(nameof(UserRole.User)));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsOrigin",
                    builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("CorsOrigin");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new BusinessModule(Configuration.GetSection("SmtpSettings")));
        }
    }
}
