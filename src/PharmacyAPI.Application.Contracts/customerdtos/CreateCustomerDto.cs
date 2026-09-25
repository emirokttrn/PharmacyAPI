using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.customerdtos
{
    // [Claude Agent] - KVKK: musteri olustururken en az KvkkAydinlatma ve AcikRiza rizasi zorunlu (CustomerManager kontrol eder)
    public class ConsentRecordInputDto
    {
        [Required]
        public ConsentType ConsentType { get; set; }

        [Required]
        public bool ConsentGiven { get; set; }

        [Required]
        public string ConsentTextVersion { get; set; }

        public string? IpAddress { get; set; }
    }

    public class CreateCustomerDto
    {
        [Required]
        public Guid UserId{get; set;}
        [Required]
        public string TcKimlikNo {get; set;}
        [Required]
        public string Address {get; set;}
        [Required]

        public DateTime? DateofBirth{get; set;}

        [Required]
        public List<ConsentRecordInputDto> Consents { get; set; } = new List<ConsentRecordInputDto>();
    }

    public class CustomerDto :  AuditedEntityDto<Guid>
    {
     
        public Guid UserId{get; set;}
        
        public string TcKimlikNo {get; set;}

        public string Address {get; set;}
    

        public DateTime? DateofBirth{get; set;}

    }
    public class FilterCustomerDto : PagedAndSortedResultRequestDto
    {
        public string? Filter {get; set;}

        public string? tcKimlikNo{get;set;}
    }



}