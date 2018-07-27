using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZxBackend.Models.Utils
{
    public class UtilResponse
    {
        public bool Success { get; private set; }
        public List<string> Errors { get; private set; }
        public object Data { get; private set; }

        public UtilResponse(bool success, object data)
        {
            Success = success;
            Errors = new List<string>();
            Data = data;
        }

        public UtilResponse(bool success, List<string> errors, object data)
        {
            Success = success;
            Errors = errors;
            Data = data;
        }
    }
}
