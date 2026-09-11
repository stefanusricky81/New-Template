using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Status
    {
        public class StatusCode
        {
            public int code { get; set; }
            public string msg { get; set; }
        }
        public static StatusCode statuscode (int errorcode)
        {
            var modelstatus = new StatusCode();
            switch (errorcode)
            {
                case 402:
                    modelstatus.code = 402;
                    modelstatus.msg = "API Key is missing";
                    break;
                case 401:
                    modelstatus.code = 401;
                    modelstatus.msg = "Invalid API Key";
                    break;
                case 405:
                    modelstatus.code = 405;
                    modelstatus.msg = "Login user and imei cannot be null";
                    break;
                case 408:
                    modelstatus.code = 408;
                    modelstatus.msg = "Login user is not exist";
                    break;
                case 200:
                    modelstatus.code = 200;
                    modelstatus.msg = "Ok";
                    break;
                case 201:
                    modelstatus.code = 201;
                    modelstatus.msg = "Please check your email to scan qrcode";
                    break;
                case 400:
                    modelstatus.code = 400;
                    modelstatus.msg = "Something went wrong";
                    break;
                case 403:
                    modelstatus.code = 403;
                    modelstatus.msg = "Login user unknown";
                    break;
                case 404:
                    modelstatus.code = 404;
                    modelstatus.msg = "Invalid token or imei";
                    break;
                case 407:
                    modelstatus.code = 407;
                    modelstatus.msg = "You already register with your mobile";
                    break;
                case 409:
                    modelstatus.code = 409;
                    modelstatus.msg = "Email is not exist";
                    break;
                default:
                    modelstatus.code = 200;
                    modelstatus.msg = "Ok";
                    break;
            }
            return modelstatus;
        }
    }
}