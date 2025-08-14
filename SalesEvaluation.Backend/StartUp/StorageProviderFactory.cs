

using SalesEvaluation.Backend.Services.Storage;

namespace SalesEvaluation.Backend.StartUp
{
    public class StorageProviderFactory
    {

        public static IStorageService Getprovider(IConfigurationSection section)
        {
            try
            {
                if (section["connectionstring"].Contains("sql"))
                {
                    return new DbService(section["connectionstring"]);
                }

                if (section["connectionstring"].Contains("blob.core.windows"))
                {
                    return new BlobService(section["connectionstring"], section);
                }

                return new DbService(section["connectionstring"]);
            }
            catch 
            {
                throw new Exception("No suitable Storage Connectioon found ");
            }
          
        }
    }
}
