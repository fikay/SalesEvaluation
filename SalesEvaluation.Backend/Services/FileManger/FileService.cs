
namespace SalesEvaluation.Backend.Services.FileManger
{
    public class FileService : IFileService
    {
        Task<Dictionary<string, byte[]>> IFileService.ProcessFilesForUpload(Dictionary<string, byte[]> rawFiles)
        {
            throw new NotImplementedException();
        }

        Task IFileService.UploadFilesToBlob(List<byte[]> byteFiles)
        {
            throw new NotImplementedException();
        }
    }
}
