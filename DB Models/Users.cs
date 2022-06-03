using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace_Web_Portal.DB_Models
{
    public class Users
    {
        public string user_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string user_image { get; set; }
        public string user_type { get; set; }
    }
}
