using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.customerdtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.customer
{
    public interface ICustomerService : IApplicationService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerDto request);
        Task<CustomerDto> GetAsync(Guid id);
        Task<PagedResultDto<CustomerDto>> FilterAsync(FilterCustomerDto filter);
        Task DeleteAsync(Guid id);

        // [Claude Agent] - KVKK: tam (sifresi cozulmus) TC Kimlik No sadece bu ayri endpoint'te donuyor.
        // Not: yetkilendirme (authorization) bu is paketinin kapsami disinda, once eklenmeli.
        Task<string> GetTcKimlikNoAsync(Guid id);

    }
}