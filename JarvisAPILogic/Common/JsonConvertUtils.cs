using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;

namespace JarvisAPILogic.Common
{
    /// <summary>
    /// json字符串和对象之间的转换
    /// </summary>
    public static class JsonConvertUtils
    {
        #region json
        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <returns></returns>
        public static string ObjToJson(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }

        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            // Description : 添加异常信息记录，帮助调试
            // Modified By : 彭畅
            // Update Time : 2016/2/15 8:42:25
            json = json.Replace("vassalage_org\":null", "vassalage_org\":0");
            T deserializedProduct = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                Error = (s, e) =>
                {
                    var message = string.Format("{0}，Member = {1}， Path = {2}", e.ErrorContext.Error.Message, e.ErrorContext.Member, e.ErrorContext.Path);
                    Debug.WriteLine(message);
                    //Log.Write(Log.LogType.Err, message);
                }
            });
            return deserializedProduct;
        }

        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="type">序列化后的类型</param>
        /// <returns></returns>
        public static object JsonToObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// json串转化为dictionary
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SelectDictionary(string json)
        {
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var jsonObject = (Dictionary<string, string>)ser.ReadObject(ms);
            return jsonObject;

        }
        /// <summary>
        /// json串转化为dictionary
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SelectDictionary(object jsonobj)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            Type t = jsonobj.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, (string)mi.Invoke(jsonobj, new object[] { }));
                }
            }

            return map;



        }


        /// <summary>
        /// 深度拷贝对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>拷贝结果</returns>
        public static T CopyObject<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            T deserializedProduct = JsonConvert.DeserializeObject<T>(json);
            return deserializedProduct;
        }

        /// <summary>
        /// JSON转xlm
        /// </summary>
        /// <param name="json">json串</param>
        /// <param name="RootElementName">xml中根节点名称</param>
        /// <param name="writeArrayAttribute"></param>
        /// <returns></returns>
        public static XmlDocument JsonToXml(string json, string RootElementName, bool writeArrayAttribute)
        {
            //Modified by heke 2012-8-7 ->修改用XmlDocment.LoadXml方式加载的方式，经测试使用xmlNode.OwnerDocument所获取对象会有问题
            var xmlNode = JsonConvert.DeserializeXmlNode(json, RootElementName);
            //XmlDocument xmldoc = xmlNode.OwnerDocument;
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlNode.InnerXml);
            return xmldoc;
        }

        /// <summary>
        /// 将XML文件转换成json串
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <returns></returns>
        public static string XmlToJson(XmlDocument doc)
        {
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }


        /// <summary>
        /// Json文件转为Json对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static T JsonFileToObject<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default(T);

            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = File.OpenRead(filePath);
                sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = sr.ReadToEnd();

                return JsonToObject<T>(strLine);
            }
            catch (Exception e)
            {

            }
            finally
            {
                try
                {
                    if (fs != null) fs.Close();
                    if (sr != null) sr.Close();
                }
                catch (IOException e)
                {

                }
            }

            return default(T);
        }
        #endregion
    }
}
