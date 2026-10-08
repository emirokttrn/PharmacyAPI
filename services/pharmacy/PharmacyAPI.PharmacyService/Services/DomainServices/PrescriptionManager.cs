using System;
using System.Threading.Tasks;
using PharmacyAPI.IRepositories;
using PharmacyAPI.Prescriptions;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class PrescriptionManager : DomainService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPharmacyStaffRepository _pharmacyStaffRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PrescriptionManager(
            IPrescriptionRepository prescriptionRepository,
            ICustomerRepository customerRepository,
            IPharmacyStaffRepository pharmacyStaffRepository,
            IGuidGenerator guidGenerator)
        {
            _prescriptionRepository = prescriptionRepository;
            _customerRepository = customerRepository;
            _pharmacyStaffRepository = pharmacyStaffRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<Prescription> CreateAsync(Guid customerId, string fileUrl)
        {
            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("boyle bir musteri yok!");
            }

            return new Prescription(_guidGenerator.Create(), customerId, fileUrl);
        }

        public async Task<Prescription> ApproveAsync(Guid prescriptionId, Guid reviewedByStaffId, string? reviewNote)
        {
            var prescription = await _prescriptionRepository.GetAsync(prescriptionId);
            var staff = await _pharmacyStaffRepository.FindAsync(reviewedByStaffId);
            if (staff == null)
            {
                throw new UserFriendlyException("boyle bir eczaci personeli yok!");
            }

            prescription.Approve(reviewedByStaffId, reviewNote);
            return prescription;
        }

        public async Task<Prescription> RejectAsync(Guid prescriptionId, Guid reviewedByStaffId, string? reviewNote)
        {
            var prescription = await _prescriptionRepository.GetAsync(prescriptionId);
            var staff = await _pharmacyStaffRepository.FindAsync(reviewedByStaffId);
            if (staff == null)
            {
                throw new UserFriendlyException("boyle bir eczaci personeli yok!");
            }

            prescription.Reject(reviewedByStaffId, reviewNote);
            return prescription;
        }
    }
}
