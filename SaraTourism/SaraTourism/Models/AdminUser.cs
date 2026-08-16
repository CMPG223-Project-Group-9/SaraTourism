using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    // Named AdminUser (not "Admin") to avoid clashing with the Areas.Admin namespace
    public class AdminUser
    {
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}