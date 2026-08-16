using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class ReviewRepository
    {
        private TourReview MapRow(DataRow r)
        {
            return new TourReview
            {
                ReviewID = r.GetInt("ReviewID"),
                BookingID = r.GetInt("BookingID"),
                Rating = r.GetInt("Rating"),
                Comment = r.GetStr("Comment"),
                ReviewDate = r.GetDate("ReviewDate"),
                BookingDate = r.Table.Columns.Contains("BookingDate") ? r.GetDate("BookingDate") : DateTime.MinValue,
                ActivityName = r.Table.Columns.Contains("ActivityName") ? r.GetStr("ActivityName") : null,
                GuideName = r.Table.Columns.Contains("GuideName") ? r.GetStr("GuideName") : null
            };
        }

        /// <summary>
        /// Throws a MySqlException (SQLSTATE 45000) if the booking isn't Completed yet -
        /// the stored procedure enforces that rule, not this repository.
        /// </summary>
        public int Insert(int bookingId, int rating, string comment)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_TourReview_Insert", "p_NewReviewID",
                DbHelper.Param("p_BookingID", bookingId),
                DbHelper.Param("p_Rating", rating),
                DbHelper.Param("p_Comment", comment),
                DbHelper.OutParam("p_NewReviewID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public List<TourReview> GetAll()
        {
            var table = DbHelper.ExecuteProcTable("sp_TourReview_GetAll");
            var list = new List<TourReview>();
            foreach (DataRow r in table.Rows) list.Add(MapRow(r));
            return list;
        }
    }
}