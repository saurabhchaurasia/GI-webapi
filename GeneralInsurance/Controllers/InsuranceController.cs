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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InsuranceController : ApiController
    {
        //to get insurance details for editing insurance(renew2 page)
        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage GetInsuDetails(int id)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.INSURANCEs.Where(x => x.InsuranceId == id).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }
        //to get motor id to post it in insurance table(plan page)
        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage getmotor(int id)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.MOTORs.Where(x => x.ChasisNo == id).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //to get insurance details for specific users(renew page)
        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage GetInsuranceDetails()
        {
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);

            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var u = from insurance in db.INSURANCEs
                        where insurance.UserId == id
                        select new
                        {
                            InsuranceId = insurance.InsuranceId,
                            Plans = insurance.Plans,
                            MotorId = insurance.MotorId
                        };
                var data = u.ToList();
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "user with Id=" + id + "not found");
                }
            }
        }
        //buy page
        [HttpPost]
        [Authorize(Roles = "User")]
        public HttpResponseMessage PostMotor([FromBody] MOTOR moto)
        {
            int userid = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    moto.UserId = userid;
                    db.MOTORs.Add(moto);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, moto);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //plan page

        [HttpPost]
        [Authorize(Roles = "User")]
        public HttpResponseMessage PostInsurance([FromBody] INSURANCE insu)
        {
            int userid = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    insu.UserId = userid;
                    db.INSURANCEs.Add(insu);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, insu);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //renew2 page(renew insurance)
        [HttpPut]
        [Authorize(Roles = "User")]
        public HttpResponseMessage PutInsurance(int id, [FromBody] INSURANCE insu)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.INSURANCEs.Find(id);
                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Product with id" + id + "not found");
                    }
                    data.Plans = insu.Plans;
                    data.Duration = insu.Duration;
                    data.Amount = insu.Amount;
                    data.PolicyStartDate = DateTime.Now;
                    data.PolicyEndDate = insu.PolicyEndDate;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, data);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public class insure
        {
            public string Plans { get; set; }
            public int Duration { get; set; }
            public int Amount { get; set; }
        }

    }
}