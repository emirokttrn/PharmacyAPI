using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Brands;
using PharmacyAPI.Categories;
using PharmacyAPI.HealtTopics;
using PharmacyAPI.Products;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace PharmacyAPI.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class PharmacyAPIDbContext :
    AbpDbContext<PharmacyAPIDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<Product> Products { get; set; }
public DbSet<Category> Categories { get; set; }
public DbSet<Brand> Brands { get; set; }
public DbSet<ProductHealthTopic> ProductHealthTopics { get; set; }
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public PharmacyAPIDbContext(DbContextOptions<PharmacyAPIDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(PharmacyAPIConsts.DbTablePrefix + "YourEntities", PharmacyAPIConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

builder.Entity<Product>(b =>
{
    b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Products", PharmacyAPIConsts.DbSchema);
    b.ConfigureByConvention();

    b.Property(x => x.ProductName).IsRequired().HasMaxLength(Product.ProductNameMaxLength);
    b.Property(x => x.Price).HasColumnType("decimal(18,2)");
    b.Property(x => x.DiscountedPrice).HasColumnType("decimal(18,2)");

    b.HasOne<Brand>().WithMany().HasForeignKey(x => x.BrandId).IsRequired();
    b.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).IsRequired();

    b.HasMany(x => x.healthTopics).WithOne().HasForeignKey(x => x.ProductId);
});

builder.Entity<Category>(b =>
{
    b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Categories", PharmacyAPIConsts.DbSchema);
    b.ConfigureByConvention();

    b.HasOne(x => x.Parent)
     .WithMany(x => x.Childeren)
     .HasForeignKey(x => x.ParentId)
     .OnDelete(DeleteBehavior.Restrict);
});

builder.Entity<Brand>(b =>
{
    b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Brands", PharmacyAPIConsts.DbSchema);
    b.ConfigureByConvention();

});

builder.Entity<ProductHealthTopic>(b =>
{
    b.ToTable(PharmacyAPIConsts.DbTablePrefix + "ProductHealthTopics", PharmacyAPIConsts.DbSchema);
    b.HasKey(x => new { x.ProductId, x.Topic });
});

    }
}
