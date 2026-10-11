using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.CustomerService.Entities.CustomersAddresses
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
        public string PostalCode { get; protected set; } = null!;
        public bool IsDefault { get; protected set; } = false;

        protected CustomerAddress() { }
        public CustomerAddress(Guid id, Guid customerId, string title, string fullAddress, string city, string district, string postalCode) : base(id)
        {
            CustomerId = customerId;
            SetTitle(title);
            SetFullAddress(fullAddress);
            SetCity(city);
            SetDistrict(district);
            SetPostalCode(postalCode);

        }
        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), CustomerAddressConsts.TitleMaxLength);
        }
        public void SetFullAddress(string fullAddress)
        {
            FullAddress = Check.NotNullOrWhiteSpace(fullAddress, nameof(fullAddress), CustomerAddressConsts.AddressLineMinLength, CustomerAddressConsts.FullAddressMaxLength);
        }
        public void SetCity(string city)
        {
            City = Check.NotNullOrWhiteSpace(city, nameof(city), CustomerAddressConsts.CityMaxLength);
        }
        public void SetDistrict(string district)
        {
            District = Check.NotNullOrWhiteSpace(district, nameof(District), CustomerAddressConsts.DistrictMaxLength);
        }

        public void SetPostalCode(string? postalCode)
        {
            PostalCode = Check.Length(postalCode, nameof(PostalCode), CustomerAddressConsts.PostalCodeMaxLength);
        }
        internal void SetIsDefault(bool isDefault)
        {
            IsDefault = isDefault;
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