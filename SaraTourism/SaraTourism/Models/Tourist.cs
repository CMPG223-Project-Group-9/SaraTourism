using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public class Tourist
    {
        public int TouristID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public string FullName { get { return Name + " " + Surname; } }
    }
}