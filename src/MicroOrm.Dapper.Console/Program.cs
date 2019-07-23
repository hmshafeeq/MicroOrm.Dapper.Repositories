using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroOrm.Dapper.Console
{
    class Program
    {


        private static string connectionString = $"Server=localhost;Port=3306;Database=outerpos_lcc;Uid=root;Pwd=root;";
        static void Main(string[] args)
        {

            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 

            var logs = new List<string>();

            #region Enable Logging
            // Provide a callback to capture logs 
            // DommelMapper.LogReceived += log => logs.Add(log);
            #endregion

            using (var conn = GetConnection())
            {

                try
                {
                    conn.Logger.Start();

                    //for (int i = 0; i < 10; i++)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        var id = Guid.Parse("d2fe6aa2-0e08-4e0e-95aa-88b2964da0f3");
                        DateTime? date = null;
                        //var user = conn.Categories.InsertAsync(new Category
                        //{
                        //    ButtonColor = "$33",
                        //    Name = "123123"

                        //}); ; ;

                       var user2 = conn.Categories.FindAll<Item>(s=>s.DeletedAt == null,x=>x.Items);
                        watch.Stop();

                        System.Console.WriteLine(Convert.ToDecimal(watch.ElapsedMilliseconds) / 1000);

                        System.Console.WriteLine(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    System.Console.Write(ex.ToString());


                }

                System.Console.Write(string.Join("\r\n\r\n", conn.Logger.Logs));


            }

            #region Disable Logging
            // Optionally remove the callback when done
            //  DommelMapper.LogReceived = null;
            #endregion

            System.Console.ReadKey();
        }


        private static MySqlDbContext GetConnection()
        {
            return new MySqlDbContext(connectionString);
        }
    }
}
