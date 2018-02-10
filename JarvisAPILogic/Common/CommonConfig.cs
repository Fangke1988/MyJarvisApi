using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JarvisAPILogic
{

    internal class CommonConfig
    {
        XElement xDoc;
        public CommonConfig()
        {
            xDoc = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "Config/DataConfig.xml");
        }
        private static CommonConfig commonData;

        public static CommonConfig CommonData
        {
            get
            {
                if (commonData == null)
                    commonData = new CommonConfig();
                return commonData;
            }
        }

        #region
        /// <summary>
        /// API地址对象
        /// </summary>
        public APIDataURL APIURL
        {
            get
            {
                if (aPIURL == null)
                {
                    aPIURL = new APIDataURL();
                    aPIURL.NewsSearchURL = GetElementString(xDoc.Element("APIUrls").Element("NewsSearch"));
                    aPIURL.NewsURL = GetElementString(xDoc.Element("APIUrls").Element("News"));
                    aPIURL.WeaTherURL = GetElementString(xDoc.Element("APIUrls").Element("Weather"));
                    aPIURL.BingImageURL = GetElementString(xDoc.Element("APIUrls").Element("BingImage"));
                }
                return aPIURL;
            }
        }

        /// <summary>
        /// API KEY
        /// </summary>
        public APIKeys APIKEY
        {
            get
            {
                if (aPIKEY == null)
                {
                    aPIKEY = new APIKeys();
                    aPIKEY.JD = GetElementString(xDoc.Element("APIKey").Element("JD"));
                    aPIKEY.BaiDuAPIKey = GetElementString(xDoc.Element("APIKey").Element("BaiduAI").Element("APIKey"));
                    aPIKEY.BaiDuSecretKey = GetElementString(xDoc.Element("APIKey").Element("BaiduAI").Element("SecretKey"));
                }
                return aPIKEY;
            }
        }

        private APIDataURL aPIURL;

        private APIKeys aPIKEY;

        #endregion

        #region Method
        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private string GetElementString(XElement element)
        {
            if (element == null) return string.Empty;
            return element.Value;
        }
        #endregion
    }
    internal class APIKeys
    {
        public string JD = string.Empty;
        public string BaiDuAPIKey = string.Empty;
        public string BaiDuSecretKey = string.Empty;
    }
    internal class APIDataURL
    {
        public string WeaTherURL = string.Empty;
        public string NewsURL = string.Empty;
        public string NewsSearchURL = string.Empty;
        public string BingImageURL = string.Empty;
    }
}
