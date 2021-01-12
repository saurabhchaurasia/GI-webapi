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
    public class TransactionController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage Get(int id)//for generating ticket
       {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities()){
                var transaction = db.Transactions.Find(id);
                var user = db.USERS.Find(transaction.UserId);
                var insurance = db.INSURANCEs.Where(t => t.UserId == transaction.UserId).FirstOrDefault();

                var db2 = new GetTicket
                {
                    Name = user.Name,
                    Address = user.Address,
                    TransactionId = transaction.TransactionId,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    PolicyStartDate = insurance.PolicyStartDate,
                    PolicyEndDate = insurance.PolicyEndDate,
                    Plans = insurance.Plans
                };
                if (insurance != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, db2);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Transaction with Id= " + id + " not found");
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage GetTrans()//for getting transactionId in payment component(Payment whick is done last)
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.Transactions.OrderByDescending(p => p.TransactionId).FirstOrDefault();
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Transactions not found");
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public HttpResponseMessage GetAmount(int id) //for getting insurance details in payment page
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.INSURANCEs.Where(b => b.MotorId == id).FirstOrDefault();
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Transaction with Id= " + id + " not found");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public HttpResponseMessage Post([FromBody] Transaction trans)//adding transactions
        {
            int userid = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    trans.UserId = userid;
                    trans.Date = DateTime.Now;
                    db.Transactions.Add(trans);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, trans);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}