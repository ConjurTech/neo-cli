using Neo.Shell;
using Neo.Wallets;
using System;
using System.IO;
using Npgsql;

namespace Neo
{
    static class Program
    {
          internal static Wallet Wallet;

          private static string Host = "localhost";
          private static string User = "postgres";
          private static string DBname = "neocli";
          private static string Password = "postgres";
          private static string Port = "5432";

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                PrintErrorLogs(w, (Exception)e.ExceptionObject);
            }
        }

        static void Main(string[] args)
        {
            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4}; SSL Mode=Prefer; Trust Server Certificate=true",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            var conn = new NpgsqlConnection(connString);
            conn.Open();
            var command = conn.CreateCommand();

            command.CommandText =
                String.Format(
                    @"
                                INSERT INTO events (category, data) VALUES ({0}, {1});
                                INSERT INTO events (category, data) VALUES ({2}, {3});
                                INSERT INTO events (category, data) VALUES ({4}, {5});
                            ",
                    "\'history1\'", "\'{\"sampledata\": \"data\"}\'",
                    "\'history2\'", "\'{\"sampledata2\": \"data2\"}\'",
                    "\'history3\'", "\'{\"sampledata3\": \"data3\"}\'"
                    );

            int nRows = command.ExecuteNonQuery();
            Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));

            Console.Out.WriteLine("Closing connection");
            conn.Close();

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();

//AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
//new MainService().Run(args);
        }

        private static void PrintErrorLogs(StreamWriter writer, Exception ex)
        {
            writer.WriteLine(ex.GetType());
            writer.WriteLine(ex.Message);
            writer.WriteLine(ex.StackTrace);
            if (ex is AggregateException ex2)
            {
                foreach (Exception inner in ex2.InnerExceptions)
                {
                    writer.WriteLine();
                    PrintErrorLogs(writer, inner);
                }
            }
            else if (ex.InnerException != null)
            {
                writer.WriteLine();
                PrintErrorLogs(writer, ex.InnerException);
            }
        }
    }
}
