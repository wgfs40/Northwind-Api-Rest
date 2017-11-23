using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Northwind.IDP
{
    public static class Config
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim(JwtClaimTypes.Role,"Freelance")
                }
            },
            new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere"),
                    new Claim(JwtClaimTypes.Role,"PayingUser")
                }
            }
        };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                //new IdentityResource("roles","Your role(s)",new List<string>(){ "role"})
                new IdentityResource
                {
                    Name = "roles",
                    UserClaims = new List<string>{"role"},
                    DisplayName = "Your role(s)"
                },
                new IdentityResource
                {
                    Name="subscriptionlevel",
                    DisplayName="Your subscription level",
                    UserClaims = new List<string>{ "subscriptionlevel" }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("northwindapi","North Wind API",new List<string>(){ "role"})
                {
                    ApiSecrets = { new Secret("apisecret".Sha256())}
                }
                
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client {
                    ClientName = "Northwind Systems",
                    ClientId = "northwindclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false,
                    AccessTokenLifetime = 120,

                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,

                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44372/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "northwindapi",
                        "subscriptionlevel"
                    },                    
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44372/signout-callback-oidc"
                    }
                    //AlwaysIncludeUserClaimsInIdToken = true
                }

            };
        }
    }
}
