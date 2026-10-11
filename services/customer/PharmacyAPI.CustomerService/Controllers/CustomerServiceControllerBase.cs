using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CustomerService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PharmacyAPI.CustomerService.Controllers;

// [ApiController]: complex-type parametreler (CreateXDto vb.) JSON body'den baglanir.
// [Authorize]: bu servisteki tum uc noktalar gecerli bir JWT ister.
[ApiController]
[Authorize]
public abstract class CustomerServiceControllerBase : AbpControllerBase
{
    protected CustomerServiceControllerBase()
    {
        LocalizationResource = typeof(CustomerServiceResource);
    }
}
