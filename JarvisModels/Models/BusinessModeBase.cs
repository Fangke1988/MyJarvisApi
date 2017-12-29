using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels.Models
{
   public class BusinessModeBase
    {
        public string code = string.Empty;
        public string msg = string.Empty;
        public ApiResultStatus result = new ApiResultStatus();
    }
    public class ApiResultStatus
    {
        public string msg = string.Empty;
        public string status = string.Empty;
        public APIResult result;
    }
    public abstract class APIResult
    {
    }
}
