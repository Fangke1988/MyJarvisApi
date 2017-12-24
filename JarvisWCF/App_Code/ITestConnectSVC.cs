using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ITestConnectSVC”。
[ServiceContract]
public interface ITestConnectSVC
{
    [OperationContract]
    void DoWork();

    [OperationContract]
    string GetTestData();

    [OperationContract]
    void InitData();

    [OperationContract]
    string GetData(string name);
    ///// <summary>
    ///// GetJsonResult
    ///// </summary>
    ///// <param name="name"></param>
    ///// <param name="address"></param>
    ///// <param name="phone"></param>
    ///// <remarks>
    ///// 为实现Json序列化，添加属性
    ///// [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    ///// </remarks>
    ///// <returns></returns>
    //[OperationContract]
    //[WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    //JsonResult GetJsonResult(string name, string address, string phone);
}
