using Azure.Identity;
using Azure.Storage.Blobs;

namespace SalesEvaluation.Backend.Services.Storage
{
    public class BlobService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
       
        // Constructor that initializes the BlobServiceClient
        public BlobService(string connectionString , IConfigurationSection section)
        {
          try
          {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // Use connection string
                    _blobServiceClient = new BlobServiceClient(connectionString);
                }
                else if (!string.IsNullOrEmpty(section["uri"]))
                {
                    // Use managed identity
                    var uri = new Uri(section["uri"]);
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

        public Task UploadBlob(string name, byte[] bytes)
        {
            throw new NotImplementedException();
        }

      
    }
}
