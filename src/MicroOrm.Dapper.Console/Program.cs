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


        private static string connectionString = $"Server=localhost;Port=3307;Database=client;Uid=root;Pwd=root;";
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
                    conn.Config.LogReceived += log => logs.Add(log);

                    //for (int i = 0; i < 10; i++)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        var id = Guid.Parse("81465fee-43e1-4294-bc6f-b74ce1ad0f33");
                        var user = conn.Categories.Update(s => s.DeletedAt != null, new Category{ Published = 1 });
                        watch.Stop();

                        System.Console.WriteLine(Convert.ToDecimal(watch.ElapsedMilliseconds) / 1000);

                        System.Console.WriteLine(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    System.Console.Write(ex.ToString());


                }

                // var ddd = conn.UpdateAsync<Item>(new { Name = "Hello World",DescriptionShort = "Hello World" }, z => z.Quantity == 98);


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
