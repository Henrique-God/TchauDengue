using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using TchauDengue.Config;

namespace TchauDengue.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;
        public PhotoService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinaryConfig:CloudName"];
            var apiKey = configuration["CloudinaryConfig:ApiKey"];
            var apiSecret = configuration["CloudinaryConfig:ApiSecret"];

            Account acc = new Account(cloud: cloudName, apiKey: apiKey, apiSecret: apiSecret);
            this.cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            ImageUploadResult uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using Stream stream = file.OpenReadStream();
                var UploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "TchauDengue"
                };

                uploadResult = await cloudinary.UploadAsync(UploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await this.cloudinary.DestroyAsync(deleteParams);
        }
    }
}
