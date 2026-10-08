using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.pharmacyStaffDTO;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.PharmacyStaff
{
    public interface IPharmacyStaffService :IApplicationService
    {
        Task<PharmacyStaffDto> CreateAsync(CreatePharmacyStaffDto request);
        Task<PharmacyStaffDto> GetAsync(Guid id);
        Task<List<PharmacyStaffDto>> GetListAsync();
        Task DeleteAsync(Guid id);

        // [Claude Agent] - Not: yetkilendirme (authorization) bu is paketinin kapsami disinda, once eklenmeli.
        Task<string> GetTcKimlikNoAsync(Guid id);

    }
}