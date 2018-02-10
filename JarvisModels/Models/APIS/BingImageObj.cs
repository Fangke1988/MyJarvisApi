using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels.Models.APIS
{
    public class BingImageObj: APIResult
    {
        public List<img> images = new List<img>();
    }
    public class img
    {
        public string url;
        public string copyright;
    }
}
