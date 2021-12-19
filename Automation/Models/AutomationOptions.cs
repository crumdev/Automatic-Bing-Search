using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Automation.Models
{    
    public class AutomationOptions {
        public List<BingUser> BingUsers { get; set; }
        public string SignInUrl { get; set; }
        public int SleepTime { get; set; }
    }
}