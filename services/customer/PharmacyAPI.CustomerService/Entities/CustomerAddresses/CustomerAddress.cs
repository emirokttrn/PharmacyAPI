using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.CustomerService.Entities.CustomerAddresses
{
    public static class CustomerAddressConsts
    {
        public const int TitleMaxLength = 64;

        public const int AddressLineMinLength = 10;
        public const int FullAddressMaxLength = 256;
        public const int CityMaxLength = 64;
        public const int DistrictMaxLength = 64;
        public const int PostalCodeMaxLength = 16;
    }

    public class CustomerAddress : FullAuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; protected set; }
        public string Title { get; protected set; } = null!;
        public string FullAddress { get; protected set; } = null!;
        public string City { get; protected set; } = null!;
        public string District { get; protected set; } = null!;
        public string? PostalCode { get; protected set; }
        public double? Latitude { get; protected set; }
        public double? Longitude { get; protected set; }
        public bool IsDefault { get; protected set; }

        protected CustomerAddress() { }

        public CustomerAddress(
            Guid id,
            Guid customerId,
            string title,
            string fullAddress,
            string city,
            string district,
            string? postalCode,
            double? latitude = null,
            double? longitude = null) : base(id)
        {
            CustomerId = customerId;
            SetTitle(title);
            SetFullAddress(fullAddress);
            SetCity(city);
            SetDistrict(district);
            SetPostalCode(postalCode);
            SetLocation(latitude, longitude);
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(Title), CustomerAddressConsts.TitleMaxLength);
        }

        public void SetFullAddress(string fullAddress)
        {
            FullAddress = Check.NotNullOrWhiteSpace(
                fullAddress,
                nameof(FullAddress),
                CustomerAddressConsts.FullAddressMaxLength,
                CustomerAddressConsts.AddressLineMinLength);
        }

        public void SetCity(string city)
        {
            City = Check.NotNullOrWhiteSpace(city, nameof(City), CustomerAddressConsts.CityMaxLength);
        }

        public void SetDistrict(string district)
        {
            District = Check.NotNullOrWhiteSpace(district, nameof(District), CustomerAddressConsts.DistrictMaxLength);
        }

        public void SetPostalCode(string? postalCode)
        {
            PostalCode = Check.Length(postalCode, nameof(PostalCode), CustomerAddressConsts.PostalCodeMaxLength);
        }

        public void SetLocation(double? latitude, double? longitude)
        {
            if (latitude.HasValue != longitude.HasValue)
            {
                throw new UserFriendlyException("Enlem ve boylam birlikte verilmeli.");
            }

            if (latitude.HasValue && (latitude.Value < -90 || latitude.Value > 90))
            {
                throw new UserFriendlyException("Enlem -90 ile 90 arasinda olmali.");
            }

            if (longitude.HasValue && (longitude.Value < -180 || longitude.Value > 180))
            {
                throw new UserFriendlyException("Boylam -180 ile 180 arasinda olmali.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        internal void MarkAsDefault()
        {
            IsDefault = true;
        }

        internal void UnmarkAsDefault()
        {
            IsDefault = false;
        }
    }
}