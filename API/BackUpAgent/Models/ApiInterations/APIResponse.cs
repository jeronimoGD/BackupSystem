using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Models.ApiInteractions
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
            ErrorMessages = string.Empty;
        }

    }
}
