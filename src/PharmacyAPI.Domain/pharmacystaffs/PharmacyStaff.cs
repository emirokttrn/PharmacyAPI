using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.pharmacystaffs
{
    public static class PharmacyStaffConsts
    {
        public const int TcKimlikNoLength = 11;
        // [Claude Agent] - TC Kimlik No artik sifreli (IStringEncryptionService.Encrypt) saklaniyor
        public const int TcKimlikNoEncryptedMaxLength = 512;
        public const int LicenseNumberMaxLength = 32;
        public const int PositionMaxLength = 64;
    }
    public class PharmacyStaff : FullAuditedAggregateRoot<Guid>
    {

        public Guid UserId { get; protected set; }
        public string TcKimlikNo { get; protected set; }
        public string LicenseNumber { get; protected set; }
        public string Position { get; protected set; }

        public virtual DateTime? HireDate { get; protected set; }


        protected PharmacyStaff() { }


        internal PharmacyStaff(Guid id, Guid userId, string tcKimlikNoEncrypted, string position) : base(id)
        {
            UserId = userId;
            SetTcKimlikNo(tcKimlikNoEncrypted);
            SetPosition(position);
        }

        public void SetPosition(string position)
        {
            Check.NotNullOrWhiteSpace(position, nameof(Position), PharmacyStaffConsts.PositionMaxLength);
            Position = position;
        }

        // [Claude Agent] - Artik PharmacyStaffManager tarafindan sifrelenmis deger geliyor, ham format
        // kontrolu (11 hane, hepsi rakam) IsValidTcKimlikNoFormat ile PharmacyStaffManager'da yapiliyor
        internal void SetTcKimlikNo(string tcKimlikNoEncrypted)
        {
            TcKimlikNo = Check.NotNullOrWhiteSpace(tcKimlikNoEncrypted, nameof(TcKimlikNo), PharmacyStaffConsts.TcKimlikNoEncryptedMaxLength);
        }

        public static bool IsValidTcKimlikNoFormat(string tcKimlikNo)
        {
            // [Claude Agent] - bug fix: eskisi "|| IsAllDigits(...)" idi, yani hepsi rakamsa hata firlatiyordu (tersti)
            return !string.IsNullOrEmpty(tcKimlikNo)
                && tcKimlikNo.Length == PharmacyStaffConsts.TcKimlikNoLength
                && IsAllDigits(tcKimlikNo);
        }



        public void SetLicenseNumber(string? licenseNumber)
        {
            LicenseNumber = Check.Length(
                licenseNumber, nameof(LicenseNumber), PharmacyStaffConsts.LicenseNumberMaxLength);
        }
        public void SetHireDate(DateTime? hireDate)
        {
            if (hireDate.HasValue && hireDate.Value > DateTime.UtcNow)
            {
                throw new UserFriendlyException("Ise baslama tarihi gelecekte olamaz.");
            }
            HireDate = hireDate;
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