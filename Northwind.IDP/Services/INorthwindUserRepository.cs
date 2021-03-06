﻿using Northwind.IDP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.IDP.Services
{
    public interface INorthwindUserRepository
    {
        User GetUserByUsername(string username);
        User GetUserByemail(string email);
        User GetUserBySubjectId(string subjectId);
        User GetUserByEmail(string email);
        User GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<UserLogin> GetUserLoginsBySubjectId(string subjectId);
        IEnumerable<UserClaim> GetUserClaimsBySubjectId(string subjectId);
        IEnumerable<User> GetUserListActive();
        bool AreUserCredentialsValid(string email, string password);
        bool IsUserActive(string subjectId);
        void AddUser(User user);
        void AddUserLogin(string subjectId, string loginProvider, string providerKey);
        void AddUserClaim(string subjectId, string claimType, string claimValue);
        bool Save();
    }
}
