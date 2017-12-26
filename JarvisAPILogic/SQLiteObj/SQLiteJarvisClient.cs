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
                try { 
                if (client == null)
                {
                    client = new SQLiteJarvisClient();
                    shutDownTimer = new System.Timers.Timer() { AutoReset = true, Interval = 1000 };
                    shutDownTimer.Elapsed -= ShutDownTimer_Elapsed;
                    shutDownTimer.Elapsed += ShutDownTimer_Elapsed;
                }

                if (connection == null)
                {
                    //connection = new SQLiteConnection("DataSource=" + @"E:\CODE\DB\DataBase\MyJarvis.db" + ";Version=3;New=False;Compress=True;");
                    connection = new SQLiteConnection("DataSource=" + @"H:\Sqlite\DataBase\MyJarvis.db" + ";Version=3;New=False;Compress=True;");
                    connection.Open();
                    shutDownTimer.Start();
                } else
                {
                    if (shutSec == 0)
                    {
                        connection = new SQLiteConnection("DataSource=" + @"H:\Sqlite\DataBase\MyJarvis.db" + ";Version=3;New=False;Compress=True;");
                        connection.Open();
                    }
                    else
                    {
                        switch (connection.State)
                        {
                            case System.Data.ConnectionState.Closed:
                                connection.Open();
                                shutDownTimer.Start();
                                break;
                            case System.Data.ConnectionState.Broken:
                                connection.Close();
                                connection.Open();
                                shutDownTimer.Start();
                                break;
                            default:
                                break;
                        }
                    }
                }
                shutSec = 30;
                }
                catch (Exception exp) {

                }
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
                
                shutDownTimer.Stop();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}
