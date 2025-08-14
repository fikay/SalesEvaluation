namespace SalesEvaluation.Backend.Services.FileManger
{
    public interface IFileService
    {
        Task UploadFilesToBlob(List<byte[]> byteFiles);

        Task<Dictionary<string, byte[]>> ProcessFilesForUpload(Dictionary<string, byte[]> rawFiles);

    }
}
