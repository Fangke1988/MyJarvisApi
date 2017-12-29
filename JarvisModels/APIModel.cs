using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JarvisModels
{
    public class APIModel: BaseModel
    {
        APIEnum apiAction = APIEnum.None;

        public APIEnum APIAction { get => apiAction; set => apiAction = value; }
    }
}
