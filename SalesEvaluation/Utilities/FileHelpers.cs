using SalesEvaluation.ConfigVariables;
using SalesEvaluation.Model;
using System.Collections.Concurrent;

namespace SalesEvaluation.Utilities
{
    public static class FileHelpers
    {

        // <summary>
        /// Processes a list of IFormFile objects and returns a list of byte arrays.
        /// Checks Implemented in this method:
        /// Check file size is less than 10MB
        /// Check file type .csv
        /// Check file name is not empty and make it a safe file name determined by the application
        /// Check file length is not empty and check length is not only BOM.
        /// If all checks pass, the file is read into a byte array and added to the list.
        /// We will perform all this checks in Parrallel to improve performance.
        /// If any file fails the checks, We store exception for that file in the list but we continue processing other files.

        public static async Task<Dictionary<string, UploadedFiles>> ProcessFormFile(List<IFormFile> files, FilesEnv env)
        {


            if (files != null && files.Count > 10)
            {
                //Run using parallel to improve performance
                return await ProcessFormFileParallelAsync(files, env);
            }
            Dictionary<string, UploadedFiles> fileBytes = new Dictionary<string, UploadedFiles>();

            foreach (var file in files)
            {
                fileBytes[SanitizeFileName(file.Name)] = await ValidateAndReadFileAsync(file, env);
            }


            return fileBytes;
        }


        public static async Task<Dictionary<string, UploadedFiles>> ProcessFormFileParallelAsync(List<IFormFile> files, FilesEnv env)
        {
            var results = new ConcurrentDictionary<string, UploadedFiles>();

            await Parallel.ForEachAsync(files, async (file, token) =>
            {
                var result = await ValidateAndReadFileAsync(file, env);
                results[SanitizeFileName(file.FileName)] = result;
            });

            return results.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }


        private static async Task<UploadedFiles> ValidateAndReadFileAsync(IFormFile file, FilesEnv env)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!env.AllowedFileTypes.Any(x => x.Equals(fileExtension)))
            {
                return new UploadedFiles
                {
                    ErrorMessage = $"Invalid file type: {fileExtension}. Only {string.Join(", ", env.AllowedFileTypes)} files are allowed."
                };
            }

            if (file.Length == 0)
            {
                return new UploadedFiles { ErrorMessage = $"File {file.FileName} is empty." };
            }

            if (file.Length > env.FileSizeLimit)
            {
                return new UploadedFiles
                {
                    ErrorMessage = $"File {file.FileName} exceeds the size limit of {env.FileSizeLimit / 1024 / 1024} MB."
                };
            }

            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                return new UploadedFiles { ErrorMessage = "File name cannot be empty." };
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileContent = memoryStream.ToArray();

            // Check if not only BOM
            if (fileContent.Length <= 3 &&
                fileContent.SequenceEqual(new byte[] { 0xEF, 0xBB, 0xBF }))
            {
                return new UploadedFiles
                {
                    ErrorMessage = $"File {file.FileName} is empty or contains only BOM."
                };
            }

            return new UploadedFiles { bytes = fileContent };
        }


        public static string SanitizeFileName (string fileName)
        {


            return "";
        }
    }
}
