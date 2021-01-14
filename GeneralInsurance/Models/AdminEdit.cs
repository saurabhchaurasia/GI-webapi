using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralInsurance.Models
{
    public class AdminEdit
    {
        public int ClaimId { get; set; }
        public Nullable<System.DateTime> ClaimDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> ClaimAmount { get; set; }
        public string ReasonToClaim { get; set; }
        public Nullable<int> ManufactureYear { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Plans { get; set; }
    }
}