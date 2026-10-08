using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Customers
{
    public static class CustomerConsts
    {
        public const int TcKimlikNoLength = 11;
        // [Claude Agent] - TC Kimlik No artik sifreli (IStringEncryptionService.Encrypt) saklaniyor,
        // ciphertext duz metinden cok daha uzun olabildigi icin kolon uzunlugu genisletildi
        public const int TcKimlikNoEncryptedMaxLength = 512;
        public const int AddressMaxLength = 256;
    }

    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; protected set; }
        public string TcKimlikNo { get; protected set; } = null!;
        public string? Address { get; protected set; }
        public DateTime? DateOfBirth { get; protected set; }

        protected Customer() { }

        internal Customer(Guid id, Guid userId, string tcKimlikNoEncrypted) : base(id)
        {
            UserId = userId;
            SetTcKimlikNo(tcKimlikNoEncrypted);
        }

        // [Claude Agent] - Artik CustomerManager tarafindan sifrelenmis (encrypted) deger geliyor,
        // ham TC format kontrolu (11 hane, hepsi rakam) CustomerManager'da IsValidTcKimlikNoFormat ile yapiliyor
        internal void SetTcKimlikNo(string tcKimlikNoEncrypted)
        {
            TcKimlikNo = Check.NotNullOrWhiteSpace(tcKimlikNoEncrypted, nameof(TcKimlikNo), CustomerConsts.TcKimlikNoEncryptedMaxLength);
        }

        public static bool IsValidTcKimlikNoFormat(string tcKimlikNo)
        {
            return !string.IsNullOrEmpty(tcKimlikNo)
                && tcKimlikNo.Length == CustomerConsts.TcKimlikNoLength
                && IsAllDigits(tcKimlikNo);
        }

        public void SetAddress(string address)
        {
            Address = Check.NotNullOrWhiteSpace(address, nameof(Address), CustomerConsts.AddressMaxLength);
        }

        public void SetDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue && dateOfBirth.Value > DateTime.UtcNow)
            {
                throw new UserFriendlyException("Dogum tarihi gelecekte olamaz.");
            }

            if (dateOfBirth.HasValue)
            {
                if (CalculateAge(dateOfBirth.Value) < 18)
                {
                    throw new UserFriendlyException("Musteri 18 yasindan kucuk olamaz.");
                }
            }

            DateOfBirth = dateOfBirth;
        }

        // [Claude Agent] - OrderManager'daki 18+ urun yas kontrolunde de kullanilan ortak yas hesaplama helper'i
        public static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow.Date;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        private static bool IsAllDigits(string value)
        {
            foreach (var c in value)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
    }
}