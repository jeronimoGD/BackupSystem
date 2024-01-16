using AutoMapper;
using BackupSystem.Common.Constants;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Interfaces.SignalR;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackUpConfigurationController : ControllerBase
    {
        private readonly IBackUpConfigurationService _backUpConfigurationService;
        private readonly IAgentsService _agentService;
        private APIResponse _response;
        private readonly IMapper _mapper;
        private IAgentConfigurationHubService _agentConfigurationHubService;

        public BackUpConfigurationController(IMapper mapper, IBackUpConfigurationService backUpConfigurationService, IAgentsService agentService, IAgentConfigurationHubService agentConfigurationHubService)
        {
            _response = new APIResponse();
            _mapper = mapper;
            _backUpConfigurationService = backUpConfigurationService;
            _agentService = agentService;
            _agentConfigurationHubService = agentConfigurationHubService;
        }

        [HttpGet("GetBackUpConfigurations", Name = "GetBackUpConfigurations")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBackUpConfigurations()
        {
            _response = await _backUpConfigurationService.Get();
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetBackUpConfiguration", Name = "GetBackUpConfiguration")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBackUpConfiguration(string name)
        {
            _response = await _backUpConfigurationService.Get(a => a.ConfigurationName == name, true, 1);
            return MapToActionResult(this, _response);
        }

        [HttpPost("AddBackUpConfiguration", Name = "AddBackUpConfiguration")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBackUpConfiguration([FromBody] BackUpConfigurationCreateDTO createDTO)
        {
            _response = await _backUpConfigurationService.AddBackUpConfiguration(createDTO);
            
            if (_response.IsSuccesful)
            {
                Agent agent = await _agentService.GetSingle(a => a.AgentKey == createDTO.AgentId);
                _agentConfigurationHubService.NotifyNewConfiguration(agent.AgentKey, createDTO.ConfigurationName);
            }

            return MapToActionResult(this, _response);
        }

        [HttpDelete("DeleteBackUpConfigurationByName", Name = "DeleteBackUpConfigurationByName")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBackUpConfigurationByName(string name)
        {
            _response = await _backUpConfigurationService.Delete(c => c.ConfigurationName == name);

            if (_response.IsSuccesful)
            {
                BackUpConfiguration conf = (BackUpConfiguration)_response.Result;
                _agentConfigurationHubService.NotifyConfigurationDeleted(conf.AgentId, conf.ConfigurationName);
            }

            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateBackUpConfiguration", Name = "UpdateBackUpConfiguration")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBackUpConfiguration(string name, BackUpConfigurationUpdateDTO updateDTO)
        {
            _response = await _backUpConfigurationService.Update(updateDTO, u => u.ConfigurationName == name);

            if (_response.IsSuccesful)
            {
                BackUpConfiguration conf = (BackUpConfiguration)_response.Result;
                _agentConfigurationHubService.NotifyConfigurationUpdated(conf.AgentId, conf.ConfigurationName);
            }

            return MapToActionResult(this, _response);
        }
    }
}
