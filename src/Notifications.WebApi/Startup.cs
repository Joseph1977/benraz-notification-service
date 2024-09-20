using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Notifications.Domain.Authorization;
using Notifications.Domain.Jobs;
using Notifications.Domain.Messages.MessagesFilter;
using Notifications.Domain.NotificationServices.Emails;
using Notifications.Domain.NotificationServices.Phones;
using Notifications.Domain.Settings;
using Notifications.EF;
using Notifications.EF.Repositories;
using Notifications.EF.Services;
using Notifications.WebApi.Authorization;
using Notifications.WebApi.Controllers;
using Notifications.WebApi.Extensions;
using Benraz.Infrastructure.Authorization.Tokens;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.BackgroundQueue;
using Benraz.Infrastructure.Common.DataRedundancy;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Web.Authorization;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using System.IdentityModel.Tokens.Jwt;

namespace Notifications.WebApi
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates startup instance.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures application services.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (IsEnablesApplicationInsights())
            {
                services.AddApplicationInsightsTelemetry();
            }

            services.AddMvc();
            services.AddCors();

            ConfigureSqlServerContext(services);

            services.AddAutoMapper(typeof(NotificationsAutoMapperProfile));

            services.AddHttpClient();

            services
                .AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(ITController)));

            AddVersioning(services);

            services.AddSwagger(Configuration);

            AddServices(services);
            AddAuthorization(services);
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Environment.</param>
        /// <param name="apiVersionDescriptionProvider">API version description provider.</param>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            UseDatabaseMigrations(app);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger(apiVersionDescriptionProvider, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSqlServerContext(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("Notifications");
            if (IsInjectDbCredentialsToConnectionString())
            {
                connectionString +=
                    $";User Id={Environment.GetEnvironmentVariable("ASPNETCORE_DB_USERNAME")};Password={Environment.GetEnvironmentVariable("ASPNETCORE_DB_PASSWORD")}";
            }

            services.AddDbContext<NotificationsDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    o => o.EnableRetryOnFailure(3)
                ));
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDrChecker, DrChecker>();
            services.AddScoped<ErrorFilterAttribute>();
            services.AddScoped<DRFilterAttribute>();

            services.AddTransient<IDbMigrationService, NotificationsDbMigrationService>();

            services.AddTransient<ISettingsEntriesRepository, SettingsEntriesRepository>();

            services.AddTransient<IEmailsService, EmailsService>();
            services.Configure<EmailsSettings>(Configuration.GetSection("Emails"));
            services.AddTransient<IMessageFilterProvider, MessageFilterProvider>();
            services.Configure<EmailsFilterSettings>(Configuration.GetSection("MessagesFilter:Emails"));

            services.AddTransient<IPhoneService, PhoneService>();
            services.Configure<PhoneSettings>(Configuration.GetSection("Phones"));
            services.AddTransient<IMessageFilterProvider, MessageFilterProvider>();
            services.Configure<EmailsFilterSettings>(Configuration.GetSection("MessagesFilter:Phones"));

            services.AddTransient<IEmptyRepeatableJobsService, EmptyRepeatableJobsService>();

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
        }

        /// <summary>
        /// Adds authorization.
        /// </summary>
        /// <param name="services">Services.</param>
        protected virtual void AddAuthorization(IServiceCollection services)
        {
            services.Configure<BenrazAuthorizationAuthGatewaySettings>(x =>
            {
                x.BaseUrl = Configuration.GetValue<string>("General:AuthorizationBaseUrl");
            });
            services.AddTransient<IBenrazAuthorizationAuthGateway, BenrazAuthorizationAuthGateway>();

            services.Configure<TokenValidationServiceSettings>(Configuration.GetSection("TokenValidation"));
            services.AddTransient<ITokenValidationService, TokenValidationService>();

            services.AddTransient<TokenValidator>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var tokenValidationService = serviceProvider.GetRequiredService<ITokenValidationService>();

                    // Switch to using TokenHandlers
                    options.TokenHandlers.Clear();  // Clear existing handlers if any
                    options.TokenHandlers.Add(new JwtSecurityTokenHandler());  // Add JwtSecurityTokenHandler for handling JWTs

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        IssuerSigningKeyResolver = tokenValidationService.IssuerSigningKeyResolver,
                        ValidateAudience = true,
                        AudienceValidator = tokenValidationService.AudienceValidator,
                        ValidateIssuer = true,
                        IssuerValidator = tokenValidationService.IssuerValidator
                    };
                    options.Audience = Configuration["Jwt:Audience"];
                    options.Authority = Configuration["Jwt:Authority"];
                });

            services
                .AddAuthorization(options =>
                {
                    options.AddClaimsPolicy(
                        NotificationsPolicies.EMAILS_SEND,
                        NotificationsClaims.EMAILS_SEND);

                    options.AddClaimsPolicy(
                        NotificationsPolicies.SETTINGS_READ,
                        NotificationsClaims.SETTINGS_READ);
                    options.AddClaimsPolicy(
                        NotificationsPolicies.SETTINGS_ADD,
                        NotificationsClaims.SETTINGS_ADD);
                    options.AddClaimsPolicy(
                        NotificationsPolicies.SETTINGS_UPDATE,
                        NotificationsClaims.SETTINGS_UPDATE);
                    options.AddClaimsPolicy(
                        NotificationsPolicies.SETTINGS_DELETE,
                        NotificationsClaims.SETTINGS_DELETE);

                    options.AddClaimsPolicy(
                       NotificationsPolicies.SMS_SEND,
                       NotificationsClaims.SMS_SEND);

                    options.AddPolicy(
                        NotificationsPolicies.JOB_EXECUTE,
                        builder => builder
                            .RequireRole(NotificationsRoles.INTERNAL_SERVER)
                            .RequireClaim(CommonClaimTypes.CLAIM, NotificationsClaims.JOB_EXECUTE));
                });
        }

        private bool IsEnablesApplicationInsights()
        {
            return Configuration.GetValue<bool>("EnablesApplicationInsights");
        }

        private static void AddVersioning(IServiceCollection services)
        {
            var versioningBuilder = services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Use appropriate version reader
            });

            // Add API Explorer for versioned APIs using the versioning builder
            versioningBuilder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        private void UseDatabaseMigrations(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IDbMigrationService>().MigrateAsync().Wait();
            }
        }

        private bool IsInjectDbCredentialsToConnectionString()
        {
            return Configuration.GetValue<bool>("InjectDBCredentialFromEnvironment");
        }
    }
}
