using PharmacyAPI.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PharmacyAPI.Permissions;

public class PharmacyAPIPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PharmacyAPIPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(PharmacyAPIPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PharmacyAPIResource>(name);
    }
}
