using JarvisAPILogic.Common;
using JarvisModels.Models.APIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JarvisAPILogic.APIS
{
    public class BingAPI
    {
        private const  string BING_URL_HEAD = "http://s.cn.bing.net";
        internal string DownTodayBg()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string filename = AppDomain.CurrentDomain.BaseDirectory + "BINGBG\\" + date + "\\bg.jpg";
           if( !FileUtils.Exists(filename))
            {
                string result = APIRequest.RequestByGet(CommonConfig.CommonData.APIURL.BingImageURL+ "?format=js&idx=0&n=1");
                try
                {
                    var IMAGEPATH =  JsonConvertUtils.JsonToObject<BingImageObj>(result);
                    var URL = BING_URL_HEAD + IMAGEPATH.images[0].url;
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "BINGBG\\" + date);
                    SavePhotoFromUrl(filename, URL);
                }
                catch (Exception ex)
                { }
            }
            return "";
        }

        private bool SavePhotoFromUrl(string FileName, string Url)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    Value = SaveBinaryFile(response, FileName);
                }
            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return Value;
        }
        private bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];
            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }

        internal string GetTodayBg()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string filename = AppDomain.CurrentDomain.BaseDirectory + "BINGBG\\" + date + "\\bg.jpg";
            if (FileUtils.Exists(filename))
            {
                return filename;
            }
            else
            {
                return "";
            }
        }
    }
}
