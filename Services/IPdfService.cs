using CloudinaryDotNet.Actions;

namespace TchauDengue.Services
{
    public interface IPdfService
    {
        Task<RawUploadResult> AddPdfAsync(IFormFile file);
        Task<DeletionResult> DeletePdfAsync(string publicId);
    }
}