using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.Data;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.ImageRepository
{
    public class ImageRepository : IImageRepository
    {
        private readonly TestTaskDbContext _context;

        public ImageRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<Image> GetImageById(Guid id)
        {
            return await _context.Images.Include(x => x.User).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Image>> GetAllImages()
        {
            return await _context.Images.Include(x => x.User).ToListAsync();
        }

        public async Task CreateImage(Image image)
        {
            User user = await _context.Users.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == image.UserId);

            if (user == null) throw new Exception("User doesnt exist!");

            user.Images ??= new List<Image>();

            user.Images.Add(image);

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task UploadImage(IFormFile image)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

            string path = Environment.GetEnvironmentVariable("ImagePath", EnvironmentVariableTarget.Machine);

            string imagePath = Path.Combine(path, fileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }

        public async Task UpdateImage(Image image)
        {
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(Guid id)
        {
            var image = await GetImageById(id);
            if (image != null)
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}

