using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralInsurance.Models
{
    public class CLAIM_ListInsurance
    {
        public int InsuranceId { get; set; }
        public string Plans { get; set; }
        public Nullable<int> MotorId { get; set; }
    }
}