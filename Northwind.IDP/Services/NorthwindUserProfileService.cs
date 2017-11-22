using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Extensions;
using System.Security.Claims;

namespace Northwind.IDP.Services
{ 
    public class NorthwindUserProfileService : IProfileService
    {
        private readonly INorthwindUserRepository _northwindUserRepository;

        public NorthwindUserProfileService(INorthwindUserRepository marvinUserRepository)
        {
            _northwindUserRepository = marvinUserRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _northwindUserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select
              (c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = _northwindUserRepository.IsUserActive(subjectId);

            return Task.FromResult(0);
        }
    }
}
