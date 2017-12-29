using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisAPILogic.NerveCenters;

namespace JarvisAPILogic
{
    public class Tst
    {
        public static int count = 0;
        public string GetTstStr()
        {
            return count++.ToString();
        }
        public void InitData()
        {
            createTable();
            fillTable();
        }
        public string GetData(string name)
        {
            return NerveCenter.NervObj.ExcuteMsg("北京天气");
            //return printHighscores(name);
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
