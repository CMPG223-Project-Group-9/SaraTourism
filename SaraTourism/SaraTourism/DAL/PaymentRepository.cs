using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class PaymentRepository
    {
        public int Insert(int bookingId, decimal amountPaid, PaymentMethod method)
        {
            object newId = DbHelper.ExecuteProcWithOutput("sp_Payment_Insert", "p_NewPaymentID",
                DbHelper.Param("p_BookingID", bookingId),
                DbHelper.Param("p_AmountPaid", amountPaid),
                DbHelper.Param("p_PaymentMethod", method.ToString()),
                DbHelper.OutParam("p_NewPaymentID", MySqlDbType.Int32));
            return Convert.ToInt32(newId);
        }

        public decimal GetOutstandingBalance(int bookingId)
        {
            object result = DbHelper.ExecuteProcWithOutput("sp_Payment_GetOutstandingBalance", "p_Balance",
                DbHelper.Param("p_BookingID", bookingId),
                DbHelper.OutParam("p_Balance", MySqlDbType.Decimal));
            return Convert.ToDecimal(result);
        }

        public DataTable GetByBooking(int bookingId)
        {
            return DbHelper.ExecuteProcTable("sp_Payment_GetByBooking", DbHelper.Param("p_BookingID", bookingId));
        }
    }
}