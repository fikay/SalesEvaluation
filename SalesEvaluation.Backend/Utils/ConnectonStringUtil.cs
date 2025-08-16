using static SalesEvaluation.Shared.Models.StorageEnums;

namespace SalesEvaluation.Backend.Utils
{
    public static class ConnectonStringUtil
    {
        ///<summary>
        /// This Util checks the connection string for certain clues to determine what kind of storage the application will use.
        ///</summary>
        ///
        public static StorageType GetStorageTypeFromConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            if (connectionString.Contains("AccountName=", StringComparison.OrdinalIgnoreCase) ||
                connectionString.Contains("DefaultEndpointsProtocol=https;", StringComparison.OrdinalIgnoreCase) ||
                connectionString.Contains("blob.core.windows", StringComparison.OrdinalIgnoreCase))
            {
                return ParseStorageType("Blob");
            }
            else if (connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase) ||
                     connectionString.Contains("Initial Catalog=", StringComparison.OrdinalIgnoreCase) ||
                        connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase))
            {
                return ParseStorageType("SQL");
            }
            else
            {
                throw new InvalidOperationException("Unknown storage type based on the provided connection string.");
            }
        }


        /// <summary>
        /// Returns an Enum representing the storage type based on the connection string.
        /// </summary>
        /// <returns></returns>
        public static StorageType ParseStorageType(string type)
        {
            if (Enum.TryParse<StorageType>(type, ignoreCase: true, out var result))
            {
                return result;
            }
            throw new ArgumentException($"Invalid storage type: {type}");
        }
    }
}
