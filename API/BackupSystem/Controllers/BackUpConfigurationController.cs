using AutoMapper;
using BackupSystem.Common.Hubs;
using BackupSystem.Common.Interfaces.Hubs;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

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
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
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
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBackUpConfiguration([FromForm] BackUpConfigurationCreateDTO createDTO)
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
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBackUpConfigurationByName(string name)
        {
            BackUpConfiguration conf = await _backUpConfigurationService.GetSingle(a => a.ConfigurationName == name);
            _response = await _backUpConfigurationService.Delete(conf);

            if (_response.IsSuccesful)
            {
                _agentConfigurationHubService.NotifyConfigurationDeleted(conf.AgentId, conf.ConfigurationName);
            }
            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateBackUpConfiguration", Name = "UpdateBackUpConfiguration")]
        // [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBackUpConfiguration(string name, BackUpConfigurationUpdateDTO updateDTO)
        {
            _response = await _backUpConfigurationService.Update(updateDTO, u => u.ConfigurationName == name);
            return MapToActionResult(this, _response);
        }
    }
}
