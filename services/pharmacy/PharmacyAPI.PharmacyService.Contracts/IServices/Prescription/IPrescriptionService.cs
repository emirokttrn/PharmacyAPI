using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.PrescriptionDtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Prescription
{
    public interface IPrescriptionService : IApplicationService
    {
        Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto request);
        Task<PrescriptionDto> GetAsync(Guid id);
        Task<List<PrescriptionDto>> GetListByCustomerAsync(Guid customerId);
        Task<PrescriptionDto> ApproveAsync(Guid id, ReviewPrescriptionDto request);
        Task<PrescriptionDto> RejectAsync(Guid id, ReviewPrescriptionDto request);
    }
}
