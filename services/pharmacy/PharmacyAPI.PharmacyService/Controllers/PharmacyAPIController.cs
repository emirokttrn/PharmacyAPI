using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PharmacyAPI.Controllers;

/* Inherit your controllers from this class.
 */
// [Claude Agent] - [ApiController] eksikti; bu yuzden POST/PUT action'lardaki CreateXDto/UpdateXDto
// gibi complex-type parametreler [FromBody] olmadan JSON body'den degil query string'den bind
// edilmeye calisiliyordu (ornegin CreateBrandDto.BrandName swagger'da query parametresi olarak
// gorunuyordu). [ApiController] complex type parametreleri varsayilan olarak body'den baglar.
[ApiController]
public abstract class PharmacyAPIController : AbpControllerBase
{
    protected PharmacyAPIController()
    {
        LocalizationResource = typeof(PharmacyAPIResource);
    }
}
