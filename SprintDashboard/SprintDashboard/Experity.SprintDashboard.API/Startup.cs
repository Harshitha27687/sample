using System;
using System.IO;
using System.Security.Cryptography;
using Experity.AppAuth.API.Extensions;
using Experity.SprintDashboard.API.Authentication;
using Experity.SprintDashboard.API.Authentication.Encryption;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Experity.SprintDashboard.API.Caches;
using Experity.SprintDashboard.API.Configuration;
using Experity.SprintDashboard.API.Configuration.Extensions;
using Experity.SprintDashboard.API.Configuration.Models;
using Experity.SprintDashboard.API.Extensions;
using Experity.SprintDashboard.API.Middlewares;
using Experity.SprintDashboard.API.Streams;
using Experity.SprintDashboard.DataAccess.GenPro;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Experity.SprintDashboard.DataAccess.Dapper;

namespace Experity.SprintDashboard.API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private ILogger<Startup> _logger;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment environment)
        {
            _config = configuration;
            _logger = logger;
            _environment = environment;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppConfigLoader(_config).LoadSettings();
            services.AddSingleton(appSettings);

            if (!appSettings.EnableLoggingEndpoint)
            {
                LogManager.Configuration.Variables["endpointLoggingLevel"] = "OFF";
                LogManager.ReconfigExistingLoggers();
            }

            _logger.LogInformation(appSettings.ToString());

            services.AddLogging(builder => { builder.AddConsole(); });

            services.AddProblemDetails(_environment.IsDevelopment());
            _logger.LogInformation($"Environment is Dev: {_environment.IsDevelopment()}");


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            });

            var encryptionSettings = new EncryptionConfigLoader(_config).LoadEncryptionConfig();
            services.AddSingleton(encryptionSettings);
            _logger.LogInformation(encryptionSettings.ToString());

            var appAuthSettings = new AuthApiConfigLoader(_config).LoadAuthConfig();
            services.AddSingleton(appAuthSettings);

            _logger.LogInformation(appAuthSettings.ToString());

            services.AddSingleton<IAuthHttpClient, AuthHttpClient>();

            services.AddScoped<IPracticeProvider>(sp => new PracticeProvider(appSettings.ConnectionString));
            services.AddScoped<IPracticeEnvironmentCache, PracticeEnvironmentCache>();

            //runtime resolution via factory:  https://stackoverflow.com/questions/37744637/how-can-i-pass-a-runtime-parameter-as-part-of-the-dependency-resolution
            var fileStreamLogger = services.BuildServiceProvider().GetService<ILogger<FileStreamProvider>>();
            services.AddTransient<IStreamProvider, FileStreamProvider>((sp) => new FileStreamProvider(fileStreamLogger, "", "version.txt"));

            services.AddTransient<IApplicationAuthService, ApplicationAuthService>();
            services.AddTransient<ValidateTokenFilter>();

            services.AddTransient((serviceProvider) =>
            {
                return new Func<string, IClinicProvider>((env) => new DataAccess.GenPro.ClinicProvider(env, appSettings.ConnectionString));
            });

            services.AddTransient((serviceProvider) =>
            {
                return new Func<string, ITeamProvider>((env) => new TeamProvider(appSettings.ConnectionString));
            });

            ConfigureSwagger(services, appSettings.ApplicationVersion);
            ConfigureAuth(services, appSettings.ClaimsSettingsModel);
        }
        private void ConfigureSwagger(IServiceCollection services, string appVersion)
        {
            // Register the Swagger services
            var swgTitle = "Example Microservice Template Swagger Title API";
            var swgDescription = "Template for creating microservices";
            var swgTermsOfService = "Experity Internal Use Only";

            var authDescription = "Type into the textbox: Bearer {your JWT token}. You can get a JWT token from PVM upon login if you have the correct permissions.";
            services.AddSwaggerDocumentation(appVersion, swgTitle, swgDescription, swgTermsOfService, authDescription);
        }

        private void ConfigureAuth(IServiceCollection services, JwtSettingsModel claimsModel)
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            RSA publicRsa = RSA.Create();
            var publicKey = Path.Combine(claimsModel.PublicKeyFilePath, claimsModel.PublicKeyFileName);
            publicRsa.LoadPublicKey(publicKey);
            var signingKey = new RsaSecurityKey(publicRsa);


            services
                .AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey
                    };
                });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This is called after ConfigureServices.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            bool reverseProxy = false;

            var basePath = _config.GetSection("PathBase").Value;
            if (basePath != null)
            {
                //_logger.LogInformation("Startup:Configure PathBase= " + basePath);
                app.UsePathBase(new PathString(basePath));
                reverseProxy = true;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var appSettings = serviceProvider.GetService<AppSettings>();
            if (appSettings.SwaggerEnable)
            {
                app.UseSwaggerDocumentation(reverseProxy, basePath);
            }

            app.UseStaticFiles();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            // proxy handles tls app.UseHttpsRedirection();
            app.UseProblemDetails(); // UseProblemDetails needs to be called before app.UsMvc()
            app.UseMvc();
        }
    }
}
