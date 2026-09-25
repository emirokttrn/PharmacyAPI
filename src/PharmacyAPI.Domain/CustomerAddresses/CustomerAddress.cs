using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.CustomerAddresses
{
    // [Claude Agent] - Musteri adres defteri. Her musterinin en fazla 1 IsDefault=true adresi
    // olabilir kurali CustomerAddressManager'da uygulanir.
    public static class CustomerAddressConsts
    {
        public const int TitleMaxLength = 64;
        public const int FullAddressMaxLength = 512;
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
        public bool IsDefault { get; protected set; }

        protected CustomerAddress() { }

        internal CustomerAddress(Guid id, Guid customerId, string title, string fullAddress, string city, string district) : base(id)
        {
            CustomerId = customerId;
            SetTitle(title);
            SetFullAddress(fullAddress);
            SetCity(city);
            SetDistrict(district);
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(Title), CustomerAddressConsts.TitleMaxLength);
        }

        public void SetFullAddress(string fullAddress)
        {
            FullAddress = Check.NotNullOrWhiteSpace(fullAddress, nameof(FullAddress), CustomerAddressConsts.FullAddressMaxLength);
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
