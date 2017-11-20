using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Northwind.UI.Web.Services;
using IdentityModel.Client;
using System.Globalization;

namespace Northwind.UI.Web.Helpers
{
    public  class NorthwindTrackerHttpClient : INorthwindHttpClient
    {
        private  readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient client = new HttpClient();
        public  NorthwindTrackerHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetClient()
        {
            string accessToken = string.Empty;

            //accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            // should we renew access & refresh tokens?
            // get expires_at value
            var expires_at = await _httpContextAccessor.HttpContext.GetTokenAsync("expires_at");

            // compare - make sure to use the exact date formats for comparison
            // (UTC, in this case)
            if (string.IsNullOrWhiteSpace(expires_at)
                || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime() < DateTime.UtcNow))
            {
                accessToken = await RenewTokens();
            }
            else
            {
                // get access token
                accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.SetBearerToken(accessToken);
            }


            //if (requestedVersion != null)
            //{
            //    //through a custom request header
            //    client.DefaultRequestHeaders.Add("api-version", requestedVersion);
            //}

            client.BaseAddress = new Uri("https://localhost:44304/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private async Task<string> RenewTokens()
        {
            //get the current HttpContext to access the token
            var currentContext = _httpContextAccessor.HttpContext;

            //get the metada
            var discoveryClient = new DiscoveryClient("https://localhost:44384/");
            var metaDataresponse = await discoveryClient.GetAsync();

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(metaDataresponse.TokenEndpoint, "northwindclient","secret");

            // get the saved refresh token
            var currentrefreshToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentrefreshToken);

            if (!tokenResult.IsError)
            {
                // Save the tokens

                // get auth info
                var authenticateInfo = await currentContext.AuthenticateAsync("Cookies");

                // create a new value for expires_at, and save it
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                authenticateInfo.Properties.UpdateTokenValue("expires_at",
                    expiresAt.ToString("o",CultureInfo.InvariantCulture));

                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.AccessToken,
                    tokenResult.AccessToken);

                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.RefreshToken,
                    tokenResult.RefreshToken
                    );

                // we're signing in again with the new values
                await currentContext.SignInAsync("Cookies",
                    authenticateInfo.Principal, authenticateInfo.Properties);

                // return the new access token
                return tokenResult.AccessToken;

            }
            else
            {
                throw new Exception("Problem encountered while refreshing tokens.",tokenResult.Exception);
            }



        }
    }
}
