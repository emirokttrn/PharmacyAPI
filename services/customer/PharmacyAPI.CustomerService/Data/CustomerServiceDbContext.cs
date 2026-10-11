using Microsoft.EntityFrameworkCore;
using PharmacyAPI.CustomerService.Entities.CustomersAddresses;
using PharmacyAPI.CustomerService.Entities.Customers;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using PharmacyAPI.ConsentRecords;

namespace PharmacyAPI.CustomerService.Data;

[ConnectionStringName("Default")]
public class CustomerServiceDbContext : AbpDbContext<CustomerServiceDbContext>
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;
    public DbSet<ConsentRecord> ConsentRecords { get; set; } = null!;

    public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Customer>(b =>
        {
            b.ToTable("Customers");
            b.ConfigureByConvention();

            b.Property(x => x.TcKimlikNo)
                .IsRequired()
                .HasMaxLength(CustomerConsts.TcKimlikNoEncryptedMaxLength);

            b.HasIndex(x => x.UserId).IsUnique().HasFilter("[IsDeleted] = 0");
            b.HasIndex(x => x.TcKimlikNo).IsUnique().HasFilter("[IsDeleted] = 0");
        });

        builder.Entity<CustomerAddress>(b =>
        {
            b.ToTable("CustomerAddresses");
            b.ConfigureByConvention();

            b.Property(x => x.Title).IsRequired().HasMaxLength(CustomerAddressConsts.TitleMaxLength);
            b.Property(x => x.FullAddress).IsRequired().HasMaxLength(CustomerAddressConsts.FullAddressMaxLength);
            b.Property(x => x.City).IsRequired().HasMaxLength(CustomerAddressConsts.CityMaxLength);
            b.Property(x => x.District).IsRequired().HasMaxLength(CustomerAddressConsts.DistrictMaxLength);
            b.Property(x => x.PostalCode).HasMaxLength(CustomerAddressConsts.PostalCodeMaxLength);

            b.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.CustomerId);
        });

        builder.Entity<ConsentRecord>(b =>
        {
            b.ToTable("ConsentRecords");
            b.ConfigureByConvention();

            b.Property(x => x.ConsentTextVersion)
                .IsRequired()
                .HasMaxLength(ConsentRecordConsts.ConsentTextVersionMaxLength);
            b.Property(x => x.IpAddress).HasMaxLength(ConsentRecordConsts.IpAddressMaxLength);

            b.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.CustomerId);
        });
    }
}