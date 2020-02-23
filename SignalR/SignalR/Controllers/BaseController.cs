using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.Contracts.DTOs;
using SignalR.Contracts.Enums;

namespace SignalR.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ActionResult GetApiResponseFromResultDto<T>(ResultDto<T> result)
        {
            switch (result.ResultStatus)
            {
                case ResultStatus.Ok:
                    return new OkObjectResult(result.Data);
                case ResultStatus.Error:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                case ResultStatus.NotFound:
                    return new NotFoundResult();
                case ResultStatus.ArgumentsInvalid:
                    return new BadRequestResult();
                case ResultStatus.NotAuthorized:
                    return new UnauthorizedResult();
                case ResultStatus.Forbidden:
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        protected ActionResult GetApiResponseFromResultDto(ResultDto result)
        {
            switch (result.ResultStatus)
            {
                case ResultStatus.Ok:
                    return new OkObjectResult(result);
                case ResultStatus.Error:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                case ResultStatus.NotFound:
                    return new NotFoundResult();
                case ResultStatus.ArgumentsInvalid:
                    return new BadRequestResult();
                case ResultStatus.NotAuthorized:
                    return new UnauthorizedResult();
                case ResultStatus.Forbidden:
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
