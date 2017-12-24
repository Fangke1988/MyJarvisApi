using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace JarvisAPILogic
{
    internal class SQLiteJarvisClient
    {
        static SQLiteJarvisClient client;
        static int shutSec=30;
        private static SQLiteConnection connection;
        static System.Timers.Timer shutDownTimer;

        public static SQLiteConnection Connection
        {
            get
            {
                //if (client == null)
                //{
                //    client = new SQLiteJarvisClient();
                //    shutDownTimer = new System.Timers.Timer() { AutoReset = true, Interval = 1000 };
                //    shutDownTimer.Elapsed -= ShutDownTimer_Elapsed;
                //    shutDownTimer.Elapsed += ShutDownTimer_Elapsed;
                //}

                if (connection == null)
                {
                    connection = new SQLiteConnection("DataSource=" + @"E:\CODE\DB\DataBase\MyJarvis.db" + ";Version=3;New=False;Compress=True;");
                    connection.Open();
                    //shutDownTimer.Start();
                }
                else
                    shutSec = 30;
                return connection;
            }
        }


        private static void ShutDownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (shutSec > 0)
            {
                //定时器减一秒
                shutSec--;
            }
            else
            {
                connection.Close();
                connection.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                shutDownTimer.Stop();
            }
        }

    }
}
