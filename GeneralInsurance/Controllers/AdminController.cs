using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeneralInsurance.Models;
using System.Web.Http.Cors;

namespace GeneralInsurance.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AdminController : ApiController
    {
        // Get all claims for admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Get()
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.CLAIMs.Where(b=>b.ApprovalStatus=="Pending").ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }

        //Get All Transactions for admin
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage GetTrans()
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.Transactions.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }

        }

        // Get single claim details for admin-edit to approve
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Get(int id)
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.CLAIMs.Find(id);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Claim with Id= " + id + " not found");
                }
            }
        }

        //Put single claim data by admin for approval
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put(int id, [FromBody] CLAIM claim)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.CLAIMs.Find(id);
                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Claim with Id = " + id + "not found.");
                    }
                    else
                    {
                        data.ApprovalStatus = claim.ApprovalStatus;
                        data.ClaimAmount = claim.ClaimAmount;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}