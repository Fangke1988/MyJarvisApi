using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisAPILogic.NerveCenters;
using System.Timers;

namespace JarvisAPILogic
{
    public class Tst
    {
        Timer ti = new Timer();
        public static int count = 0;
        private const string CommandString = @" Running-启动JARVIS\r\n Stoping-停止JARVIS\r\n downstart-启动Bing图片下载\r\n downstop-停止Bing图片下载\r\n bing-获取今日Bing壁纸\r\n XX天气-获取指定城市(XX)的今日天气";
        public string GetTstStr()
        {
            return count++.ToString();
        }
        public void InitData()
        {
            createTable();
            fillTable();
        }
        static bool UseJavis = false;
        public string GetData(string name)
        {
            switch (name.ToLower()) {
                case "running":
                    UseJavis = true;
                    return "Edvin Jarvis 已启动，可接受指定指令";
                case "stoping":
                    UseJavis = false;
                    return "Edvin Jarvis 已休眠，停止接收指定指令";
                case "help":
                case "指令":
                case "帮助":
                case "command":
                    return CommandString;
                default:
                    break;
            }

            if (UseJavis)
            {
                if (name.IndexOf("在吗") != -1)
                {
                    return "消息已接收，即将回复您\r\n----[Edvin Jarvis]";
                }
                if (name.ToLower() == "downstart")
                {
                    ti.Interval = 30000;
                    ti.Elapsed -= Ti_Elapsed;
                    ti.Elapsed += Ti_Elapsed;
                    ti.Start();
                    LogWritter.Write(LogType.Debug, "开始启动Timer", "BingIMGDownloader");
                    return "START COMMADE";

                }
                if (name.ToLower() == "downstop")
                {
                    ti.Stop();
                    LogWritter.Write(LogType.Debug, "开始启动Timer", "BingIMGDownloader");
                    return "STOP COMMADE";
                }
                return NerveCenter.NervObj.ExcuteMsg(name);
            }
            else
            {
                return "";
            }
        }

        private void Ti_Elapsed(object sender, ElapsedEventArgs e)
        {
            LogWritter.Write(LogType.Debug, "Timer定时执行", "BingIMGDownloader");
            NerveCenter.NervObj.ExcuteMsg("SAVE-IMG");
        }


        //在指定数据库中创建一个table
        void createTable()
        {
            string sql = "create table Contacts (name varchar(20), number varchar(20))";
            SQLiteCommand command = new SQLiteCommand(sql, SQLiteJarvisClient.Connection);
            command.ExecuteNonQuery();
           
        }

        //插入一些数据
        void fillTable()
        {
            string sql = "insert into Contacts (name, number) values ('Me', '15507310090')";
            SQLiteCommand command = new SQLiteCommand(sql, SQLiteJarvisClient.Connection);
            command.ExecuteNonQuery();

            string sql2 = "insert into Contacts (name, number) values ('Wife', '15584810811')";
            SQLiteCommand command2 = new SQLiteCommand(sql2, SQLiteJarvisClient.Connection);
            command2.ExecuteNonQuery();

            string sql3 = "insert into Contacts (name, number) values ('pol', '110')";
            SQLiteCommand command3 = new SQLiteCommand(sql3, SQLiteJarvisClient.Connection);
            command3.ExecuteNonQuery();
        }

        //使用sql查询语句，并显示结果
        string  printHighscores(string name)
        {
            string sql = "select * from Contacts where name='" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, SQLiteJarvisClient.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Name: " + reader["name"] + "\t TelNumber: " + reader["number"]);
                return reader["number"].ToString();
            }
            return "";
        }
    }
}
