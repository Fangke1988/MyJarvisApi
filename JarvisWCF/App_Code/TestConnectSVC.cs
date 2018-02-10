using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using JarvisAPILogic;
using System.Timers;
using System.IO;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“TestConnectSVC”。
public class TestConnectSVC : ITestConnectSVC
{
    Tst ac = new Tst();
    public void DoWork()
    {
     
    }

    public string GetData(string name)
    {
        return ac.GetData(name);
    }

    public string GetTestData()
    {
        var ccas = ac.GetTstStr() + "";
        return string.Format("ac: {0}", ccas);
    }

    public void InitData()
    {
        ac.InitData();
    }
}
