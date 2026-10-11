using Microsoft.Extensions.Localization;
using PharmacyAPI.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PharmacyAPI;

[Dependency(ReplaceServices = true)]
public class PharmacyAPIBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<PharmacyAPIResource> _localizer;

    public PharmacyAPIBrandingProvider(IStringLocalizer<PharmacyAPIResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
