
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestTaskData.DbModels
{
    public class User 
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<Friendship>? Friends { get; set; }
    }
}
