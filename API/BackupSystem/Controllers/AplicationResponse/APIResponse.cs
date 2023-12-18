using System.Net;

namespace BackupSystem.Controllers.AplicationResponse
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccesful { get; set; }
        public string? ErrorMessages { get; set; }
        public object Result { get; set; }

        public APIResponse()
        {
            StatusCode = HttpStatusCode.OK;
            IsSuccesful = true;
        }

        public APIResponse(HttpStatusCode statusCode, bool isSuccesful, string errorMessages, object result)
        {
            StatusCode = statusCode;
            IsSuccesful = isSuccesful;
            ErrorMessages = errorMessages;
            Result = result;
        }

        // More common success responses
        public static APIResponse Ok(object result)
        {
            return new APIResponse { StatusCode = HttpStatusCode.OK, IsSuccesful = true, Result = result };
        }

        public static APIResponse Created(object result)
        {
            return new APIResponse { StatusCode = HttpStatusCode.Created, IsSuccesful = true, Result = result };
        }
        public static APIResponse NoContent()
        {
            return new APIResponse { StatusCode = HttpStatusCode.NoContent, IsSuccesful = true };
        }


        // More common error responses
        public static APIResponse BadRequest(object result, string errorMessages)
        {
            return new APIResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccesful = false, Result = result, ErrorMessages = errorMessages };
        }

        public static APIResponse NotFound(string errorMessages)
        {
            return new APIResponse { StatusCode = HttpStatusCode.NotFound, IsSuccesful = false, Result = null, ErrorMessages = errorMessages };
        }

        public static APIResponse InternalServerError(string errorMessages)
        {
            return new APIResponse { StatusCode = HttpStatusCode.InternalServerError, IsSuccesful = false, Result = null, ErrorMessages = errorMessages };
        }
    }
}
