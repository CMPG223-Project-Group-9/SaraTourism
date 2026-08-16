using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public enum PaymentMethod { Card, EFT, Cash }
    public enum PaymentStatus { Pending, Successful, Failed, Refunded }

    public class Payment
    {
        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}