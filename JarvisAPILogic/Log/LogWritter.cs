using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JarvisAPILogic
{
    public static class LogWritter
    {
        private const int logMaxSize = 1; //单个文件最大大小,单位为M
        private static readonly object m_Lock = new object();
        private static string _currentPath;
        /// <summary>
        /// 存储异常(其它)日志
        /// </summary>
        /// <param name="type">异常、其它信息</param>
        /// <param name="content">具体信息</param>
        /// <param name="describe">描述</param>
        /// <param name="interfaceName">接口名</param>
        public static void Write(LogType type, object content, string describe = "", string interfaceName = "")
        {
            lock (m_Lock)
            {
                string dir = CURRENT_PATH;// + "log\\";
                if (type == LogType.Err)
                {
                    dir += "Error";
                }

                WriteInternal(type, dir, content, describe, interfaceName);
            }
        }
        /// <summary>
        /// 当前应用程序根目录
        /// </summary>
        public static string CURRENT_PATH
        {
            get
            {
                if (_currentPath == null)
                {
                    _currentPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
                }
                return _currentPath;
            }
        }
        /// <summary>
        /// 存储报文日志
        /// </summary>
        /// <param name="type">请求、响应</param>
        /// <param name="inferface">接口名称</param>
        /// <param name="content">Json内容</param>
        /// <param name="timeDifference">时差值</param>
        /// <param name="fid">请求标识符</param>
        public static void WriteJsonLog(string inferface, string content, string timeDifference, string fid)
        {
            lock (m_Lock)
            {
                StreamWriter sw = null;
                string dir = CURRENT_PATH + "\\log\\Json";
                try
                {
                    Directory.CreateDirectory(dir); //创建文件夹
                    string filename = dir + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log"; //文件名

                    //删除旧日志
                    DeleteOldLog(CURRENT_PATH + "log");

                    if (File.Exists(filename))
                    {
                        var file = new FileInfo(filename);
                        if (file.Length > 1024 * 1024 * logMaxSize)
                        {
                            BackupLogFile(dir, FileUtils.GetFileNameNoneExt(filename));
                        }
                    }
                    sw = new StreamWriter(filename, true);
                    string title = "[时间]:" +
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "[接口]:" +
                        inferface;
                    if (timeDifference != "")
                    {
                        title += "\r\n[" + timeDifference + "]";
                    }
                    else
                    {
                        title += ":\r\n";
                    }
                    sw.WriteLine(title); //标题
                    sw.WriteLine("[内容]:\r\n" + content);
                    sw.WriteLine("------------------------------↑Log↑------------------------------");
                }
                catch (Exception)
                {
                    //写日志发生错误无需通知处理
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }
                }
            }
        }

      

        /// <summary>
        /// 日志内部方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="dir"></param>
        /// <param name="describe"></param>
        /// <param name="interfaceName"></param>
        private static void WriteInternal(LogType type,string dir, object content, string describe = "", string interfaceName = "")
        {
            StreamWriter sw = null;
            string filename = dir + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log"; //文件名         
            try
            {
                Directory.CreateDirectory(dir); //创建文件夹
                DeleteOldLog(CURRENT_PATH + "log"); //删除旧日志

                #region 备份日志

                if (File.Exists(filename))
                {
                    FileInfo file = new FileInfo(filename);
                    // bit k  M
                    if (file.Length > 1024 * 1024 * logMaxSize)
                    {
                        //FileUtil.Copy(filename,dir+"\\" FileUtils.GetFileNameNoneExt(filename)+""
                        BackupLogFile(dir, FileUtils.GetFileNameNoneExt(filename));
                    }
                }

                #endregion

                sw = new StreamWriter(filename, true);
                string title ="[接口]:" + interfaceName + "\r\n" + "[时间]:" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sw.WriteLine(title); //标题
                if (describe != "")
                {
                    sw.WriteLine("[描述]:" + describe + "->");
                }
                sw.WriteLine("[内容]:" + content);
                sw.WriteLine("------------------------------------------------------------");
            }
            catch
            {
                //写日志发生错误无需通知处理
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }

        }


        #region //日志文件操作

        /// <summary>
        /// 删除旧日志
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteOldLog(string dir)
        {
            long lOldDate = Convert.ToInt64(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));
            string[] sChildDirs = { dir + "\\Json", dir + "\\Error", dir + "\\OtherInfo" };
            foreach (string childDir in sChildDirs)
            {
                try
                {
                    if (Directory.Exists(childDir))
                    {
                        List<string> arrfile = new List<string>();

                        string[] files = Directory.GetFiles(childDir);
                        foreach (var file in files)
                        {
                            string filenametemp = FileUtils.GetFileNameNoneExt(file);
                            if (filenametemp.Length >= 8)
                            //文件名大于8位，说明是已备份文件,再判断是否为15天前的数据
                            {
                                long fileNameHash;
                                if (long.TryParse(filenametemp, out fileNameHash))
                                {
                                    if (fileNameHash < lOldDate)
                                    {
                                        arrfile.Add(file);
                                    }
                                }

                            }
                        }
                        foreach (string str in arrfile)
                        {
                            FileUtils.DeleteFile(str);
                        }
                    }
                }
                catch (Exception)
                {
                    //无需处理    
                }
            }
        }

        /// <summary>
        /// 备份日志文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filename"></param>
        private static void BackupLogFile(string dir, string filename)
        {
            string[] files = Directory.GetFiles(dir);
            int iCount = 0;

            foreach (string file in files)
            {
                string filenametemp = FileUtils.GetFileNameNoneExt(file);
                if (null != filename && filenametemp.IndexOf(filename) >= 0) //判断是否为当天日志文件
                {
                    if (filenametemp.Length > 8) //文件名大于8位，说明是已备份文件
                    {
                        int itemp = Convert.ToInt16(filenametemp.Substring(9, filenametemp.Length - 8 - 2));
                        //获取备份编号
                        if (itemp > iCount)
                        {
                            iCount = itemp;
                        }
                    }
                }
            }
            try
            {
                //复制文件
                FileUtils.Copy(dir + "\\" + filename + ".log", dir + "\\" + filename + "(" + (iCount + 1) + ").log");
                //删除原文件
                File.Delete(dir + "\\" + filename + ".log");
            }
            catch
            {
                //无需处理
            }
        }
        #endregion
    }

    public enum LogType
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        Err,
        /// <summary>
        /// 其它普通信息
        /// </summary>
        OtherInfo,
        /// <summary>
        /// xmpp信息
        /// </summary>
        Xmpp,
        /// <summary>
        /// 调试信息，只有在进入调试模式下才会开启此日志的输出。
        /// </summary>
        Debug,
        /// <summary>
        /// Js信息
        /// </summary>
        Js,
    }
}
