using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace JarvisAPILogic
{
    /// <summary>
    /// 文件处理帮助类
    /// </summary>
    public class FileUtils
    {
        #region file 方法

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsEmploy(string file)
        {
            const string spestr = "§•№☆★○●◎◇◆□℃‰€■△▲※→←↑↓〓¤°＃＆＠＼︿＿￣―♂♀";
            foreach (var ch in file) //如果文件名中带有音标，直接返回未占用
            {
                if (ch >= 127 && ch <= 255)
                    return false;
                //如果文件名带有特殊字符，直接返回未占用
                //甄立 EBAG-19295 2013-10-16
                if (spestr.IndexOf(ch) > 0)
                    return false;
            }
            if (File.Exists(file))
            {
                IntPtr vHandle = _lopen(file, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    return true;
                }

                CloseHandle(vHandle);
            }
            return false;
        }

        /// <summary>
        /// 判定文件是否可读
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsCanRead(string file)
        {
            if (string.IsNullOrEmpty(file))
                return false;
            if (!File.Exists(file))
                return false;
            bool canRead;
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                canRead = true;
            }
            catch (IOException ex)
            {
                canRead = false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return canRead; //true可读 
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            //判断文件是否存在
            if (File.Exists(filePath) && !IsEmploy(filePath))
            {
                try
                {
                    //删除文件
                    File.Delete(filePath);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirPath">指定目录</param>
        /// <returns></returns>
        public static void DeleteDir(string dirPath)
        {
            //判断文件是否存在
            if (Directory.Exists(dirPath))
            {
                try
                {
                    //删除文件
                    Directory.Delete(dirPath, true);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirPath">指定目录</param>
        /// <returns></returns>
        public static void DeleteWholeDir(string dirPath)
        {
            //判断文件是否存在
            if (Directory.Exists(dirPath))
            {
                var dir = new DirectoryInfo(dirPath);
                foreach (var file in dir.GetFiles())
                {
                    try
                    {
                        file.Delete(); //删除文件
                    }
                    catch (Exception ex)
                    {

                    }
                }
                foreach (var directoryInfo in dir.GetDirectories())
                {
                    DeleteWholeDir(directoryInfo.FullName);
                }
            }
        }


        /// <summary>
        /// 返回文件路径的后缀(保留".")
        /// </summary>
        /// <param name="path"></param>
        /// <returns>.png</returns>
        public static string GetFileExt(string path)
        {
            if (path != null && path.LastIndexOf('.') > 0)
            {
                return path.Substring(path.LastIndexOf('.'));
            }
            //throw new Exception("图片无扩展名：" + path);
            return null;

        }

        /// <summary>
        /// 返回文件路径的后缀（无"."）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExtNonePoint(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            int index = path.LastIndexOf(".", StringComparison.Ordinal);
            if (index >= 0)
            {
                return path.Substring(path.LastIndexOf('.') + 1);
            }
            return path;//防止传入的格式没有'.'
        }

        /// <summary>
        /// 返回文件路径的文件名（保留后缀）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            string filePath = path.Replace("/", "\\").Trim();
            return filePath.Substring(filePath.LastIndexOf('\\') + 1);
        }

        /// <summary>
        /// 返回文件路径的文件名（无后缀）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameNoneExt(string path)
        {
            string filename = GetFileName(path);
            int index = filename.LastIndexOf(".", StringComparison.Ordinal);
            if (index > 0)
            {
                return filename.Substring(0, index);
            }
            return filename;
        }

        /// <summary>
        /// 返回文件路径的路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFilePath(string filePath)
        {
            string path = filePath.Replace("/", "\\").Trim();
            return path.Substring(0, path.LastIndexOf('\\') + 1);
        }

        #endregion

        /// <summary>
        /// 返回带单位的文件大小
        /// </summary>
        /// <param name="size">字节</param>
        /// <returns></returns>
        public static string FileSize(long size)
        {
            if (size < 1024)
            {
                return size.ToString(CultureInfo.InvariantCulture) + "B";
            }
            if (size < 1024 * 1024 && size >= 1024)
            {
                return (size / 1024).ToString(CultureInfo.InvariantCulture) + "KB";
            }
            if (size < 1024 * 1024 * 1024 && size >= 1024 * 1024)
            {
                return (size / 1024 / 1024).ToString(CultureInfo.InvariantCulture) + "MB";
            }
            if (size < 1024.0 * 1024.0 * 1024.0 * 1024.0 && size >= 1024 * 1024 * 1024)
            {
                return (size / 1024 / 1024 / 1024).ToString(CultureInfo.InvariantCulture) + "GB";
            }
            return (size / 1024 / 1024 / 1024 / 1024).ToString(CultureInfo.InvariantCulture) + "TB";
        }
        /// <summary>
        /// 返回带单位的文件大小
        /// </summary>
        /// <param name="size">字节</param>
        /// <returns></returns>
        public static long FileSizeMBbyB(long size)
        {
            return (size / 1024 / 1024);

        }
        /// <summary>
        /// 根据文件大小生成K、M、G的表达方式
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static String GetFileSizeOfNum(long filesize)
        {
            String strFileSize;
            long fileLength = filesize;
            double dFileSize;
            if (fileLength < 1024)
            {
                strFileSize = fileLength.ToString(CultureInfo.InvariantCulture) + " B";
            }
            else if (fileLength < (1024 * 1024))
            {
                dFileSize = fileLength / 1024.0;
                strFileSize = dFileSize.ToString("0.00") + " KB";
            }
            else if (fileLength > (1024 * 1024) && fileLength < (1024 * 1024 * 1024))
            {
                dFileSize = fileLength / 1024.0 / 1024;
                strFileSize = dFileSize.ToString("0.00") + " MB";
            }
            else
            {
                dFileSize = fileLength / 1024.0 / 1024 / 1024;
                strFileSize = dFileSize.ToString("0.00") + " GB";
            }
            return strFileSize;
        }

        /// <summary>
        /// 根据秒数生成时间
        /// </summary>
        /// <param name="timeSize">秒数</param>
        /// <returns>返回时间字符串</returns>
        public static String GetTimeSizeOfNum(long timeSize)
        {
            string strtime;
            long timeLength = timeSize;
            if (timeLength < 60)
            {
                strtime = timeLength.ToString(CultureInfo.InvariantCulture) + " Seconds";
            }
            else if (timeLength < (60 * 60))
            {
                //分
                var timeMinutes = timeLength / 60;
                //秒
                var timeSeconds = timeLength % 60;
                strtime = timeMinutes.ToString(CultureInfo.InvariantCulture) + "Minutes" +
                          timeSeconds.ToString(CultureInfo.InvariantCulture) + "Seconds";
            }
            else if (timeLength > (60 * 60) && timeLength < (60 * 60 * 60))
            {
                //时
                var timeHours = timeLength / 3600;
                //分
                var timeMinutes = (timeLength % 3600) / 60;
                //秒
                var timeSeconds = (timeLength % 3600) % 60;
                strtime = timeHours.ToString(CultureInfo.InvariantCulture) + "Hours" +
                          timeMinutes.ToString(CultureInfo.InvariantCulture) + "Minutes" +
                          timeSeconds.ToString(CultureInfo.InvariantCulture) + "Seconds";
            }
            else
            {
                //天
                var timeDays = timeLength / 86400;
                //时
                var timeHours = (timeLength % 86400) / 3600;
                //分
                var timeMinutes = ((timeLength % 86400) % 3600) / 60;
                //秒
                var timeSeconds = ((timeLength % 86400) % 3600) % 60;
                strtime = timeDays.ToString(CultureInfo.InvariantCulture) + "Days" +
                          timeHours.ToString(CultureInfo.InvariantCulture) + "Hours" +
                          timeMinutes.ToString(CultureInfo.InvariantCulture) + "Minutes" +
                          timeSeconds.ToString(CultureInfo.InvariantCulture) + "Seconds";
            }
            return strtime;
        }

        /// <summary>
        /// 获取以字节为单位的文件大小
        /// </summary>
        /// <param name="path">带路径的文件名</param>
        /// <returns>如果文件存在，则返回以字节为单位的文件大小，否则返回-1</returns>
        public static long GetFileSize(string path)
        {
            if (!File.Exists(path))
                return -1;
            var file = new FileInfo(path);
            return file.Length;
        }

        /// <summary>
        /// 获取以KB为单位的文件大小
        /// </summary>
        /// <param name="path">带路径的文件名</param>
        /// <returns>如果文件存在，则返回以KB为单位的文件大小，否则返回-1</returns>
        public static double GetFileSizeByKb(string path)
        {
            long size = GetFileSize(path);
            if (size == -1)
                return -1;
            return size / 1024.0;
        }

        /// <summary>
        /// 获取以MB为单位的文件大小
        /// </summary>
        /// <param name="path">带路径的文件名</param>
        /// <returns>如果文件存在，则返回以MB为单位的文件大小，否则返回-1</returns>
        public static double GetFileSizeByMb(string path)
        {
            long size = GetFileSize(path);
            if (size == -1)
                return -1;
            return size / (1024.0 * 1024.0);
        }

        /// <summary>
        /// 找目录下指定后缀名的第一个文件
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="ext">不带点 后缀名(全小写)</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool FindFileByExt(string rootDirectory, string ext, out string fileName)
        {
            fileName = string.Empty;
            if (Directory.Exists(rootDirectory))
            {
                string[] files = Directory.GetFiles(rootDirectory);
                string[] dirs = Directory.GetDirectories(rootDirectory);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var fileTmp = file.ToLower();
                        //查找json文件
                        if (!string.IsNullOrEmpty(fileTmp) && fileTmp.EndsWith(ext))
                        {
                            fileName = fileTmp;
                            return true;
                        }
                    }
                }
                if (dirs.Length > 0)
                {
                    foreach (string strT in dirs)
                    {
                        //递归，如果找到一个，直接返回，否则继续循环
                        if (FindFileByExt(strT, ext, out fileName))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 遍历文件夹，找出该文件夹中按名字升序排列且贝壳数字内容生产管理系统支持的第一个文件
        /// </summary>
        /// <param name="rootDirectory">要查找的目录</param>
        /// <param name="subDirectory">找到的子目录</param>
        /// <param name="fileName">找到的第一个文件名</param>
        /// <returns>找到返回true，否则返回false</returns>
        public static bool FirstFileFromDirByAscName(string rootDirectory, out string subDirectory, out string fileName)
        {
            subDirectory = string.Empty;
            fileName = string.Empty;
            if (Directory.Exists(rootDirectory))
            {
                string[] files = Directory.GetFiles(rootDirectory);
                string[] dirs = Directory.GetDirectories(rootDirectory);
                if (files.Length > 0)
                {
                    subDirectory = rootDirectory;
                    string outFile;
                    if (FindAscMinValue(files, out outFile))
                    {
                        fileName = outFile;
                        return true;
                    }

                }
                if (dirs.Length > 0)
                {
                    foreach (string strT in dirs)
                    {
                        //递归，如果找到一个，直接返回，否则继续循环
                        if (FirstFileFromDirByAscName(strT, out subDirectory, out fileName))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 找出字典序最小的文件名
        /// </summary>
        /// <param name="values"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static bool FindAscMinValue(string[] values, out string minValue)
        {
            if (values == null)
            {
                minValue = "";
                return false;
            }
            var sortValues = new List<string>(values);
            sortValues.Sort();
            foreach (var value in sortValues)
            {
                //判断贝壳数字内容生产管理系统是否支持
                if (CanSupportFileType(value))
                {
                    minValue = value;
                    return true;
                }
            }

            minValue = "";
            return false;
        }

        /// <summary>
        /// 获取图片转png后的保存文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetImgTransPngPath(string path)
        {
            string ret = path.Substring(0, path.LastIndexOf(".", StringComparison.Ordinal) + 1);
            ret += "jpg";
            return ret;
        }

        /// <summary>
        /// 创建子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreatePath(string path)
        {
            //非法路径
            if (string.IsNullOrEmpty(path))
                return false;
            //文件夹已存在
            if (Directory.Exists(path))
                return true;

            string[] split = path.Split(new[] { '/', '\\' });
            string strT = "";
            foreach (var s in split)
            {
                if (String.IsNullOrEmpty(s)) continue;
                strT += s;
                if (!Directory.Exists(strT))
                {
                    if (string.IsNullOrEmpty(strT))
                        continue;
                    //创建文件夹
                    Directory.CreateDirectory(strT);
                }
                strT += "\\";
            }
            return true;
        }

        /// <summary>
        /// 创建子文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public static void CreateFile(string path)
        {
            //非法路径
            if (string.IsNullOrEmpty(path))
                return;
            //文件夹已存在
            if (Directory.Exists(path))
                return;

            try
            {
                CreatFilePath(path);
                var stream = File.Create(path);
                stream.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 创建子文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void CreatFilePath(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                CreatePath(fileInfo.Directory.FullName);
        }

        #region 判断文件属性


        /// <summary>
        /// 判断文件是否音频文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsAudio(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "mp3" || fileExt == "mid" || fileExt == "wav" || fileExt == "wma" || fileExt == "wav" ||
                    fileExt == "rm" || fileExt == "audio" || fileExt == "aac");
        }

        /// <summary>
        /// 判断文件是否视频文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsVideo(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "mp4" || fileExt == "wmv" || fileExt == "avi" || fileExt == "flv" || fileExt == "3gp" ||
                    fileExt == "mpg");
        }

        /// <summary>
        /// 判断文件是否文本文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsText(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            if (fileExt == "txt" || fileExt == "html" || fileExt == "mht" || fileExt == "htm")
            {
                return true;
            }
            return false;
            //return (fileExt == "txt");
        }
        /// <summary>
        /// 判断文件是否pdf
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPdf(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            if (fileExt == "pdf")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否为ST4离线试卷
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsSt4(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "st4");
        }


        /// <summary>
        /// 判断文件是否zip压缩文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsZip(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "zip" || fileExt == "rar" || fileExt == "7z");
        }

        /// <summary>
        /// 判断文件是否flash文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsFlash(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "swf" || fileExt == "animation");
        }

        /// <summary>
        /// 判断文件是否图片文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsImage(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "jpg" || fileExt == "gif" || fileExt == "png" || fileExt == "jpeg" || fileExt == "bmp" ||
                    fileExt == "image" || fileExt == "img");

        }

        /// <summary>
        /// 判断文件是否Microsoft Office Word文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsMicrosoftOfficeWord(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "doc" || fileExt == "docx");
        }

        /// <summary>
        /// 判断文件是否Microsoft Office Excel文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsMicrosoftOfficeExcel(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "xls" || fileExt == "xlsx");
        }

        /// <summary>
        /// 判断文件是否Microsoft Office PPT文件
        /// </summary>
        /// <param name="path">文件名称</param>
        public static bool IsMicrosoftOfficePpt(string path)
        {
            string fileExt = GetFileExtNonePoint(path).ToLower();
            return (fileExt == "ppt" || fileExt == "pptx" || fileExt == "pps");
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件完全路径</param>
        /// <param name="destFileName">新文件路径</param>
        /// <param name="overwrite">是否覆盖(默认true)</param>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件完全路径</param>
        /// <param name="destFileName">新文件路径</param>
        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, true);
        }
        /// <summary>
        /// 目录复制
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }

        #endregion

        /// <summary>
        /// 计算本地文件的SHA1值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA1(string filePath)
        {
            var strResult = "";
            var strHashData = "";
            byte[] arrbytHashValue;
            FileStream oFileStream = null;
            SHA1CryptoServiceProvider osha1 = new SHA1CryptoServiceProvider();
            try
            {
                oFileStream = new FileStream(filePath.Replace("\"", ""), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                arrbytHashValue = osha1.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值
                oFileStream.Close();
                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
                strHashData = BitConverter.ToString(arrbytHashValue);
                //替换-
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }
        /// <summary>
        /// 计算字符串的sha1值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HashStringBySha1(string s)
        {
            // 将字符串编码为一个字节序列  
            byte[] bufferValue = Encoding.UTF8.GetBytes(s);
            // 定义加密哈希算法操作类，在System.Security.Cryptography 命名空间 下  
            HashAlgorithm ha = new SHA1CryptoServiceProvider();
            // 计算指定字节数组的哈希值  
            byte[] bufferHash = ha.ComputeHash(bufferValue);
            // 释放由 HashAlgorithm 类使用的所有资源  
            ha.Clear();
            // 将 8 位无符号整数数组的值转换为它的等效 String 表示形式（使用 base 64 数字编码）。  
            return Convert.ToBase64String(bufferHash);
        }

        /// <summary>
        /// 获取文件MD5值
        /// 郭敏
        /// </summary>
        /// <param name="fileName">文件完整路径</param>
        /// <returns>MD5值</returns>
        public static string GetMd5HashFromFile(string fileName)
        {
            try
            {
                if (!FileUtils.Exists(fileName)) return "";
                byte[] retVal;
                using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    retVal = md5.ComputeHash(file);
                    file.Close();
                }
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString() + GetFileExt(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 只获取文件MD5值
        /// </summary>
        /// <param name="fileName">文件完整路径</param>
        /// <returns>MD5值</returns>
        public static string GetMd5HashFromFileOnly(string fileName)
        {
            try
            {
                if (!FileUtils.Exists(fileName)) return "";
                byte[] retVal;
                using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    retVal = md5.ComputeHash(file);
                    file.Close();
                }
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 判断该文件贝壳数字内容生产管理系统是否支持
        /// </summary>
        /// <param name="fileName">文件类型</param>
        /// <returns>true 为贝壳数字内容生产管理系统支持，false为不支持</returns>
        public static bool CanSupportFileType(string fileName)
        {
            var ext = GetFileExtNonePoint(fileName);
            if (string.IsNullOrEmpty(ext))
                return false;

            ext = ext.ToLower();
            switch (ext)
            {
                case "mp3":
                case "mid":
                case "rm":
                case "audio":
                case "aac":
                case "wma":
                case "wav":

                    return true;

                case "mp4":
                case "mpg":
                case "wmv":
                case "avi":
                case "3gp":
                case "flv": //flv 必须归类为视频，要不然备课打不开。
                    return true;



                case "txt":
                case "xml":
                case "html":
                case "chm":
                    return true;
                case "zip":
                    return true;
                case "swf":
                case "animation":

                    return true;

                case "jpg":
                case "gif":
                case "png":
                case "img":
                case "image":
                case "jpeg":
                case "bmp":
                    return true;

                case "doc":
                case "docx":
                    return true;
                case "ppt":
                case "pptx":
                    return true;

                case "xls":
                case "xlsx":
                    return true;
                case "st4":
                    return true;
                case "dcf":
                    return true;

            }
            return false;

        }

        //public static string GetTopVersionForld(string path)
        //{
        //    var directory = new DirectoryInfo(path);
        //    var children = directory.GetDirectories();
        //    if (!children.Any())
        //        return path + "\\1.0\\";
        //    var maxDirectory = children.Max(info => StringUtils.ToDouble(info.Name));
        //    return path + "\\" + maxDirectory + "\\";
        //}

        #region Zip文件相关

        /// <summary>
        /// 判断Zip文件的可执行文件
        /// </summary>
        /// <param name="fileName">文件类型</param>
        /// <returns>true 可执行，false为不可执行</returns>
        public static bool CanExecuteFileType(string fileName)
        {
            var ext = GetFileExtNonePoint(fileName);
            if (string.IsNullOrEmpty(ext))
                return false;

            ext = ext.ToLower();
            switch (ext)
            {
                case "html":
                case "htm":
                case "mht":
                case "doc":
                case "docx":
                case "ppt":
                case "pptx":
                case "jpg":
                case "jpeg":
                case "png":
                case "swf":
                case "mp3":
                case "mp4":
                case "flv":
                case "aac":
                case "txt":
                case "xls":
                case "xlsx":
                case "zip":
                case "exe":
                    return true;
            }
            return false;

        }

        #endregion

        /// <summary>
        /// 文件是否能被删除，
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>true 能被删除 false 不能被删除</returns>
        public static bool DocumentCanDelete(string path)
        {
            if (!string.IsNullOrEmpty(path) &&
                File.Exists(path) &&
                !IsEmploy(path))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool Exists(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("参数为空或NULL");

            return File.Exists(file);

        }

        ///1.判断远程文件是否存在 
        ///fileUrl:远程文件路径，包括IP地址以及详细的路径
        public static bool RemoteFileExists(string fileUrl, out string strErrMsg)
        {
            strErrMsg = string.Empty;
            var result = false;

            // 2. 判断远程文件是否存在
            HttpWebResponse response = null;
            try
            {
                var req = (HttpWebRequest)HttpWebRequest.Create(fileUrl);
                req.ReadWriteTimeout = 2000;
                response = (HttpWebResponse)req.GetResponse();
                result = true;
            }
            catch (Exception exception)
            {
                strErrMsg = exception.Message;
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 文本文件编码格式
        /// </summary>
        public static Encoding GetFileEncodeType(string filename)
        {
            //用using块保证程序执行完后释放对象，不让文件被占用
            using (var fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var br = new System.IO.BinaryReader(fs))
                {
                    var buffer = br.ReadBytes(2);
                    if (buffer[0] >= 0xEF)
                    {
                        if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                        {
                            return System.Text.Encoding.UTF8;
                        }
                        else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                        {
                            return System.Text.Encoding.BigEndianUnicode;
                        }
                        else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                        {
                            return System.Text.Encoding.Unicode;
                        }
                        else
                        {
                            return System.Text.Encoding.Default;
                        }
                    }
                    else
                    {
                        return System.Text.Encoding.Default;
                    }
                }
            }
        }

        #region 获取文件占用空间
        public static long GetFileSizeOnDisk(string file)
        {
            FileInfo info = new FileInfo(file);
            return GetFileSizeOnDisk(info);
        }

        public static long GetFileSizeOnDisk(FileInfo info)
        {
            uint dummy, sectorsPerCluster, bytesPerSector;
            int result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy);
            if (result == 0) throw new Win32Exception();
            uint clusterSize = sectorsPerCluster * bytesPerSector;
            uint hosize;
            uint losize = GetCompressedFileSizeW(info.FullName, out hosize);
            long size;
            size = (long)hosize << 32 | losize;
            return ((size + clusterSize - 1) / clusterSize) * clusterSize;
        }

        [DllImport("kernel32.dll")]
        static extern uint GetCompressedFileSizeW([In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
           [Out, MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        static extern int GetDiskFreeSpaceW([In, MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
           out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
           out uint lpTotalNumberOfClusters);
        #endregion
    }
}
