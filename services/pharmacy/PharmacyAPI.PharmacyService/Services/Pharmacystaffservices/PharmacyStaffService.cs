using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.PharmacyStaff;
using PharmacyAPI.pharmacyStaffDTO;
using PharmacyAPI.pharmacystaffs;
using Riok.Mapperly.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Mapperly;
using Volo.Abp.Security.Encryption;

namespace PharmacyAPI.Pharmacystaffservices
{
     public class PharmacyStaffService : ApplicationService, IPharmacyStaffService
    {
        private readonly IPharmacyStaffRepository _repository;
        private readonly PharmacyStaffManager _staffManager;
        private readonly IStringEncryptionService _stringEncryptionService;

        public PharmacyStaffService(IPharmacyStaffRepository repository, PharmacyStaffManager staffManager, IStringEncryptionService stringEncryptionService)
        {
            _repository = repository;
            _staffManager = staffManager;
            _stringEncryptionService = stringEncryptionService;
        }

        public async Task<PharmacyStaffDto> CreateAsync(CreatePharmacyStaffDto request)
        {
            var staff = await _staffManager.CreateAsync(
                request.UserId, request.TcKimlikNo, request.Position);

            if (request.LicenseNumber != null)
            {
                staff.SetLicenseNumber(request.LicenseNumber);
            }

            if (request.HireDate.HasValue)
            {
                staff.SetHireDate(request.HireDate);
            }

            await _repository.InsertAsync(staff);
            return MapToMaskedDto(staff);
        }

        public async Task<PharmacyStaffDto> GetAsync(Guid id)
        {
            var staff = await _repository.GetAsync(id);
            return MapToMaskedDto(staff);
        }

        public async Task<List<PharmacyStaffDto>> GetListAsync()
        {
            var result = await _repository.GetListAsync();
            return result.Select(MapToMaskedDto).ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            var staff = await _repository.FindAsync(id);
            if (staff == null)
            {
                throw new UserFriendlyException("Bu id ile bir personel bulunamadi.");
            }
            await _repository.DeleteAsync(id);
        }

        // [Claude Agent] - Not: yetkilendirme (authorization) bu is paketinin kapsami disinda, once eklenmeli.
        public async Task<string> GetTcKimlikNoAsync(Guid id)
        {
            var staff = await _repository.GetAsync(id);
            return _stringEncryptionService.Decrypt(staff.TcKimlikNo);
        }

        private PharmacyStaffDto MapToMaskedDto(PharmacyStaff staff)
        {
            var dto = ObjectMapper.Map<PharmacyStaff, PharmacyStaffDto>(staff);
            dto.TcKimlikNo = MaskTcKimlikNo(staff.TcKimlikNo);
            return dto;
        }

        private string MaskTcKimlikNo(string encryptedTcKimlikNo)
        {
            var plain = _stringEncryptionService.Decrypt(encryptedTcKimlikNo);
            if (string.IsNullOrEmpty(plain) || plain.Length < 7)
            {
                return new string('*', plain?.Length ?? 0);
            }
            return plain.Substring(0, 3) + "****" + plain.Substring(plain.Length - 4);
        }
    }

    [Mapper]
    public partial class PharmacyStaffApplicationMapper : MapperBase<PharmacyStaff, PharmacyStaffDto>
    {
        public override partial PharmacyStaffDto Map(PharmacyStaff source);
        public override partial void Map(PharmacyStaff source, PharmacyStaffDto destination);
    }
}