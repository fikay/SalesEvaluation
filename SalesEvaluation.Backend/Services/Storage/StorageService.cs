using SalesEvaluation.Model;

namespace SalesEvaluation.Backend.Services.Storage
{
    public class StorageService
    {
        private readonly IStorageService _storageService;
        public StorageService(IStorageService storage) 
        {
            _storageService = storage;
        }


        /// <summary>
        /// Uploads files to the storage service chosen at program initialization.
        /// 
        /// Supported storage services:
        /// - Azure Blob Storage
        /// - SQL Server
        /// 
        /// Steps performed:
        /// 1. Sanitize the file name to remove unsafe or invalid characters.
        /// 2. Optionally transform or preprocess the file content if required.
        /// 3. Upload the file to the configured storage provider.
        /// 4. Return the status/result for each uploaded file.
        /// 
        /// </summary>
        /// <param name="files">Collection of files to be uploaded.</param>
        /// <returns>Upload result information for each file.</returns>
        public async Task FileUpload(Dictionary<string, UploadedFiles> files)
        {
            //Perform all tasks asychronously
            var tasks = new Task[files.Count];  
            int index = 0;
            foreach (var file in files)
            {
                tasks[index++] = _storageService.UploadBlob(file.Key, file.Value.bytes);
            }
        }

    }
}
