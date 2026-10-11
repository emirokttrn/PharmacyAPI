using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.IRepositories;
using PharmacyAPI.pharmacystaffs;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Security.Encryption;

namespace PharmacyAPI.DomainServices
{
    public class PharmacyStaffManager :DomainService
    {
        private readonly IPharmacyStaffRepository _pharmacyStaffRepository;
        private readonly IIdentityUserRepository _ıdentityUserRepository;
        private readonly IStringEncryptionService _stringEncryptionService;

        private readonly IGuidGenerator _guidGenerator;

        public PharmacyStaffManager(IPharmacyStaffRepository pharmacyStaffRepository,IIdentityUserRepository ıdentityUserRepository, IStringEncryptionService stringEncryptionService, IGuidGenerator guidGenerator)
        {
            _pharmacyStaffRepository=pharmacyStaffRepository;
            _guidGenerator=guidGenerator;
            _ıdentityUserRepository=ıdentityUserRepository;
            _stringEncryptionService=stringEncryptionService;
        }

// [Claude Agent] - TC Kimlik No veritabanina yazilmadan once sifreleniyor (bkz. CustomerManager'daki ayni desen)
public async Task<PharmacyStaff> CreateAsync(Guid userId, string tcKimlikNo, string position)
        {
            await _ıdentityUserRepository.GetAsync(userId);

            var alreadystaff= await _pharmacyStaffRepository.FindUserIdAsync(userId);
            if(alreadystaff!=null)
            {
                throw new UserFriendlyException("bu kullanici zaten bi profile sahip");
            }

            if (!PharmacyStaff.IsValidTcKimlikNoFormat(tcKimlikNo))
            {
                throw new UserFriendlyException("TC kimlik no sadece 11 haneli numaralardan olusur");
            }

            var encryptedTc = _stringEncryptionService.Encrypt(tcKimlikNo);

            var tcInUse = await _pharmacyStaffRepository.FindByTcKimlikNoAsync(encryptedTc);
            if(tcInUse !=null)
            {
                throw new UserFriendlyException("bu tc'ye sakip bi profil var");
            }

            return new PharmacyStaff(_guidGenerator.Create(), userId, encryptedTc, position);
        }


    }
}