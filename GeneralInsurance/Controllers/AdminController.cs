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
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.CLAIMs.Where(b => b.ApprovalStatus == "Pending").ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        //Get All Transactions for admin
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage GetTrans()
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.Transactions.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        // Get single claim details for admin-edit to approve
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.CLAIMs.Where(b => b.ClaimId == id).FirstOrDefault();
                    var data1 = db.INSURANCEs.Find(data.InsuranceId);
                    var data2 = db.MOTORs.Find(data1.MotorId);
                    var db2 = new AdminEdit
                    {
                        ClaimId = data.ClaimId,
                        ClaimDate = data.ClaimDate,
                        ApprovalStatus = data.ApprovalStatus,
                        ClaimAmount = data.ClaimAmount,
                        ReasonToClaim = data.ReasonToClaim,
                        ManufactureYear = data2.ManufactureYear,
                        Model = data2.Model,
                        Type = data2.Type,
                        Plans = data1.Plans
                    };
                    if (db2 != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, db2);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Claim with Id= " + id + " not found");
                    }
                }
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest);
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