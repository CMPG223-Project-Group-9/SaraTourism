using System;
using System.Data;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class AdminRepository
    {
        private AdminUser MapRow(DataRow r)
        {
            return new AdminUser
            {
                AdminID = r.GetInt("AdminID"),
                AdminName = r.GetStr("AdminName"),
                Email = r.GetStr("Email"),
                PasswordHash = r.GetStr("PasswordHash")
            };
        }

        public AdminUser GetForLogin(string email)
        {
            var table = DbHelper.ExecuteProcTable("sp_Admin_Login", DbHelper.Param("p_Email", email));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }
    }
}