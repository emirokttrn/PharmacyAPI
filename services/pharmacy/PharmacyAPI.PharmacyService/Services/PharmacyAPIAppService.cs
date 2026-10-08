using System;
using System.Collections.Generic;
using System.Text;
using PharmacyAPI.Localization;
using Volo.Abp.Application.Services;

namespace PharmacyAPI;

/* Inherit your application services from this class.
 */
public abstract class PharmacyAPIAppService : ApplicationService
{
    protected PharmacyAPIAppService()
    {
        LocalizationResource = typeof(PharmacyAPIResource);
    }
}
