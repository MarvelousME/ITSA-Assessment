using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using VetClinic.Api.Authorization;
using VetClinic.Core.Constants;
using VetClinic.Core.Permissions;
using VetClinic.DAl.Managers;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Managers;
using VetClinic.DAL.Models;
using VetClinic.DAL.UnitOfWork;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.Api.Helpers
{
    public static class Utilities
    {
        public static void QuickLog(string text, string logPath)
        {
            string dirPath = Path.GetDirectoryName(logPath);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (StreamWriter writer = File.AppendText(logPath))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(JwtClaimTypes.Subject)?.Value?.Trim();
        }
        public static string[] GetRoles(ClaimsPrincipal identity)
        {
            return identity.Claims
                .Where(c => c.Type == JwtClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
        }
        public static void AddServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var authServerUrl = builder.Configuration["AuthServerUrl"].TrimEnd('/');

            string migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));

            // add identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity options and password complexity here
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //// Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
            });

            // Adds IdentityServer.
            builder.Services.AddIdentityServer(o =>
            {
                o.IssuerUri = authServerUrl;
            })
              // The AddDeveloperSigningCredential extension creates temporary key material for signing tokens.
              // This might be useful to get started, but needs to be replaced by some persistent key material for production scenarios.
              // See http://docs.identityserver.io/en/release/topics/crypto.html#refcrypto for more information.
              .AddDeveloperSigningCredential()
              .AddInMemoryPersistedGrants()
              // To configure IdentityServer to use EntityFramework (EF) as the storage mechanism for configuration data (rather than using the in-memory implementations),
              // see https://identityserver4.readthedocs.io/en/release/quickstarts/8_entity_framework.html
              .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
              .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
              .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
              .AddInMemoryClients(IdentityServerConfig.GetClients())
              .AddAspNetIdentity<ApplicationUser>()
              .AddProfileService<ProfileService>();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = authServerUrl;
                   options.RequireHttpsMetadata = false; // Note: Set to true in production
                   options.ApiName = IdentityServerConfig.ApiName;
               });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ViewAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewUsers));
                options.AddPolicy(Policies.ManageAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManageUsers));

                options.AddPolicy(Policies.ViewAllVisitsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewVisits));
                options.AddPolicy(Policies.ManageAllVisitsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManageVisits));

                options.AddPolicy(Policies.ViewAllPetOwnersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewPetOwners));
                options.AddPolicy(Policies.ManageAllPetOwnersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManagePetOwners));

                options.AddPolicy(Policies.ViewAllPetDetailsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewPetDetails));
                options.AddPolicy(Policies.ManageAllPetDetailsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManagePetDetails));
                
                options.AddPolicy(Policies.ViewAllVetsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewVets));
                options.AddPolicy(Policies.ManageAllVetsPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManageVets));


                options.AddPolicy(Policies.ViewAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ViewRoles));
                options.AddPolicy(Policies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
                options.AddPolicy(Policies.ManageAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.ManageRoles));

                options.AddPolicy(Policies.AssignAllowedRolesPolicy, policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
            });

            // Add cors
            builder.Services.AddCors();

            builder.Services.AddControllersWithViews();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = IdentityServerConfig.ApiFriendlyName, Version = "v1" });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>()
                            {
                                { IdentityServerConfig.ApiName, IdentityServerConfig.ApiFriendlyName }
                            }
                        }
                    }
                });
            });

            builder.Services.AddAutoMapper(typeof(Program));

            // Configurations
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // Business Services
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            // Repositories (Managers)
            builder.Services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddScoped<IPetOwnerManager, PetOwnerManager>();
            builder.Services.AddScoped<IPetDetailManager, PetDetailManager>();
            builder.Services.AddScoped<IVetManager, VetManager>();
            builder.Services.AddScoped<IVisitManager, VisitManager>();

            // Auth Handlers
            builder.Services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

            // DB Creation and Seeding
            builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            //File Logger
            builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

            //Email Templates
            EmailTemplates.Initialize(builder.Environment);
        }
        public static void ConfigureRequestPipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "VetClinic API";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{IdentityServerConfig.ApiFriendlyName} V1");
                c.OAuthClientId(IdentityServerConfig.SwaggerClientID);
                c.OAuthClientSecret("no_password"); //Leaving it blank doesn't work
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.Map("api/{**slug}", context =>
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return Task.CompletedTask;
                });

                endpoints.MapFallbackToFile("index.html");
            });
        }
        public static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                    databaseInitializer.SeedAsync().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogCritical(LoggingEvents.INITIALISE_DATABASE, ex, LoggingEvents.INITIALISE_DATABASE.Name);

                    throw new Exception(LoggingEvents.INITIALISE_DATABASE.Name, ex);
                }
            }
        }
    }
}
