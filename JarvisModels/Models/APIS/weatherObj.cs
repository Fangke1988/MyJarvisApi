using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels.Models
{
    public class weatherObj : APIResult
    {
        public string date = string.Empty;
        public string week = string.Empty;
        public string city = string.Empty;
        public string weather = string.Empty;
        public string winddirect = string.Empty;
        public string windspeed = string.Empty;
        public string updatetime = string.Empty;
    }
}
