using AutoMapper;
using BackupSystem.Common.Hubs;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackUpConfigurationController : ControllerBase
    {
        private readonly IBackUpConfigurationService _backUpConfigurationService;
        private APIResponse _response;
        private readonly IMapper _mapper;
        private IHubContext<AgentConfigurationHub> _hubContex;
        public BackUpConfigurationController(IMapper mapper, IBackUpConfigurationService backUpConfigurationService, IHubContext<AgentConfigurationHub> hubContex)
        {
            _response = new APIResponse();
            _mapper = mapper;
            _backUpConfigurationService = backUpConfigurationService;
            _hubContex = hubContex;
        }

        [HttpGet("GetBackUpConfigurations", Name = "GetBackUpConfigurations")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getBackUpConfigurations()
        {
            _response = await _backUpConfigurationService.GetAll();

            _hubContex.Clients.All.SendAsync("ReceiveConfiguration", "Here you have your configuration");
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
            _response = await _backUpConfigurationService.Get(a => a.ConfigurationName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPost("AddBackUpConfiguration", Name = "AddBackUpConfiguration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBackUpConfiguration([FromForm] BackUpConfigurationCreateDTO createDTO)
        {
            _response = await _backUpConfigurationService.Add(createDTO, bc => bc.ConfigurationName == createDTO.ConfigurationName);
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
            _response = await _backUpConfigurationService.Delete(a => a.ConfigurationName == name);
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
