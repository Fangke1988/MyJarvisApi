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
            if (result != null)
                result.Msg = data;
            else
                return null;
            return result;
        }


        private BaseModel GetNotionObj(string data)
        {
            BaseModel result = null;

            //var OBJ=MsgNLP.Lexer(data);
            //var OBJ2 = MsgNLP.DepParser(data);
            //var OBJ3 = MsgNLP.SentimentClassify(data);

            if (data.IndexOf("天气") != -1)
            {
                result = new APIModel() { MsgAction = ActionEnum.API,APIAction= APIEnum.weather};
                result.KeyActionWords = "天气";
                result.KeyTargetWords = data.Replace("天气","");
               // result.= 
            }
            if (data.IndexOf("SAVE-IMG") != -1)
            {
                result = new APIModel() { MsgAction = ActionEnum.API, APIAction = APIEnum.filesManage };
                result.KeyActionWords = "下载BING图片";
                result.KeyTargetWords = data.Replace("SAVE-IMG", "");
            }
            if (data.ToUpper().IndexOf("BING") != -1)
            {
                result = new APIModel() { MsgAction = ActionEnum.API, APIAction = APIEnum.filesManage };
                result.KeyActionWords = "BING背景";
                result.KeyTargetWords = data.Replace("BING", "");
            }
            return result;
        }
    }
}
