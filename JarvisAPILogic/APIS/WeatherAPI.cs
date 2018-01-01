using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarvisAPILogic.Common;
using JarvisModels.Models;

namespace JarvisAPILogic.APIS
{
    class WeatherAPI
    {
        internal string GetWeatherAlarm(string city)
        {
            //https://wx.jcloud.com/market/datas/26/10610
            string getData = string.Format("?city={0}&appkey={1}",city, CommonConfig.CommonData.APIKEY.JD);
            string result = APIRequest.RequestByGet(CommonConfig.CommonData.APIURL.WeaTherURL+ getData);
            try {
                var obj = JsonConvertUtils.JsonToXml(result, "root", false).SelectSingleNode("root/result/HeWeather5");
                var daily = obj.SelectNodes("daily_forecast");
                var hourly = obj.SelectNodes("hourly_forecast");
               string msg=  string.Format("{0}今天:{1}\r\n未来6小时：{2}\r\n明天：{3}\r\n后天：{4}",
                    obj.SelectSingleNode("basic/city").InnerText,//地区
                    obj.SelectSingleNode("now/cond/txt").InnerText+" "+ obj.SelectSingleNode("now/wind/sc").InnerText+" 温度:"+ daily[0].SelectSingleNode("tmp/min").InnerText+ "°-" + daily[0].SelectSingleNode("tmp/max").InnerText+ "°",//当前天气
                    hourly[0].SelectSingleNode("date").InnerText.Remove(0,11)+" "+ hourly[0].SelectSingleNode("tmp").InnerText+"° "+ hourly[0].SelectSingleNode("cond/txt").InnerText+" "+ hourly[0].SelectSingleNode("wind/sc").InnerText+"-" +
                    hourly[1].SelectSingleNode("date").InnerText.Remove(0, 11) + " " + hourly[1].SelectSingleNode("tmp").InnerText + "° " + hourly[1].SelectSingleNode("cond/txt").InnerText + " " + hourly[1].SelectSingleNode("wind/sc").InnerText, //未来6小时天气
                    daily[1].SelectSingleNode("wind/sc").InnerText + " "+ daily[1].SelectSingleNode("cond/txt_n").InnerText + " 温度:" + daily[1].SelectSingleNode("tmp/min").InnerText + "°-" + daily[1].SelectSingleNode("tmp/max").InnerText + "°",
                      daily[2].SelectSingleNode("wind/sc").InnerText + " " + daily[2].SelectSingleNode("cond/txt_n").InnerText + " 温度:" + daily[2].SelectSingleNode("tmp/min").InnerText + "°-" + daily[2].SelectSingleNode("tmp/max").InnerText + "°");
                return msg;
            }
            catch (Exception exp) {
                Console.WriteLine(exp.Message);
            }
            return "";
        }
    }
  
}
