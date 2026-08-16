using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class TourGuideRepository
    {
        private TourGuide MapRow(DataRow r)
        {
            return new TourGuide
            {
                TourGuideID = r.GetInt("TourGuideID"),
                Name = r.GetStr("Name"),
                Specialization = r.GetStr("Specialization"),
                IsActive = r.GetBool("IsActive")
            };
        }

        public int Insert(TourGuide g)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_TourGuide_Insert", "p_NewTourGuideID",
                DbHelper.Param("p_Name", g.Name),
                DbHelper.Param("p_Specialization", g.Specialization),
                DbHelper.OutParam("p_NewTourGuideID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public void Update(TourGuide g)
        {
            DbHelper.ExecuteProcNonQuery("sp_TourGuide_Update",
                DbHelper.Param("p_TourGuideID", g.TourGuideID),
                DbHelper.Param("p_Name", g.Name),
                DbHelper.Param("p_Specialization", g.Specialization));
        }

        public void Delete(int tourGuideId)
        {
            DbHelper.ExecuteProcNonQuery("sp_TourGuide_Delete", DbHelper.Param("p_TourGuideID", tourGuideId));
        }

        public TourGuide GetById(int tourGuideId)
        {
            var table = DbHelper.ExecuteProcTable("sp_TourGuide_GetById", DbHelper.Param("p_TourGuideID", tourGuideId));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }

        public List<TourGuide> GetAll()
        {
            var table = DbHelper.ExecuteProcTable("sp_TourGuide_GetAll");
            var list = new List<TourGuide>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }

        /// <summary>Guides with no overlapping booking for the given date/time range.</summary>
        public List<TourGuide> GetAvailable(DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
        {
            var table = DbHelper.ExecuteProcTable("sp_TourGuide_GetAvailable",
                DbHelper.Param("p_BookingDate", bookingDate.Date),
                DbHelper.Param("p_StartTime", startTime),
                DbHelper.Param("p_EndTime", endTime));
            var list = new List<TourGuide>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }
    }
}