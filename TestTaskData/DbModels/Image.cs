
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskData.DbModels
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
