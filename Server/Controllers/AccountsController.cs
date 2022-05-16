using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MultipleBlazorApps.Shared.DTOs;


namespace MultipleBlazorApps.Server.Controllers
{
    [Route("/FirstApp/[controller]")]
    [Route("/SecondApp/[controller]")]
    [ApiController]
    public class AccountsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly ISendEmailService _sendEmailService;
        private readonly IConfiguration _configuration;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
        //    ISendEmailService sendEmailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        //    _sendEmailService = sendEmailService;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfoDTO model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfoDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(userInfo);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }



        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renew()
        {
            var userInfo = new UserInfoDTO()
            {
                Email = HttpContext.User.Identity.Name
            };

            return await BuildToken(userInfo);
        }

        private async Task<UserToken> BuildToken(UserInfoDTO userinfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userinfo.Email),
                new Claim(ClaimTypes.Email, userinfo.Email),
                new Claim("myvalue", "whatever I want")
            };

            var identityUser = await _userManager.FindByEmailAsync(userinfo.Email);
            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                  issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        //[HttpPost("Forgot")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        //{
        //    //this is to check if the user's email address they have provided is a valid user email. In the event that it is not, the system does nothing 
        //    // and then just redirects them to a confirmation page - we do not alert them to an incorrect email address for security purposes.
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return BadRequest("Modelstate not valid");
        //        //return RedirectToAction("Account/Resetpassword");
        //    }
        //    else
        //    {
        //        var userInfo = new UserInfoDTO()
        //        {
        //            Email = user.Email
        //        };
        //        var resettoken = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackurl = model.BaseURL + $"account/resetpassword?userId={user.Id}&code={resettoken}";
        //        await _sendEmailService.SendResetPasswordEmail(model, callbackurl);
        //        return Ok();
        //    }
        //}

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(model.Token);
            string Token = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes).Replace(" ", "+");

            //this is to check if the user's email address they have provided is a valid user email. In the event that it is not, the system does nothing 
            // and then just redirects them to a confirmation page - we do not alert them to an incorrect email address for security purposes.
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("user is null");

            }
            else
            {
                var result = await _userManager.ResetPasswordAsync(user, Token, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Reset password failed");
                }
            }
        }





    }
}
