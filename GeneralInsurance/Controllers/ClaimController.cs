using GeneralInsurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GeneralInsurance.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: ("*"))]
    public class ClaimController : ApiController
    {
        [HttpGet]
        [Authorize(Roles ="User")]
        public HttpResponseMessage GetInsurance()
        {
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);

            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var list = db.INSURANCEs.Where(i => i.UserId == id).ToList();
                var result = new List<CLAIM_ListInsurance>();
                foreach (var l in list)
                {
                    result.Add(new CLAIM_ListInsurance { InsuranceId = l.InsuranceId, MotorId = l.MotorId, Plans = l.Plans });
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage GetClaimHistory()
        {
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);

            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var list = db.CLAIMs.Where(c => c.UserId == id).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public HttpResponseMessage PostAddClaim([FromBody] CLAIM claim)
        {
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    db.CLAIMs.Add(claim);
                    claim.ClaimDate = DateTime.Now;
                    claim.ApprovalStatus = "Pending";
                    claim.UserId = id;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, claim);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}