using AutoMapper;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using BackupSystem.DTO.BackUpHistoryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackupSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackUpHistoryController : ControllerBase
    {
        private readonly IBackUpHistoryService _backUpHistoryService;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public BackUpHistoryController(IMapper mapper, IBackUpHistoryService backUphistoryService)
        {
            _response = new APIResponse();
            _mapper = mapper;
            _backUpHistoryService = backUphistoryService;
        }

        [HttpGet("GetBackUpHistory", Name = "GetBackUpHistory")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBackUpHistorys()
        {
            _response = await _backUpHistoryService.Get();
            return MapToActionResult(this, _response);
        }

        [HttpGet("GetBackUpHistoryByName", Name = "GetBackUpHistoryByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBackUpHistoryByName(string name)
        {
            _response = await _backUpHistoryService.Get(bch => bch.BackUpName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPost("AddBackUpHistory", Name = "AddBackUpHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBackUpHistory([FromForm] BackUpHistoryCreateDTO createDTO)
        {
            _response = await _backUpHistoryService.AddConfigurationHistoryRecord(createDTO);
            return MapToActionResult(this, _response);
        }

        [HttpDelete("DeleteBackUpHistoryByName", Name = "DeleteBackUpHistoryByName")]
        // TODO [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBackUpHistoryByName(string name)
        {
            _response = await _backUpHistoryService.Delete(bch => bch.BackUpName == name);
            return MapToActionResult(this, _response);
        }

        [HttpPut("UpdateBackUpHistory", Name = "UpdateBackUpHistory")]
        // [Authorize(Roles = DefaultRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBackUpHistory(string name, BackUpHistoryUpdateDTO updateDTO)
        {
            _response = await _backUpHistoryService.Update(updateDTO, bch => bch.BackUpName == name);
            return MapToActionResult(this, _response);
        }
    }
}
