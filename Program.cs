using System;
using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;

namespace lab2 {
    class Program {
        static void Main(string[] args) {
            for(int i=0;i<=20;i++){

                DateTime dnowTime = DateTime.Now;
                string nowTime = dnowTime.ToString("yyyy-MM-dd HH:mm:ss");
                string connectionString = "datasource=localhost;port=3306;username=root;password=;database=architectorlab;";
                string selectquery = "SELECT * FROM lab1new";
                string texts = "------------\n"+nowTime+"\t"+i;
                string insertquery = "insert into lab1new(time,name) values("+"'"+ nowTime +"'"+","+i+")";
                // Console.WriteLine(nowTime);
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand insertDatabase = new MySqlCommand(insertquery, databaseConnection);
                MySqlCommand selectDatabase = new MySqlCommand(selectquery, databaseConnection);
                selectDatabase.CommandTimeout = 10;
                insertDatabase.CommandTimeout = 10;
                try {
                    Console.WriteLine("Server connected :D");
                    int counter = 0;  
                    string line;
                    System.IO.StreamReader file =   
                        new System.IO.StreamReader(@"logfile.txt");  
                    while((line = file.ReadLine()) != null) {
                        Console.WriteLine(line);  
                        string syncquery = "insert into lab1new(time,name,errdesc) values("+line+")";
                        // Console.WriteLine(syncquery);
                        MySqlCommand syncDatabase = new MySqlCommand(syncquery, databaseConnection);
                        syncDatabase.CommandTimeout = 10;
                        MySqlDataReader syncreader;
                        databaseConnection.Open();
                        syncreader = syncDatabase.ExecuteReader();
                        Console.WriteLine("Backup starting");
                        databaseConnection.Close();

                        counter++;  
                    }
                    File.WriteAllText(@"logfile.txt", String.Empty);
                    file.Close();  
                    Console.WriteLine("Backup done. {0} line log synced.", counter);
                    Console.WriteLine("running");
                    MySqlDataReader reader;
                    databaseConnection.Open();
                    reader = insertDatabase.ExecuteReader();
                    // reader = selectDatabase.ExecuteReader();
                    // Console.WriteLine("connected");
                    // if (reader.HasRows) {
                    //     while (reader.Read()) {
                    //         Console.WriteLine("{0}\t{1}", reader.GetString(0),reader.GetString(1));
                    //     }
                    // } else {
                    //     Console.WriteLine("No rows found.");
                    // }
                }
                catch (Exception ex) {
                    Console.WriteLine("===== sad :(( \t" + ex.Message + "=====");

                    StreamWriter log;
                    log = File.AppendText("logfile.txt");
                    log.WriteLine("'"+ nowTime +"' ,"+" '"+i+"' , '" + ex.Message+"'");
                    log.Close();
                }
                databaseConnection.Close();
                Thread.Sleep(2000);
            }
        }
    }
}
