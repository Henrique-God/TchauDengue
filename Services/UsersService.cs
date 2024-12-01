using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Exceptions;
using TchauDengue.Models;
using TchauDengue.Providers;

namespace TchauDengue.Services
{
    public class UsersService: IUsersService
    {
        public UsersService(DataContext dataContext, IPhotoService photoService, IPdfService pdfService)
        {
            this.DataContext = dataContext;
            this.PhotoService = photoService;
            this.HttpClient = new HttpClient();
            this.PdfService = pdfService;
        }

        private DataContext DataContext { get; }
        private IPhotoService PhotoService { get; }
        private HttpClient HttpClient { get; }
        private IPdfService PdfService { get; }

        public async Task<IEnumerable<UserReturnDTO>> GetUsers()
        {
            List<User> users = await this.DataContext.Users.ToListAsync();
            List<UserReturnDTO> userReturnDTOs = new List<UserReturnDTO>();
            foreach (var user in users)
            {
                userReturnDTOs.Add(new UserReturnDTO(user));
            }
            return userReturnDTOs;
        }

        public async Task<User> Register(RegisterDTO registerDTO)
        {
            if (await this.DataContext.Users.AnyAsync(u => u.UserName == registerDTO.UserName))
            {
                throw new UserAlreadyExistsException("Nome de usuário em uso!");
            }
            if (await this.DataContext.Users.AnyAsync(u => u.Email == registerDTO.Email))
            {
                throw new UserAlreadyExistsException("Email em uso!");
            }
            if (await this.DataContext.Users.AnyAsync(u => u.SocialNumber == registerDTO.SocialNumber))
            {
                throw new UserAlreadyExistsException("CPF em uso!");
            }

            string validation = ValidationStatus.VALIDATED;

            if(registerDTO.Role == Roles.OPERATOR)
            {
                validation = ValidationStatus.UNDER_VALIDATION;
            }
            using HMACSHA512 hmac = new HMACSHA512();

            User user = new User()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                SocialNumber = registerDTO.SocialNumber,
                ZipCode = registerDTO.ZipCode,
                Validated = validation,
                Role = registerDTO.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            this.DataContext.Users.Add(user);
            await this.DataContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> CheckLogin(string userName, string password)
        {
            User user = await this.DataContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return false;

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return false;
            }

            return true;
        }

        public async Task<User> FindByUserName(string userName)
        {
            User user = await this.DataContext.Users
                .Include(w => w.WikiPages)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            return user;
        }

        public async Task<bool> UpdateUser(User user, UpdateUserDTO updateDto)
        {
            if (user.UserName != updateDto.UserName &&
                await this.DataContext.Users.AnyAsync(u => u.UserName == updateDto.UserName))
            {
                throw new UserAlreadyExistsException("Nome de usuário em uso!");
            }

            if (user.Email != updateDto.Email &&
                await this.DataContext.Users.AnyAsync(u => u.Email == updateDto.Email))
            {
                throw new UserAlreadyExistsException("Email em uso!");
            }

            if (user.UserName != updateDto.UserName) user.UserName = updateDto.UserName;
            if (user.Email != updateDto.Email) user.Email = updateDto.Email;
            if (user.ZipCode != updateDto.ZipCode) user.ZipCode = updateDto.ZipCode;
            if (user.Role != updateDto.Role) user.Role = updateDto.Role;

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                using HMACSHA512 hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(updateDto.Password));
                user.PasswordSalt = hmac.Key;
            }

            user.UpdatedAt = DateTime.UtcNow;

            this.DataContext.Users.Update(user);
            return await this.DataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AproveUser(int userId)
        {
            User user = await this.DataContext.Users.FindAsync(userId);

            if (user == null) return false;

            user.UpdatedAt = DateTime.UtcNow;
            user.Validated = ValidationStatus.VALIDATED;

            this.DataContext.Users.Update(user);
            return await this.DataContext.SaveChangesAsync() > 0;
        }
        public async Task<Picture> AddPhoto(User user, IFormFile photo)
        {
            var result = await this.PhotoService.AddPhotoAsync(photo);

            if (result.Error != null) throw new Exception("Unable to upload photo");

            Picture picture = new Picture
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.ProfilePicture = result.SecureUrl.AbsoluteUri;
            user.PicturePublicId = result.PublicId;

            await this.DataContext.SaveChangesAsync();

            return picture;
        }

        public async Task<Picture> AddDocument(User user, IFormFile document)
        {
            var result = await this.PdfService.AddPdfAsync(document);

            if (result.Error != null) throw new Exception("Unable to upload Document");

            Picture picture = new Picture
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.PdfUrl = result.SecureUrl.AbsoluteUri;
            user.PdfPublicId = result.PublicId;

            await this.DataContext.SaveChangesAsync();

            return picture;
        }

        public async Task<byte[]> GetDocument(User user, string pdfUrl)
        {
            User? documentOwner = await this.DataContext.Users.FirstOrDefaultAsync(x => x.PdfUrl == pdfUrl);

            if(user.Role == Roles.ADMIN || documentOwner.UserName == user.UserName)
            {
                try
                {
                    HttpResponseMessage response = await this.HttpClient.GetAsync(pdfUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to download PDF: {response.StatusCode}");
                    }

                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao baixar o PDF: {ex.Message}", ex);
                }
            }

            return null;
        }

    }
}
