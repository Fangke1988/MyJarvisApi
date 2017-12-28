using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarvisAPILogic.APIS;
using JarvisAPILogic.JarvisEnum;

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
            if (MsgReaders.MsgReader.MsgObj.GetNotion(data) == ActionEnum.API)
            {
                return new WeatherAPI().GetWeatherAlarm(data);
            }
            return "";
        }

    }
}
