using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BackUpAgent.Models.ApiInteractions.APIHttpActions;

namespace BackUpAgent.Models.ApiInteractions
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET;
        public string Url { get; set; }
        public object Datos { get; set; }
        public string Token { get; set; }
    }
}
