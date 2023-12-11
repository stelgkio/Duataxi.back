﻿﻿﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Infrastructure.Constants;
using AuthServer.Infrastructure.Data.Identity;
using DuaTaxi.Entities.Core.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Infrastructure.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public IdentityClaimsProfileService(UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            
            var getclaims = await _userManager.GetClaimsAsync(user);
            var role =  getclaims.Where(x=> x.Type.Contains("role")).Select(z=> z.Value).FirstOrDefault();
            var claims = principal.Claims.ToList();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            // note: to dynamically add roles (ie. for users other than consumers - simply look them up by sub id


            // var user = await _userManager.GetUserAsync(context.Subject);

            // var roles = await _userManager.GetRolesAsync(user);

            // foreach (var role in roles)
            // {
            //    context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
            // }
            //4

            //In your Api, somewhere before services.AddAuthentication("Bearer") add a line for JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();.

            claims.Add(new Claim(ClaimTypes.Role,role)); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4
            
            context.IssuedClaims = claims;

          
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
