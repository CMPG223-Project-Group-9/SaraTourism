using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class ActivityRepository
    {
        private Activity MapRow(DataRow r)
        {
            return new Activity
            {
                ActivityID = r.GetInt("ActivityID"),
                ActivityName = r.GetStr("ActivityName"),
                Description = r.GetStr("Description"),
                ImagePath = r.GetStr("ImagePath"),
                Location = r.GetStr("Location"),
                DurationMinutes = r.GetInt("DurationMinutes"),
                PricePerPerson = r.GetDec("PricePerPerson"),
                MaxCapacity = r.GetInt("MaxCapacity"),
                IsActive = r.GetBool("IsActive"),
                TimesBooked = r.Table.Columns.Contains("TimesBooked") ? r.GetInt("TimesBooked") : 0
            };
        }

        public int Insert(Activity a)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_Activity_Insert", "p_NewActivityID",
                DbHelper.Param("p_ActivityName", a.ActivityName),
                DbHelper.Param("p_Description", a.Description),
                DbHelper.Param("p_ImagePath", a.ImagePath),
                DbHelper.Param("p_Location", a.Location),
                DbHelper.Param("p_DurationMinutes", a.DurationMinutes),
                DbHelper.Param("p_PricePerPerson", a.PricePerPerson),
                DbHelper.Param("p_MaxCapacity", a.MaxCapacity),
                DbHelper.OutParam("p_NewActivityID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public void Update(Activity a)
        {
            DbHelper.ExecuteProcNonQuery("sp_Activity_Update",
                DbHelper.Param("p_ActivityID", a.ActivityID),
                DbHelper.Param("p_ActivityName", a.ActivityName),
                DbHelper.Param("p_Description", a.Description),
                DbHelper.Param("p_ImagePath", a.ImagePath),
                DbHelper.Param("p_Location", a.Location),
                DbHelper.Param("p_DurationMinutes", a.DurationMinutes),
                DbHelper.Param("p_PricePerPerson", a.PricePerPerson),
                DbHelper.Param("p_MaxCapacity", a.MaxCapacity));
        }

        public void Delete(int activityId)
        {
            DbHelper.ExecuteProcNonQuery("sp_Activity_Delete", DbHelper.Param("p_ActivityID", activityId));
        }

        public Activity GetById(int activityId)
        {
            var table = DbHelper.ExecuteProcTable("sp_Activity_GetById", DbHelper.Param("p_ActivityID", activityId));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }

        public List<Activity> GetAll()
        {
            var table = DbHelper.ExecuteProcTable("sp_Activity_GetAll");
            var list = new List<Activity>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }

        /// <summary>Same as GetAll but includes a TimesBooked popularity count, for the admin listing.</summary>
        public List<Activity> GetAllWithPopularity()
        {
            var table = DbHelper.ExecuteProcTable("sp_Activity_GetAllWithPopularity");
            var list = new List<Activity>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }
    }
}