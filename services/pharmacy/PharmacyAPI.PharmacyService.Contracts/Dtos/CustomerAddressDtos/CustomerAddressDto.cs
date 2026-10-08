using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.CustomerAddressDtos
{
    public class CreateCustomerAddressDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
        public string FullAddress { get; set; }

        [Required]
        [StringLength(64)]
        public string City { get; set; }

        [Required]
        [StringLength(64)]
        public string District { get; set; }

        [StringLength(16)]
        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; }
    }

    public class CustomerAddressDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public string Title { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
