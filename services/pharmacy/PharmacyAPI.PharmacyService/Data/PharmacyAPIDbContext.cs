using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Brands;
using PharmacyAPI.Categories;
using PharmacyAPI.ConsentRecords;
using PharmacyAPI.CustomerAddresses;
using PharmacyAPI.Customers;
using PharmacyAPI.HealtTopics;
using PharmacyAPI.Orders;
using PharmacyAPI.pharmacystaffs;
using PharmacyAPI.Prescriptions;
using PharmacyAPI.Products;
using PharmacyAPI.Reviews;
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
    // [Claude Agent] - bug fix: Customer ve PharmacyStaff hic DbContext'e kayitli degildi,
    // bu yuzden onlarin migration'i bos uretiliyordu (tablolari hic olusmamisti)
    public DbSet<Customer> Customers { get; set; }
    public DbSet<pharmacystaffs.PharmacyStaff> PharmacyStaffs { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<ConsentRecord> ConsentRecords { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Review> Reviews { get; set; }
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

            b.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId).IsRequired();
            b.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).IsRequired();

            b.HasMany(x => x.healthTopics).WithOne().HasForeignKey(x => x.ProductId);
        });

        // [Claude Agent] - bug fix: Customer/PharmacyStaff daha once hic tabloya baglanmamisti
        builder.Entity<Customer>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Customers", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.TcKimlikNo).IsRequired().HasMaxLength(CustomerConsts.TcKimlikNoEncryptedMaxLength);
            b.Property(x => x.Address).HasMaxLength(CustomerConsts.AddressMaxLength);

            b.HasIndex(x => x.UserId).IsUnique();
            b.HasIndex(x => x.TcKimlikNo).IsUnique();
        });

        builder.Entity<pharmacystaffs.PharmacyStaff>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "PharmacyStaffs", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.TcKimlikNo).IsRequired().HasMaxLength(pharmacystaffs.PharmacyStaffConsts.TcKimlikNoEncryptedMaxLength);
            b.Property(x => x.Position).IsRequired().HasMaxLength(pharmacystaffs.PharmacyStaffConsts.PositionMaxLength);
            // [Claude Agent] - bug fix: LicenseNumber entity'de "string" (nullable isaretsiz) ama pratikte
            // opsiyonel (SetLicenseNumber(string?) ve CreatePharmacyStaffDto'da [Required] yok), EF bunu
            // NOT NULL kolon olarak uretirdi ve lisans numarasiz personel eklerken patlardi.
            b.Property(x => x.LicenseNumber).IsRequired(false).HasMaxLength(pharmacystaffs.PharmacyStaffConsts.LicenseNumberMaxLength);

            b.HasIndex(x => x.UserId).IsUnique();
            b.HasIndex(x => x.TcKimlikNo).IsUnique();
        });

        builder.Entity<CustomerAddress>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "CustomerAddresses", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Title).IsRequired().HasMaxLength(CustomerAddressConsts.TitleMaxLength);
            b.Property(x => x.FullAddress).IsRequired().HasMaxLength(CustomerAddressConsts.FullAddressMaxLength);
            b.Property(x => x.City).IsRequired().HasMaxLength(CustomerAddressConsts.CityMaxLength);
            b.Property(x => x.District).IsRequired().HasMaxLength(CustomerAddressConsts.DistrictMaxLength);
            b.Property(x => x.PostalCode).HasMaxLength(CustomerAddressConsts.PostalCodeMaxLength);

            b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => x.CustomerId);
        });

        builder.Entity<Prescription>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Prescriptions", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FileUrl).IsRequired().HasMaxLength(PrescriptionConsts.FileUrlMaxLength);
            b.Property(x => x.ReviewNote).HasMaxLength(PrescriptionConsts.ReviewNoteMaxLength);

            b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasOne<pharmacystaffs.PharmacyStaff>().WithMany().HasForeignKey(x => x.ReviewedByStaffId).OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => x.CustomerId);
        });

        builder.Entity<ConsentRecord>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "ConsentRecords", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.ConsentTextVersion).IsRequired().HasMaxLength(ConsentRecordConsts.ConsentTextVersionMaxLength);
            b.Property(x => x.IpAddress).HasMaxLength(ConsentRecordConsts.IpAddressMaxLength);

            b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => x.CustomerId);
        });

        builder.Entity<Review>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Reviews", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Comment).HasMaxLength(ReviewConsts.CommentMaxLength);

            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => new { x.ProductId, x.CustomerId }).IsUnique();
        });

        builder.Entity<Order>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "Orders", PharmacyAPIConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.TrackingNumber).HasMaxLength(OrderConsts.TrackingNumberMaxLength);

            b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasOne<CustomerAddress>().WithMany().HasForeignKey(x => x.ShippingAddressId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => x.CustomerId);

            b.HasMany(x => x.OrderLines).WithOne().HasForeignKey(x => x.OrderId).IsRequired();
        });

        builder.Entity<OrderLine>(b =>
        {
            b.ToTable(PharmacyAPIConsts.DbTablePrefix + "OrderLines", PharmacyAPIConsts.DbSchema);

            b.Property(x => x.ProductName).IsRequired().HasMaxLength(OrderLine.ProductNameMaxLength);
            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");

            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired().OnDelete(DeleteBehavior.Restrict);
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
