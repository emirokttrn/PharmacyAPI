using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.PrescriptionDtos
{
    public class CreatePrescriptionDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [StringLength(512)]
        public string FileUrl { get; set; }
    }

    // [Claude Agent] - Eczaci personel bu DTO ile receteyi onaylar/reddeder
    public class ReviewPrescriptionDto
    {
        [Required]
        public Guid ReviewedByStaffId { get; set; }

        [StringLength(1000)]
        public string? ReviewNote { get; set; }
    }

    public class PrescriptionDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public string FileUrl { get; set; }
        public PrescriptionStatus Status { get; set; }
        public Guid? ReviewedByStaffId { get; set; }
        public string? ReviewNote { get; set; }
    }
}
