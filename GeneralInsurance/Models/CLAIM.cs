//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeneralInsurance.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CLAIM
    {
        public int ClaimId { get; set; }
        public Nullable<System.DateTime> ClaimDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> ClaimAmount { get; set; }
        public string ReasonToClaim { get; set; }
        public Nullable<int> InsuranceId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public  INSURANCE INSURANCE { get; set; }
        public  USER USER { get; set; }
    }
}
