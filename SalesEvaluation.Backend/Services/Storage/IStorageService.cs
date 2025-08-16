namespace SalesEvaluation.Backend.Services.Storage
{
    public interface IStorageService
    {
        Task UploadBlob(string name , byte[] bytes , string uploadedBy, string userId);

        Task DeleteBlob(string blobName);

        Task<List<string>> ListBlobsAsync();
    }
}
