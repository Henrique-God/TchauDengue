using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TchauDengue.Services
{
    public class PdfService : IPdfService
    {
        private readonly Cloudinary _cloudinary;

        public PdfService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinaryConfig:CloudName"];
            var apiKey = configuration["CloudinaryConfig:ApiKey"];
            var apiSecret = configuration["CloudinaryConfig:ApiSecret"];

            Account account = new Account(cloud: cloudName, apiKey: apiKey, apiSecret: apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<RawUploadResult> AddPdfAsync(IFormFile file)
        {
            RawUploadResult uploadResult = new RawUploadResult();

            if (file.Length > 0)
            {
                using Stream stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "TchauDengue/PDFs",
                    AccessMode = "public"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePdfAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw // Define o tipo de recurso como "raw" para deletar PDFs
            };

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
