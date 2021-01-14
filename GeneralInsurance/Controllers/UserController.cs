using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using GeneralInsurance.Models;

namespace GeneralInsurance.Controllers
{
    [EnableCors("*","*","*")]
    public class UserController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUsers()
        {
            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                var data = db.USERS.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }


        [HttpPost]
        public HttpResponseMessage PostUserEmail([FromBody]USER U)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.USERS.Where(user => user.Email == U.Email).FirstOrDefault();
                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "NOT_FOUND");
                    }
                    
                    return Request.CreateResponse(HttpStatusCode.OK, data.UserId);
                    //changed here from data to data.userid
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public class ParamPostNewPassword
        {
            public string Password { get; set; }
            public int UserId { get; set; }
            public string DrivingLiscence { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage PostNewPassword([FromBody] ParamPostNewPassword U)
        {
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var data = db.USERS.Find(U.UserId);

                    if(data.DrivingLiscence == U.DrivingLiscence)
                    {
                        data.Password = U.Password;

                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, "PASSWORD_CHANGED");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "INCORRECT_DETAILS");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage RegisterUser([FromBody]USER rm)
        {
            using(GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.USERS.Add(new USER
                        {
                            Email = rm.Email,
                            Address = rm.Address,
                            ContactNo = rm.ContactNo,
                            DateOfBirth = rm.DateOfBirth,
                            Name = rm.Name,
                            DrivingLiscence = rm.DrivingLiscence,
                            Password = rm.Password
                        });

                        db.SaveChanges();
                        var data = new { rm.Email, rm.Password };
                        return Request.CreateResponse(HttpStatusCode.Created, data); 
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "VALIADTION_ERROR");
                    }
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }

        // route : /api/user/RoleDetails
        [HttpGet]
        [Authorize]
        public HttpResponseMessage RoleDetails()
        {
            
            try
            {
                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    RoleModel rm = new RoleModel();

                    int userid = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);
                    var roles = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                    var emails = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "username").Select(c => c.Value);

                    rm.Email = string.Join(",", emails.ToList());
                    rm.Role = string.Join(",", roles.ToList());
                    rm.Name = db.USERS.Find(userid).Name;

                    return Request.CreateResponse(HttpStatusCode.OK, rm);
                }
            }
            catch (Exception)
            {
                RoleModel rm = new RoleModel();
                rm.Name = "admin";
                rm.Email = "admin1@gmail.com";
                rm.Role = "Admin";
                return Request.CreateResponse(HttpStatusCode.OK, rm);
                
            }

        }
        

        [Authorize(Roles = "User")]
        [HttpGet]
        public HttpResponseMessage GetInsuranceDetails()
        {
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);

            using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
            {
                try
                {
                    var u = from insurance in db.INSURANCEs
                            where insurance.UserId == id
                            select new
                            {
                                InsuranceId = insurance.InsuranceId,
                                Plans = insurance.Plans,
                                Duration = insurance.Duration,
                                PolicyStartDate = insurance.PolicyStartDate,
                                PolicyEndDate = insurance.PolicyEndDate,
                                Amount = insurance.Amount,
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
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
        }

        [Authorize(Roles ="User")]
        [HttpGet]
        public HttpResponseMessage GetClaimHistory()
        {
            try
            {
                int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).Name);

                using (GeneralInsuranceEntities db = new GeneralInsuranceEntities())
                {
                    var u = from claims in db.CLAIMs
                            where claims.UserId == id
                            select new
                            {
                                claimId = claims.ClaimId,
                                claimAmount = claims.ClaimAmount,
                                claimDate = claims.ClaimDate,
                                claimStatus = claims.ApprovalStatus,
                                claimReason = claims.ReasonToClaim
                            };
                    var result = u.ToList();
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No claims done by user");
                    }
                }
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

    }
}