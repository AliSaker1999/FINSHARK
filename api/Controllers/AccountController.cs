using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Account;
using api.Interfaces;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _SignInManager;

        private readonly ITokenService _tokenservice;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> SignInManager, ITokenService tokenservice)
        {
            _userManager = userManager;
            _tokenservice = tokenservice;
            _SignInManager = SignInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var AppUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var CreatedUser = await _userManager.CreateAsync(AppUser, registerDto.Password);
                if (CreatedUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = AppUser.UserName,
                                Email = AppUser.Email,
                                Token = _tokenservice.CreateToken(AppUser)

                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, CreatedUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invaled Username");
            var pass = await _SignInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!pass.Succeeded) return Unauthorized("Invaled Username Or Password");
            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenservice.CreateToken(user)
                }
            );

        }
        

        

      
    }
}