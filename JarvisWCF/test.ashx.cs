using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JarvisAPILogic;

namespace JarvisWCF
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler
    {
        Tst ac = new Tst();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(ac.GetTstStr());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}