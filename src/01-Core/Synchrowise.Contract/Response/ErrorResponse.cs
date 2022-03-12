using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Response
{
    public class ErrorResponse
    {
        public List<string> Errors  { get; set; } = new List<string> ();
        public bool IsShow { get; set; }
        public ErrorResponse()
        {
            Errors = new List<string>();
        }
        public ErrorResponse(string error, bool isShow)
        {
            Errors.Add(error);
            isShow= true;
        }
        public ErrorResponse(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}