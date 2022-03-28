using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.User
{
    public class CreateUserRequest
    {
        public string Firebase_uid { get; set; }
        public string Firebase_id_token { get; set; }
        public string Email { get; set; }
        public bool Email_verified { get; set; } 
        public bool Is_New_user { get; set; }
        public string Signin_Method { get; set; }
        public long Firebase_Creation_Time { get; set; }
        public long Firebase_Last_Signin_Time { get; set; }
    }
}