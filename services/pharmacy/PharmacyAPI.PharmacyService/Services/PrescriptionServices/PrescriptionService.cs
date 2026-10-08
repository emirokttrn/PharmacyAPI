using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.Prescription;
using PharmacyAPI.PrescriptionDtos;
using PharmacyAPI.Prescriptions;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.PrescriptionServices
{
    public class PrescriptionService : ApplicationService, IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly PrescriptionManager _prescriptionManager;

        public PrescriptionService(IPrescriptionRepository repository, PrescriptionManager prescriptionManager)
        {
            _repository = repository;
            _prescriptionManager = prescriptionManager;
        }

        public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto request)
        {
            var prescription = await _prescriptionManager.CreateAsync(request.CustomerId, request.FileUrl);
            await _repository.InsertAsync(prescription);
            return ObjectMapper.Map<Prescription, PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> GetAsync(Guid id)
        {
            var prescription = await _repository.GetAsync(id);
            return ObjectMapper.Map<Prescription, PrescriptionDto>(prescription);
        }

        public async Task<List<PrescriptionDto>> GetListByCustomerAsync(Guid customerId)
        {
            var prescriptions = await _repository.GetListByCustomerAsync(customerId);
            return ObjectMapper.Map<List<Prescription>, List<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto> ApproveAsync(Guid id, ReviewPrescriptionDto request)
        {
            var prescription = await _prescriptionManager.ApproveAsync(id, request.ReviewedByStaffId, request.ReviewNote);
            await _repository.UpdateAsync(prescription);
            return ObjectMapper.Map<Prescription, PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> RejectAsync(Guid id, ReviewPrescriptionDto request)
        {
            var prescription = await _prescriptionManager.RejectAsync(id, request.ReviewedByStaffId, request.ReviewNote);
            await _repository.UpdateAsync(prescription);
            return ObjectMapper.Map<Prescription, PrescriptionDto>(prescription);
        }
    }
}
