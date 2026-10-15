using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PharmacyAPI.StoreService.Data;
using PharmacyAPI.StoreService.Localization;
using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace PharmacyAPI.StoreService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
)]
public class StoreServiceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        // Pharmacy'deki gibi: sadece acikca [Required] olan DTO alanlari zorunlu sayilir.
        context.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });

        context.Services.AddMapperlyObjectMapper();

        ConfigureDatabase(context);
        ConfigureLocalization();
        ConfigureAuthentication(context, configuration, hostingEnvironment);
        ConfigureSwagger(context);

        // Conventional (otomatik) controller uretimi bilerek kapali: ConventionalControllers.Create
        // cagrilmiyor, bu yuzden sadece Controllers/ altindaki elle yazilmis controller'lar gorunur.
    }

    private static void ConfigureDatabase(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<StoreServiceDbContext>(options =>
        {
            options.Entity<Store>(o => o.DefaultWithDetailsFunc = q => q.Include(s => s.StoreWorkingHours));
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        context.Services.Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StoreServiceResource>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<StoreServiceResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/StoreService");

            options.DefaultResourceType = typeof(StoreServiceResource);
        });
    }

    // Bu servis token uretmez; Pharmacy (AuthServer) tarafindan uretilen JWT'yi dogrular.
    private static void ConfigureAuthentication(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddAbpJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.Audience = "StoreService";
                options.RequireHttpsMetadata = !hostingEnvironment.IsDevelopment();
            });
    }

    // OAuth akisi yok: Swagger'da "Authorize" ile Bearer token yapistirilir.
    private static void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddAbpSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Store Service API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
            // ABP'nin yerlesik /api/abp/* uc noktalari Swagger'da gosterilmez.
            options.HideAbpEndpoints();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Pharmacy servisinden alinan access token'i yapistirin (\"Bearer \" yazmadan)."
            });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.UseCorrelationId();
        app.UseRouting();
        app.UseAuthentication();
        app.UseUnitOfWork();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Store Service API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
