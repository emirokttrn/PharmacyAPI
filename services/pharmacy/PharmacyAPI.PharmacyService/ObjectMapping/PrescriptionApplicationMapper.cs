using PharmacyAPI.PrescriptionDtos;
using PharmacyAPI.Prescriptions;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.PrescriptionServices
{
    [Mapper]
    public partial class PrescriptionApplicationMapper : MapperBase<Prescription, PrescriptionDto>
    {
        public override partial PrescriptionDto Map(Prescription source);
        public override partial void Map(Prescription source, PrescriptionDto destination);
    }
}
