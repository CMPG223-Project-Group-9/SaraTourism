using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class BookingRepository
    {
        private Booking MapRow(DataRow r)
        {
            BookingStatus status;
            Enum.TryParse(r.GetStr("BookingStatus"), true, out status);

            return new Booking
            {
                BookingID = r.GetInt("BookingID"),
                TouristID = r.GetInt("TouristID"),
                ActivityID = r.GetInt("ActivityID"),
                TourGuideID = r.GetIntOrNull("TourGuideID"),
                AdminID = r.GetIntOrNull("AdminID"),
                BookingDate = r.GetDate("BookingDate"),
                StartTime = r.GetTime("StartTime"),
                EndTime = r.GetTime("EndTime"),
                NumberOfPeople = r.GetInt("NumberOfPeople"),
                TotalAmount = r.GetDec("TotalAmount"),
                BookingStatus = status,
                CreatedDate = r.GetDate("CreatedDate"),
                TouristName = r.Table.Columns.Contains("TouristName") ? r.GetStr("TouristName") : null,
                TouristSurname = r.Table.Columns.Contains("TouristSurname") ? r.GetStr("TouristSurname") : null,
                ActivityName = r.Table.Columns.Contains("ActivityName") ? r.GetStr("ActivityName") : null,
                GuideName = r.Table.Columns.Contains("GuideName") ? r.GetStr("GuideName") : null
            };
        }

        /// <summary>Checks whether the activity has capacity left on the given date for the requested group size.</summary>
        public bool CheckAvailability(int activityId, DateTime bookingDate, int numberOfPeople)
        {
            object result = DbHelper.ExecuteProcWithOutput("sp_Booking_CheckAvailability", "p_IsAvailable",
                DbHelper.Param("p_ActivityID", activityId),
                DbHelper.Param("p_BookingDate", bookingDate.Date),
                DbHelper.Param("p_NumberOfPeople", numberOfPeople),
                DbHelper.OutParam("p_IsAvailable", MySqlDbType.Byte));
            return Convert.ToInt32(result) == 1;
        }

        public int Insert(Booking b)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_Booking_Insert", "p_NewBookingID",
                DbHelper.Param("p_TouristID", b.TouristID),
                DbHelper.Param("p_ActivityID", b.ActivityID),
                DbHelper.Param("p_TourGuideID", b.TourGuideID),
                DbHelper.Param("p_AdminID", b.AdminID),
                DbHelper.Param("p_BookingDate", b.BookingDate.Date),
                DbHelper.Param("p_StartTime", b.StartTime),
                DbHelper.Param("p_EndTime", b.EndTime),
                DbHelper.Param("p_NumberOfPeople", b.NumberOfPeople),
                DbHelper.Param("p_TotalAmount", b.TotalAmount),
                DbHelper.OutParam("p_NewBookingID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public void AllocateGuide(int bookingId, int tourGuideId, int adminId)
        {
            DbHelper.ExecuteProcNonQuery("sp_Booking_AllocateGuide",
                DbHelper.Param("p_BookingID", bookingId),
                DbHelper.Param("p_TourGuideID", tourGuideId),
                DbHelper.Param("p_AdminID", adminId));
        }

        /// <summary>Admin confirms the tourist showed up. Moves Confirmed -> Completed and unlocks the review.</summary>
        public void ConfirmArrival(int bookingId)
        {
            DbHelper.ExecuteProcNonQuery("sp_Booking_ConfirmArrival", DbHelper.Param("p_BookingID", bookingId));
        }

        /// <summary>Admin cancels a no-show booking.</summary>
        public void Cancel(int bookingId)
        {
            DbHelper.ExecuteProcNonQuery("sp_Booking_Delete", DbHelper.Param("p_BookingID", bookingId));
        }

        public Booking GetById(int bookingId)
        {
            var table = DbHelper.ExecuteProcTable("sp_Booking_GetById", DbHelper.Param("p_BookingID", bookingId));
            return table.Rows.Count > 0 ? MapRow(table.Rows[0]) : null;
        }

        public List<Booking> GetAll()
        {
            var table = DbHelper.ExecuteProcTable("sp_Booking_GetAll");
            var list = new List<Booking>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }

        public List<Booking> GetByTourist(int touristId)
        {
            var table = DbHelper.ExecuteProcTable("sp_Booking_GetByTourist", DbHelper.Param("p_TouristID", touristId));
            var list = new List<Booking>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }
    }
}