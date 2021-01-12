using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralInsurance.Models
{
    public class CLAIM_ListClaimHistory
    {
        public int ClaimId { get; set; }
        public Nullable<System.DateTime> ClaimDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> ClaimAmount { get; set; }
        public string ReasonToClaim { get; set; }
        public Nullable<int> InsuranceId { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}