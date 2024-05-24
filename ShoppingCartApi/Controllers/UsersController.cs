using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected ResponseDto _response;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new ResponseDto();
        }


        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserDto user)
        {
            var res = await _userRepository.Register(
                    new User
                    {
                        UserName = user.UserName
                    }, user.Password);

            if (res == "exist")
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "User already exist";
                return BadRequest(_response);
            }

            if (res == "error")
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error creating user";
                return BadRequest(_response);
            }

            _response.DisplayMessage = "User created successfuly!";
            //_response.Result = res;
            JwTPackage jtp = new JwTPackage();
            jtp.UserName = user.UserName;
            jtp.Token = res;
            _response.Result = jtp;


            return Ok(_response);
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserDto user)
        {
            var res = await _userRepository.Login(user.UserName, user.Password);

            if (res == "nouser")
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "User does not exist";
                return BadRequest(_response);
            }
            if (res == "wrongpassword")
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Password incorrect";
                return BadRequest(_response);
            }

            //_response.Result = res;
            JwTPackage jtp = new JwTPackage();
            jtp.UserName = user.UserName;
            jtp.Token = res;
            _response.Result = jtp;

            _response.DisplayMessage = "User connected";
            return Ok(_response);
        }

    }

    public class JwTPackage
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}

