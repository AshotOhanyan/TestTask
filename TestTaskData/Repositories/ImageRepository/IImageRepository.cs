using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.ImageRepository
{
    public interface IImageRepository
    {
        public Task<Image> GetImageById(Guid id);
        public Task<IEnumerable<Image>> GetAllImages();
        public Task CreateImage(Image Image);
        public Task UpdateImage(Image Image);
        public Task DeleteImage(Guid id);
        public Task UploadImage(IFormFile image);
    }
}
