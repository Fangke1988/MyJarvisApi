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
            //https://wx.jcloud.com/gwtest/init/11065
            string getData = string.Format("?city={0}&cityid=111&citycode=101260301&appkey={1}",city, CommonConfig.CommonData.APIKEY.JD);
            string result = APIRequest.RequestByGet(CommonConfig.CommonData.APIURL.WeaTherURL+ getData);
            try {
                var obj = JsonConvertUtils.JsonToXml(result, "rooat", false);
                //var obj = JsonConvertUtils.JsonToObject<BusinessModeBase>(result);
                return "";
            }
            catch (Exception exp) {
                Console.WriteLine(exp.Message);
            }
            return "";
        }
    }
  
}
