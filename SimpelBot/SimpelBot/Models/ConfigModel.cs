using System;
using System.Collections.Generic;
using System.Text;

namespace SimpelBot.Models
{
    public class ConfigModel
    {
        public string ClientSecret { get; set; }

        public string ClientName { get; set; }
        
        public char ClientPrefix { get; set; }
    }
}
