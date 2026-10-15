using Microsoft.EntityFrameworkCore;
using PharmacyAPI.StoreService.Entities.StoreProducts;
using PharmacyAPI.StoreService.Entities.StoreReviews;
using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace PharmacyAPI.StoreService.Data;

[ConnectionStringName("Default")]
public class StoreServiceDbContext : AbpDbContext<StoreServiceDbContext>
{
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<StoreWorkingHour> StoreWorkingHours { get; set; } = null!;
    public DbSet<StoreProduct> StoreProducts { get; set; } = null!;
    public DbSet<StoreReview> StoreReviews { get; set; } = null!;

    public StoreServiceDbContext(DbContextOptions<StoreServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Store>(b =>
        {
            b.ToTable("Stores");
            b.ConfigureByConvention();

            b.Property(x => x.StoreName).IsRequired().HasMaxLength(StoreConsts.MaxStoreNameLength);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(StoreConsts.MaxPhoneNumberLength);
            b.Property(x => x.FullAddress).IsRequired().HasMaxLength(StoreConsts.MaxFullAddressLength);
            b.Property(x => x.City).IsRequired().HasMaxLength(StoreConsts.MaxCityLength);
            b.Property(x => x.District).IsRequired().HasMaxLength(StoreConsts.MaxDistrictLength);

            b.HasMany(x => x.StoreWorkingHours)
                .WithOne()
                .HasForeignKey(x => x.StoreId)
                .IsRequired();

            b.HasIndex(x => new { x.Latitude, x.Longitude });
            b.HasIndex(x => new { x.City, x.District });
        });

        builder.Entity<StoreWorkingHour>(b =>
        {
            b.ToTable("StoreWorkingHours");
            b.ConfigureByConvention();

            b.HasKey(x => new { x.StoreId, x.DayOfWeek });
        });

        builder.Entity<StoreProduct>(b =>
        {
            b.ToTable("StoreProducts");
            b.ConfigureByConvention();

            b.Property(x => x.Price).HasPrecision(18, 2);

            b.HasOne<Store>()
                .WithMany()
                .HasForeignKey(x => x.StoreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.StoreId, x.ProductId }).IsUnique().HasFilter("[IsDeleted] = 0");
            b.HasIndex(x => x.ProductId);
        });

        builder.Entity<StoreReview>(b =>
        {
            b.ToTable("StoreReviews");
            b.ConfigureByConvention();

            b.Property(x => x.Comment).HasMaxLength(StoreReviewConsts.MaxCommentLength);

            b.HasOne<Store>()
                .WithMany()
                .HasForeignKey(x => x.StoreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.StoreId, x.UserId }).IsUnique().HasFilter("[IsDeleted] = 0");
        });
    }
}
