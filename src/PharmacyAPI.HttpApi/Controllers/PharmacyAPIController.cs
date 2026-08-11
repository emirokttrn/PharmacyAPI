using PharmacyAPI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PharmacyAPI.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class PharmacyAPIController : AbpControllerBase
{
    protected PharmacyAPIController()
    {
        LocalizationResource = typeof(PharmacyAPIResource);
    }
}
