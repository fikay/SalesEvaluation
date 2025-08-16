using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace SalesEvaluation.Backend.Services.Storage
{
    public class BlobService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobService> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 10);
        private readonly IConfigurationSection _blobDetails;
        private int _currentInUse = 0; // Track the number of concurrent uploads

        // Constructor that initializes the BlobServiceClient
        public BlobService(string connectionString, IConfigurationSection section)
        {
            try
            {
                _blobDetails = section ?? throw new ArgumentNullException(nameof(section));
                using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                _logger = loggerFactory.CreateLogger<BlobService>() ?? throw new ArgumentNullException(nameof(loggerFactory));

                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Use connection string
                    _blobServiceClient = new BlobServiceClient(connectionString);
                }
                else if (!string.IsNullOrEmpty(_blobDetails["uri"]!))
                {
                    // Use managed identity
                    var uri = new Uri(_blobDetails["uri"]!);
                    _blobServiceClient = new BlobServiceClient(uri, new ManagedIdentityCredential());
                }
                else
                {
                    // Neither connection string nor URI provided
                    throw new NullReferenceException("Connection string or blob URI must be provided.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create Blob storage client.", ex);
            }
        }

        public Task DeleteBlob(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListBlobsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// We implement the semaphores to limit the number of concurrent uploads to the blob storage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bytes"></param>
        /// <param name="uploadedBy"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task UploadBlob(string name, byte[] bytes, string uploadedBy, string userId)
        {
            await _semaphore.WaitAsync();
            int inUseNow = Interlocked.Increment(ref _currentInUse);
            try
            {
                _logger.LogInformation($"[Semaphore] In use: {inUseNow}, Available: {_semaphore.CurrentCount}");
                var container = _blobServiceClient.GetBlobContainerClient("sales-evaluation");
                await container.CreateIfNotExistsAsync();
                var blobClient = container.GetBlobClient(name);
                using var stream = new MemoryStream(bytes);
                await blobClient.UploadAsync(stream, overwrite: true);
                _logger.LogInformation($"Blob '{name}' uploaded successfully.");
            }
            catch
            {
                throw;
            }
            finally
            {
                int inUseAfter = Interlocked.Decrement(ref _currentInUse);
                _logger.LogInformation($"[Semaphore] In use: {inUseAfter}, Available: {_semaphore.CurrentCount}");
                _semaphore.Release();
            }
        }



        public async Task<HealthCheckResult> BlobConnectivity()
        {
            try
            {
                // Test the connection by listing containers
                var containers = _blobServiceClient.GetBlobContainersAsync(prefix: _blobDetails.GetValue<string>("ContainerName")).AsPages(default, default);
                await foreach (Azure.Page<BlobContainerItem> container in containers)
                {
                    if (container.Values.Count >= 1)
                    {
                        return HealthCheckResult.Healthy("Blob storage is reachable.");
                    }
                }

                return HealthCheckResult.Unhealthy("Blob storage is reachable but no containers found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to Blob storage.");
                return HealthCheckResult.Unhealthy("Blob storage is not reachable.", ex);
            }
        }
    }
}
