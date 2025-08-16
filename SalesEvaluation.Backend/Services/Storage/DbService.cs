using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace SalesEvaluation.Backend.Services.Storage
{
    public class DbService : IStorageService
    {
        private readonly string _connnectionString;
        public DbService(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new NullReferenceException("Connection string is null");
                }
                _connnectionString = connectionString;
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

        public Task UploadBlob(string name , byte[] bytes , string uploadedBy, string userId)
        {
            if (string.IsNullOrEmpty(name) || bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("Name or file cannot be null or empty.");
            }
            
            
            try
            {
                var storedProcName = "FILES.INSERTFILE";
                Guid userGuid = string.IsNullOrEmpty(userId) ? Guid.Empty : new Guid (userId);
                var procParams = new[]
                {
                    new SqlParameter("@FileName", name),
                    new SqlParameter("@Content", bytes),
                    new SqlParameter("@UserId", userGuid ),
                    new SqlParameter("@UploadedBy", uploadedBy),
                    new SqlParameter("@FileType" ,Path.GetExtension(name))
                };


                using (var dbconnect = new SqlConnection(_connnectionString))
                {
                    dbconnect.Open();
                    using (var command = new SqlCommand(storedProcName, dbconnect))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (procParams != null && procParams.Length > 0)
                        {
                            command.Parameters.AddRange(procParams);
                        }

                        command.ExecuteNonQuery();
                    }

                }

                return Task.CompletedTask;
            }
            catch 
            {
                throw;
            }
            

          
        }

        public Task<HealthCheckResult> DatabaseConnectivity()
        {
            try
            {
                //Test the connection
                using (var dbconnect = new SqlConnection(_connnectionString))
                {
                    dbconnect.Open();
                }
                return Task.FromResult(HealthCheckResult.Healthy("Database connection succeeded"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Database connection failed", ex));
            }
        }
    }
}
