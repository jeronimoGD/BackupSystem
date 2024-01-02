using AutoMapper;
using BackupSystem.Common.Constants;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.LoginDTO;
using BackupSystem.DTO.RegisterDTO;
using BackupSystem.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _response = new APIResponse();
            _mapper = mapper;
        }

        [HttpGet("GetUsers", Name = "GetUsers")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            _response = await _userService.Get();
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetUserById/{id:guid}", Name = "GetUserById")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _response = await _userService.Get(u => u.Id == id.ToString());
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetUserByName", Name = "GetUserByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByName(string name)
        {
            _response = await _userService.Get(u => u.UserName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPost("RegisterUser", Name = "RegisterUserEnd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterRequestDTO registerRequest)
        {
            _response = await _userService.Register(registerRequest);
            return MapToActionResult(this, _response);
        }

        [HttpPost("LoginUser", Name = "LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginUser([FromForm] LoginRequestDTO loginRequest)
        {
            _response = await _userService.LogIn(loginRequest);
            return MapToActionResult(this, _response);
        }


        [HttpDelete("DeleteUserById/{id:guid}", Name = "DeleteUserById")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            _response = await _userService.Delete(u => u.Id == id.ToString());
            return MapToActionResult(this, _response);
        }

        [HttpDelete("DeleteUserByName", Name = "DeleteUserByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserByName(string name)
        {
            _response = await _userService.Delete(u => u.UserName == name);
            return MapToActionResult(this, _response);
        }

        [HttpDelete("DeleteLogedUser", Name = "DeleteLogedUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLogedUser()
        {

            var authorizedUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _response = await _userService.Delete(u => u.Id == authorizedUserId.ToString());

            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateUserById/{id:guid}", Name = "UpdateUserById")]
        [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserById(Guid id, ApplicationUserUpdateDTO updateDTO)
        {
           
            _response = await _userService.UpdateUser(updateDTO, u => u.Id == id.ToString());
           
            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateUserByName", Name = "UpdateUserByName")]
        // [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(string name, ApplicationUserUpdateDTO updateDTO)
        {
            _response = await _userService.UpdateUser(updateDTO, u => u.UserName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateLoggedUser", Name = "UpdateLoggedUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLoggedUser(ApplicationUserUpdateDTO updateDTO)
        {
            var authorizedUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _response = await _userService.UpdateUser(updateDTO, u => u.Id == authorizedUserId.ToString());

            return MapToActionResult(this, _response);
        }
    }
}
