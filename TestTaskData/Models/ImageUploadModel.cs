using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskData.Models
{
    public class ImageUploadModel
    {
        public IFormFile ImageData { get; set; }
        public string Title { get; set; }
    }
}
