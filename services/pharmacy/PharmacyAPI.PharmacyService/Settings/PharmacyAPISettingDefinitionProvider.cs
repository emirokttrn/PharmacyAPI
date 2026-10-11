using Volo.Abp.Settings;

namespace PharmacyAPI.Settings;

public class PharmacyAPISettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(PharmacyAPISettings.MySetting1));
    }
}
