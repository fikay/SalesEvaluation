using Microsoft.Extensions.Diagnostics.HealthChecks;
using SalesEvaluation.Backend.Services.Storage;
using SalesEvaluation.Backend.Utils;
using SalesEvaluation.Shared.Models;
using static SalesEvaluation.Shared.Models.HealthCheckEnums;
using static SalesEvaluation.Shared.Models.StorageEnums;

namespace SalesEvaluation.Backend.StartUp
{
    public class IntegrationHealthCheck : IHealthCheck
    {
        private readonly string _storageConnectionString;
        public IntegrationHealthCheck (string storageConnectionString)
        {
            _storageConnectionString = storageConnectionString;
        }

        private Dictionary<string, object> _healthCheckResults  = new Dictionary<string, object>();
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_storageConnectionString))
            {
                _healthCheckResults[HealthChecks.StorageConnectionString.ToString()] = HealthCheckResult.Unhealthy("Storage connection string is not configured.");
                return HealthCheckResult.Unhealthy("Connection string not configured") ;
            }

            var storageType = ConnectonStringUtil.GetStorageTypeFromConnectionString(_storageConnectionString);

            //Test storage Health
           var storageStatus = await CheckStorageHealthAsync(storageType);
            if (storageStatus.Status == HealthStatus.Unhealthy)
            {
                _healthCheckResults[HealthChecks.Storage.ToString()] = storageStatus;
            }


            if (_healthCheckResults.Count >= 1)
            {
                //Convert the keys to string and then send it 
                
                return HealthCheckResult.Unhealthy("One or more health checks failed.", null, _healthCheckResults); 
            }
            return HealthCheckResult.Healthy();
        }


        private async Task<HealthCheckResult> CheckStorageHealthAsync(StorageType storage)
        {
            try
            {
                if (storage == StorageType.Sql)
                {
                    var dbService = new DbService(_storageConnectionString);
                    return await dbService.DatabaseConnectivity();
                }
                else  if (storage == StorageType.Blob)
                {
                    var blobService = new BlobService(_storageConnectionString, null);
                    return await blobService.BlobConnectivity();
                    
                }
               
                return HealthCheckResult.Unhealthy("Unsupported storage type.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Storage health check failed.", ex);
            }
        }
    }
}
