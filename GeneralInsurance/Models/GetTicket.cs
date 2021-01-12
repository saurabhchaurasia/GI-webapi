using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralInsurance.Models
{
    public class GetTicket
    {
        public int TransactionId { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> PolicyStartDate { get; set; }
        public Nullable<System.DateTime> PolicyEndDate { get; set; }
        public string Plans { get; set; }

    }
}