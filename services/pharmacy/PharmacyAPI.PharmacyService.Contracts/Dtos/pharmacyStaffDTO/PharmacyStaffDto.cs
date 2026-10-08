using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.pharmacyStaffDTO
{
    public class CreatePharmacyStaffDto
    {      public Guid UserId { get; set; }
        public string TcKimlikNo { get; set; }
        public string Position { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? HireDate { get; set; }
    }
    public class PharmacyStaffDto :AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName{get; set;}
        public string Name{get;set;}
        public string Surname{get;set;}
        public string TcKimlikNo { get; set; }
        public string Position { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime? HireDate { get; set; }
    }
}