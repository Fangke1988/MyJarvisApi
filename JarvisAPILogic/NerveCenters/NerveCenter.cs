using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarvisAPILogic.APIS;
using JarvisModels;

namespace JarvisAPILogic.NerveCenters
{
    /// <summary>
    /// 神经中枢类
    /// 用于接受原始数据，进行初级逻辑处理，用于处理 --功能方向接口分发
    /// </summary>
    class NerveCenter
    {
        static NerveCenter nervObj;
        internal static NerveCenter NervObj { get {
                if (nervObj == null)
                    nervObj = new NerveCenter();
                return nervObj;
            } }


        internal string ExcuteMsg(string data)
        {
            string result = string.Empty;
            var obj = MsgReaders.MsgReader.MsgObj.GetNotion(data);
            if (obj != null)
            {
                switch (obj.MsgAction)
                {
                    case ActionEnum.API:
                        result = APIDataExcute(obj as APIModel);
                        break;
                    case ActionEnum.Conversation:
                        break;
                    case ActionEnum.Task:
                        break;
                    case ActionEnum.None:
                        break;
                    default:
                        break;
                }
                return result;
            }
            else
            {
                return "无法识别该指令，正在等待转义\r\n----[Edvin Jarvis]";
            }
        }

        private string APIDataExcute(APIModel modelObj)
        {
            switch (modelObj.APIAction)
            {
                case APIEnum.weather:
                    return new WeatherAPI().GetWeatherAlarm(modelObj.KeyTargetWords);
                case APIEnum.filesManage:
                    var bingApi = new BingAPI();
                    string res = string.Empty;
                    if (modelObj.KeyActionWords.IndexOf("下载BING图片") != -1)
                    {
                        res = bingApi.DownTodayBg();
                    }
                    if (modelObj.KeyActionWords.IndexOf("BING背景") != -1)
                    {
                        res = bingApi.GetTodayBg();
                    }
                    
                    return res;
                default:
                    break;
            }
            return "";
        }


    }
}
