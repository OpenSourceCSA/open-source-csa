using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApplicationServices.Auth0.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApplicationServices.Auth0
{
    using Auth0;

    public interface IAuth0ManagementApi
    {
        Task<CreateAuth0UserResponse> CreateAuth0UserAccount(CreateUserRequest user);
    }

    public class Auth0Auth0ManagementApi : IAuth0ManagementApi
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;
        private string _domain;
        private string _clientId;
        private string _clientSecret;
        private string _audience;
        private string _connection;
        private string _tokenUri;
        public Auth0Auth0ManagementApi(IConfiguration configuration, HttpClient httpClient)
        { 
            _configuration = configuration;
            _httpClient = httpClient;
            
            _domain = _configuration["Auth0:Domain"];
            _clientId = _configuration["Auth0:ClientId"];
            _clientSecret = _configuration["Auth0:ClientSecret"];
            _audience = _configuration["Auth0:Audience"];
            _connection = _configuration["Auth0:Connection"];
            _tokenUri = _configuration["Auth0:Url_MachineToken"];
        }

        /// <summary>
        /// Get a token for the app to make calls to Auth0 management api.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private async Task<TokenResponse> GetAuth0ManagementApiToken(
            string domain, string clientId, string clientSecret, string audience)
        {
            var tokenRequest = new Auth0ManangementApiTokenRequest(clientId, clientSecret, audience);
            var json = JsonConvert.SerializeObject(tokenRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_tokenUri, data);
            string respContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(respContent);
            return tokenResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Auth0 UserId</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<CreateAuth0UserResponse> CreateAuth0UserAccount(CreateUserRequest user)
        {
            var uri = $"https://{_domain}/api/v2/users";

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var token = await GetAuth0ManagementApiToken(_domain, _clientId, _clientSecret, _audience);

            // Make sure user doesn't already exist at Auth0
            if (await UserExistsForConnection(user.email, _connection, token))
            {
                const string emailExists = "Email address is already in use.";
                throw new ApplicationException(emailExists);
            }
            
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                token.access_token);
            var response = await _httpClient.PostAsync(uri, data);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new ApplicationException("Failed to create user.");
            }

            string respContent = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<CreateAuth0UserResponse>(respContent);

            return respObj;
        }
        private async Task<bool> UserExistsForConnection(string email, string connection, TokenResponse token)
        {
            //var domain = _configuration["Auth0:Domain"];
             
            var method = HttpMethod.Get;
            var uri = new 
                Uri($"https://{_domain}/api/v2/users-by-email?" +
                    $"fields=user_id%2Cidentities&" +
                    $"include_fields=true&email={WebUtility.UrlEncode(email.ToLower())}");
            var authHeader = new AuthenticationHeaderValue("Bearer",
                token.access_token);
            
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = method,
                Headers = { Authorization = authHeader}
            };

            var response = await _httpClient.SendAsync(request);
            
            // handle responses
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new 
                    ApplicationException("Error checking for existing user with same email." +
                                         " Make sure email is valid.");
            }

            string respContent = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<GetUsersByEmailResponse>>(respContent);
            var userExists = users
                .Any(x => x.identities.Any(x => x.connection == connection));
            return userExists;
            
        }
         
    }
}