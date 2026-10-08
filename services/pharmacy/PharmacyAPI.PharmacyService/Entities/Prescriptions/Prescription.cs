using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Prescriptions
{
    // [Claude Agent] - Recete yukleme/onay entity'si. Gercek dosya yukleme altyapisi olmadigi
    // icin FileUrl sadece dosyanin durdugu yerin string'i.
    public static class PrescriptionConsts
    {
        public const int FileUrlMaxLength = 512;
        public const int ReviewNoteMaxLength = 1000;
    }

    public class Prescription : FullAuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; protected set; }
        public string FileUrl { get; protected set; } = null!;
        public PrescriptionStatus Status { get; protected set; }
        public Guid? ReviewedByStaffId { get; protected set; }
        public string? ReviewNote { get; protected set; }

        protected Prescription() { }

        internal Prescription(Guid id, Guid customerId, string fileUrl) : base(id)
        {
            CustomerId = customerId;
            SetFileUrl(fileUrl);
            Status = PrescriptionStatus.Pending;
        }

        internal void SetFileUrl(string fileUrl)
        {
            FileUrl = Check.NotNullOrWhiteSpace(fileUrl, nameof(FileUrl), PrescriptionConsts.FileUrlMaxLength);
        }

        // [Claude Agent] - Sadece Pending durumundaki bir recete onaylanabilir/reddedilebilir
        public void Approve(Guid reviewedByStaffId, string? reviewNote)
        {
            if (Status != PrescriptionStatus.Pending)
            {
                throw new UserFriendlyException("sadece bekleyen (Pending) receteler onaylanabilir!");
            }
            Status = PrescriptionStatus.Approved;
            ReviewedByStaffId = reviewedByStaffId;
            ReviewNote = Check.Length(reviewNote, nameof(ReviewNote), PrescriptionConsts.ReviewNoteMaxLength);
        }

        public void Reject(Guid reviewedByStaffId, string? reviewNote)
        {
            if (Status != PrescriptionStatus.Pending)
            {
                throw new UserFriendlyException("sadece bekleyen (Pending) receteler reddedilebilir!");
            }
            Status = PrescriptionStatus.Rejected;
            ReviewedByStaffId = reviewedByStaffId;
            ReviewNote = Check.Length(reviewNote, nameof(ReviewNote), PrescriptionConsts.ReviewNoteMaxLength);
        }
    }
}
