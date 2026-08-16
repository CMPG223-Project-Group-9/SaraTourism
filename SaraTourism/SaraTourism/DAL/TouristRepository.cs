using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class TouristRepository
    {
        private Tourist MapRow(DataRow r)
        {
            return new Tourist
            {
                TouristID = r.GetInt("TouristID"),
                Name = r.GetStr("Name"),
                Surname = r.GetStr("Surname"),
                Email = r.GetStr("Email"),
                PasswordHash = r.GetStr("PasswordHash"),
                DateOfBirth = r.GetDate("DateOfBirth"),
                CreatedDate = r.GetDate("CreatedDate"),
                IsActive = r.GetBool("IsActive")
            };
        }

        public int Insert(Tourist t)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_Tourist_Insert", "p_NewTouristID",
                DbHelper.Param("p_Name", t.Name),
                DbHelper.Param("p_Surname", t.Surname),
                DbHelper.Param("p_Email", t.Email),
                DbHelper.Param("p_PasswordHash", t.PasswordHash),
                DbHelper.Param("p_DateOfBirth", t.DateOfBirth),
                DbHelper.OutParam("p_NewTouristID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public void Update(Tourist t)
        {
            DbHelper.ExecuteProcNonQuery("sp_Tourist_Update",
                DbHelper.Param("p_TouristID", t.TouristID),
                DbHelper.Param("p_Name", t.Name),
                DbHelper.Param("p_Surname", t.Surname),
                DbHelper.Param("p_Email", t.Email),
                DbHelper.Param("p_DateOfBirth", t.DateOfBirth));
        }

        public void Deactivate(int touristId)
        {
            DbHelper.ExecuteProcNonQuery("sp_Tourist_Delete", DbHelper.Param("p_TouristID", touristId));
        }

        public Tourist GetById(int touristId)
        {
            var table = DbHelper.ExecuteProcTable("sp_Tourist_GetById", DbHelper.Param("p_TouristID", touristId));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }

        public System.Collections.Generic.List<Tourist> GetAll()
        {
            var table = DbHelper.ExecuteProcTable("sp_Tourist_GetAll");
            var list = new System.Collections.Generic.List<Tourist>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }

        /// <summary>Returns the stored hash for a given email so the caller can verify it, or null if not found.</summary>
        public Tourist GetForLogin(string email)
        {
            var table = DbHelper.ExecuteProcTable("sp_Tourist_Login", DbHelper.Param("p_Email", email));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }
    }
}