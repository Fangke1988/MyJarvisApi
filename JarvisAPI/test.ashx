<%@ WebHandler Language="C#" Class="test" %>

using System;
using System.Web;

public class test : IHttpHandler {
      JarvisAPILogic.Tst acc = new JarvisAPILogic.Tst();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write(acc.GetTstStr());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}