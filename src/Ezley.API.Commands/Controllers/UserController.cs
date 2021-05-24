using System;
using System.Threading.Tasks;
using Ezley.API.Commands.ViewModels;
using Ezley.Commands;
using Ezley.Commands.Auth0Users;
using Ezley.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApplicationServices.Auth0;
using ApplicationServices.Auth0.Models;

namespace Ezley.API.Commands.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController

    {
        private string _domain;
        private string _clientId;
        private string _clientSecret;
        private string _audience;
        private string _connection;

        private IConfiguration _configuration;
        private IAuth0ManagementApi _auth0ManagementApi;
        private IMediator _mediator;

        public UserController(
            IConfiguration configuration, IAuth0ManagementApi auth0ManagementApi,
            IMediator mediator)
        {
            _configuration = configuration;
            _auth0ManagementApi = auth0ManagementApi;
            _mediator = mediator;

            _domain = _configuration["Auth0Admin:Domain"];
            _clientId = _configuration["Auth0Admin:ClientId"];
            _clientSecret = _configuration["Auth0Admin:ClientSecret"];
            _audience = _configuration["Auth0Admin:Audience"];
            _connection = _configuration["Auth0Admin:Connection"];
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registerUser")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterUserViewModel registerUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(registerUserModel);
            }
            
            /*******************************************************/
            // Make sure email doesn't exist locally or at Auth0
            // Create User
            // Create ServiceProvider
            // Create Auth0 User
            /*******************************************************/
            var userInfo = GetUserInfo();
            var appUserId = (Guid)registerUserModel.Id;
            var consumerId = Guid.NewGuid();
            
            // Create Auth0 Login - at Auth0 - will throw ApplicationEx if email already exists at Auth0
            var createUserRequest = new CreateUserRequest(registerUserModel.EmailUserName,
                registerUserModel.Password, 
                _connection, new AppMetaData(appUserId.ToString(), consumerId.ToString()));
            
            var auth0UserResponse = await _auth0ManagementApi.CreateAuth0UserAccount(createUserRequest);
            var auth0Id = auth0UserResponse.user_id;
            
            // Record Auth0User registered
            var registerAuth0User =
                new RegisterAuth0User(userInfo, auth0Id, appUserId);
            await _mediator.Send(registerAuth0User);
            
            // Record UserRegistered
            var registerUser = new RegisterUser(
                userInfo, 
                appUserId, 
                auth0Id, 
                null,
                null,
                null,
                null,
                new Email(registerUserModel.EmailUserName));
            await _mediator.Send(registerUser);

            return Ok();
        }
    }
}