using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application.Commands
{
    public class BaseResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public bool isValid { get { return string.IsNullOrEmpty(ErrorCode); } }
    }
}
