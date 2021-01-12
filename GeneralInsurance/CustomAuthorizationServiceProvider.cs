using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using GeneralInsurance.Models;

namespace GeneralInsurance
{
    public class CustomAuthorizationServiceProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            GeneralInsuranceEntities db = new GeneralInsuranceEntities();
            ADMIN admin = db.ADMINS.Where(a => a.EmailId == context.UserName).FirstOrDefault();
            USER user = db.USERS.Where(u => u.Email == context.UserName).FirstOrDefault();

            if (admin != null && context.Password == admin.Password)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                identity.AddClaim(new Claim("username", admin.EmailId ));
                identity.AddClaim(new Claim(ClaimTypes.Name, Convert.ToString(admin.AdminId) ));
                context.Validated(identity);
            }
            else if (user != null && context.Password == user.Password)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                identity.AddClaim(new Claim("username", user.Email ));
                identity.AddClaim(new Claim(ClaimTypes.Name, Convert.ToString(user.UserId) ));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid grant", "Provided username and password is incorrect");
                return;
            }
        }
    }
}