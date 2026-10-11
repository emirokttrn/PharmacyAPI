using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.ConsentRecords
{
    // [Claude Agent] - KVKK / acik riza onay kaydi. CreationAudited yeterli, sonradan
    // degistirilmez (bir onay ne zaman/hangi metne verildi bilgisi sabit kalmali).
    public static class ConsentRecordConsts
    {
        public const int ConsentTextVersionMaxLength = 32;
        public const int IpAddressMaxLength = 64;
    }

    public class ConsentRecord : CreationAuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; protected set; }
        public ConsentType ConsentType { get; protected set; }
        public bool ConsentGiven { get; protected set; }
        public string ConsentTextVersion { get; protected set; } = null!;
        public string? IpAddress { get; protected set; }

        protected ConsentRecord() { }

        internal ConsentRecord(
            Guid id,
            Guid customerId,
            ConsentType consentType,
            bool consentGiven,
            string consentTextVersion,
            string? ipAddress) : base(id)
        {
            CustomerId = customerId;
            ConsentType = consentType;
            ConsentGiven = consentGiven;
            ConsentTextVersion = Check.NotNullOrWhiteSpace(consentTextVersion, nameof(ConsentTextVersion), ConsentRecordConsts.ConsentTextVersionMaxLength);
            IpAddress = Check.Length(ipAddress, nameof(IpAddress), ConsentRecordConsts.IpAddressMaxLength);
        }
    }
}
