using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarvisModels;

namespace JarvisAPILogic.MsgReaders
{
    class MsgReader
    {
        static MsgReader msgObj;
        internal static MsgReader MsgObj
        {
            get
            {
                if (msgObj == null)
                    msgObj = new MsgReader();
                return msgObj;
            }
        }

        internal BaseModel GetNotion(string data)
        {
            BaseModel result= GetNotionObj(data);
            result.Msg = data;
            return result;
        }


        private BaseModel GetNotionObj(string data)
        {
            BaseModel result = null;
            if (data.IndexOf("天气") != -1)
            {
                result = new APIModel() { MsgAction = ActionEnum.API,APIAction= APIEnum.weather};
                result.KeyActionWords = "天气";
                result.KeyTargetWords = data.Replace("天气","");
               // result.= 
            }
            return result;
        }
    }
}
