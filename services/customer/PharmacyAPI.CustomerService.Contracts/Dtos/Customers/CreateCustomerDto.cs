using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.CustomerService.Contracts.Dtos.Customers
{
    // KVKK: musteri olustururken en az KvkkAydinlatma ve AcikRiza rizasi zorunlu (CustomerManager kontrol eder)
    public class ConsentRecordInputDto
    {
        [Required]
        public ConsentType ConsentType { get; set; }

        [Required]
        public bool ConsentGiven { get; set; }

        [Required]
        public string ConsentTextVersion { get; set; } = null!;

        public string? IpAddress { get; set; }
    }

    public class CreateCustomerDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string TcKimlikNo { get; set; } = null!;

        [StringLength(256, MinimumLength = 10)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public List<ConsentRecordInputDto> Consents { get; set; } = new();
    }

    public class UpdateCustomerDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(16, MinimumLength = 10)]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(256, MinimumLength = 10)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class CustomerDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string TcKimlikNo { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}