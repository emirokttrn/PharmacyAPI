using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.StoreService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PharmacyAPI.StoreService.Controllers;

// [ApiController]: complex-type parametreler (CreateXDto vb.) JSON body'den baglanir.
// [Authorize]: bu servisteki tum uc noktalar gecerli bir JWT ister.
[ApiController]
[Authorize]
public abstract class StoreServiceControllerBase : AbpControllerBase
{
    protected StoreServiceControllerBase()
    {
        LocalizationResource = typeof(StoreServiceResource);
    }
}
