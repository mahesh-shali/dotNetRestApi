using System;
using System.Net;

namespace RestApi.Models
{
    public class LoginDetails
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string? IpAddress { get; set; }
        public string? Browser { get; set; }
        public string? BrowserVersion { get; set; }
        public string? OS { get; set; }
        public string? LogState { get; set; }  //register or login 
        public DateTime LoginTime { get; set; }
    }
}
