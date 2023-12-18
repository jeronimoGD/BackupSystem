using BackupSystem.Controllers.AplicationResponse;
using Microsoft.AspNetCore.Mvc;

namespace BackupSystem.Controllers
{
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected ActionResult MapToActionResult(ControllerBase controller, APIResponse response)
        {
            return response.Result == null && response.ErrorMessages == null? controller.StatusCode((int)response.StatusCode) : controller.StatusCode((int)response.StatusCode, response);
        }
    }
}


