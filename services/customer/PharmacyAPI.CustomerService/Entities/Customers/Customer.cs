using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.CustomerService.Entities.Customers
{
    public static class CustomerConsts
    {
        public const int NameMaxLength = 128;
        public const int TcKimlikNoLength = 11;
        public const int TcKimlikNoEncryptedMaxLength = 512;
        public const int PhoneNumberMinLength = 10;
        public const int PhoneNumberMaxLength = 16;
        public const int AddressMinLength = 10;
        public const int AddressMaxLength = 256;
    }

    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; protected set; }
        public string CustomerName { get; protected set; } = null!;
        public string PhoneNumber { get; protected set; } = null!;
        public string TcKimlikNo { get; protected set; } = null!;
        public string? Address { get; protected set; }
        public DateTime? DateOfBirth { get; protected set; }

        protected Customer() { }

        public Customer(Guid id, Guid userId, string name, string phoneNumber, string tcKimlikNoEncrypted) : base(id)
        {
            UserId = userId;
            SetName(name);
            SetPhoneNumber(phoneNumber);
            SetTcKimlikNo(tcKimlikNoEncrypted);
        }

        public void SetName(string name)
        {
            CustomerName = Check.NotNullOrWhiteSpace(name, nameof(CustomerName), CustomerConsts.NameMaxLength);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(PhoneNumber), CustomerConsts.PhoneNumberMaxLength, CustomerConsts.PhoneNumberMinLength);

            var digits = phoneNumber.StartsWith('+') ? phoneNumber[1..] : phoneNumber;
            if (!IsAllDigits(digits))
            {
                throw new UserFriendlyException("Telefon numarasi sadece rakam icermeli.");
            }

            PhoneNumber = phoneNumber;
        }

        internal void SetTcKimlikNo(string tcKimlikNoEncrypted)
        {
            TcKimlikNo = Check.NotNullOrWhiteSpace(tcKimlikNoEncrypted, nameof(TcKimlikNo), CustomerConsts.TcKimlikNoEncryptedMaxLength);
        }

        public void SetAddress(string address)
        {
            Address = Check.NotNullOrWhiteSpace(address, nameof(Address), CustomerConsts.AddressMaxLength, CustomerConsts.AddressMinLength);
        }

        public void SetDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue && dateOfBirth.Value > DateTime.Now)
            {
                throw new UserFriendlyException("Dogum tarihi gelecekte olamaz.");
            }

            if (dateOfBirth.HasValue && CalculateAge(dateOfBirth.Value) < 18)
            {
                throw new UserFriendlyException("Musteri 18 yasindan kucuk olamaz.");
            }

            DateOfBirth = dateOfBirth;
        }

        public static bool IsValidTcKimlikNoFormat(string tcKimlikNo)
        {
            return !string.IsNullOrEmpty(tcKimlikNo)
                && tcKimlikNo.Length == CustomerConsts.TcKimlikNoLength
                && IsAllDigits(tcKimlikNo);
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        private static bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}