using Microsoft.Data.SqlClient;

namespace SalesEvaluation.Backend.Services.Storage
{
    public class DbService : IStorageService, IDisposable
    {
        private readonly SqlConnection _connection;
        public DbService(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new NullReferenceException("Connection string is null");
                }
                _connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create SQL connection.", ex);
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

        public Task UploadBlob(string name , byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
