using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JarvisAPILogic
{
    static class APIRequest
    {
        public static string RequestByPost(string url,string postData)
        {
            //string postData = "id=1&user=hzt";

            WebRequest request = WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("gb2312"));
            //MessageBox.Show(sr.ReadToEnd());

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return "";
        }
        public static string RequestByGet(string url)
        {
            //string url = "http://127.0.0.1/page.aspx?id=1&user=hzt";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("UTF-8"));
            //MessageBox.Show(sr.ReadToEnd());
            string res= sr.ReadToEnd();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return res;
        }
    }
}
