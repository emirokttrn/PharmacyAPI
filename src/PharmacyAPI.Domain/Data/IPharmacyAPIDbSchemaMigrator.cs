using System.Threading.Tasks;

namespace PharmacyAPI.Data;

public interface IPharmacyAPIDbSchemaMigrator
{
    Task MigrateAsync();
}
