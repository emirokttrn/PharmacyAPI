using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
namespace PharmacyAPI.CustomerService.Contracts.Dtos.CustomerFilters
{
    public class CustomerFilter : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }
}