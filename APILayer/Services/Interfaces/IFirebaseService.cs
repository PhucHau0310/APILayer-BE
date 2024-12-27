namespace APILayer.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string fileUrl);
        Task<string> GetFileUrlAsync(string filePath);
    }
}
