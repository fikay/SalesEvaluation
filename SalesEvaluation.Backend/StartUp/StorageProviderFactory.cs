using SalesEvaluation.Backend.Services.Storage;
using SalesEvaluation.Backend.Utils;
using static SalesEvaluation.Shared.Models.StorageEnums;

namespace SalesEvaluation.Backend.StartUp
{
    public class StorageProviderFactory
    {

        public static IStorageService Getprovider(IConfigurationSection section)
        {
            try
            {
                if (section == null)
                {
                    throw new ArgumentNullException("Configuration section is null or empty.");
                }

                var storageType = ConnectonStringUtil.GetStorageTypeFromConnectionString(section["connectionstring"]!);

                if (storageType == StorageType.Blob)
                {
                    return new BlobService(section["connectionstring"]!, section);
                }

                if (storageType == StorageType.Sql)
                {
                    return new DbService(section["connectionstring"]!);
                }

                return new DbService(section["connectionstring"]!);
            }
            catch 
            {
                throw;
            }
          
        }
    }
}
