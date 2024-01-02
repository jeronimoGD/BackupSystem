using AutoMapper;
using Azure;
using BackupSystem.Common.Constants;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.AgentDTOs;
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
    public class AgentsController : ControllerBase
    {
        private readonly IAgentsService _agentService;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public AgentsController(IMapper mapper, IAgentsService agentService)
        {
            _response = new APIResponse();
            _mapper = mapper;
            _agentService = agentService;
        }

        [HttpGet("GetAgents", Name = "GetAgents")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgents()
        {
            _response = await _agentService.Get(null, true, 0, a => a.BackUpConfigurations);
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetOnlineAgents", Name = "GetOnlineAgents")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOnlineAgents()
        {
            _response = await _agentService.Get(a => a.IsOnline == true, true, 0, a => a.BackUpConfigurations);
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetAgentByName", Name = "GetAgentByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentByName(string name)
        {
            _response = await _agentService.Get(a => a.AgentName == name, true, 0, a => a.BackUpConfigurations);
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetBackUpConfigurationByAgent", Name = "GetBackUpConfigurationByAgent")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBackUpConfigurationByAgent(Guid connectionKey)
        {
            _response = await _agentService.GetAgentBackUpConfiguration(connectionKey);
            return MapToActionResult(this, _response);
        }

        [HttpPost("AddAgent", Name = "AddAgent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAgent([FromForm] AgentCreateDTO createDTO)
        {
            _response = await _agentService.AddAgent(createDTO);
            return MapToActionResult(this, _response);
        }

        [HttpDelete("DeleteAgentByName", Name = "DeleteAgentByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAgentByName(string name)
        {
            _response = await _agentService.Delete(a => a.AgentName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateAgent", Name = "UpdateAgent")]
        // [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAgent(string name, AgentUpdateDTO updateDTO)
        {
            _response = await _agentService.Update(updateDTO, u => u.AgentName == name);
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetAuthorizationToConnect", Name = "GetAuthorizationToConnect")]
        // [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorizationToConnect(Guid connectionKey)
        {
            _response = await _agentService.IsAuthorized(connectionKey);
            return MapToActionResult(this, _response);
        }
    }
}
