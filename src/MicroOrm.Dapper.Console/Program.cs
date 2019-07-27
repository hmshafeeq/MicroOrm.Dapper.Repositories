using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

                    conn.Logger.Start();

                  for (int i = 0; i < 10; i++)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        DateTime? date = null;

                        //var category = (conn.Categories.FindAll()).FirstOrDefault();
                        //category.SyncedAt = DateTime.UtcNow.Subtract(new TimeSpan(0, 20, 0));
                        //conn.Categories.Update(category);

                        //System.Console.WriteLine(categories.Count().ToString() + " Items fetched");
                        //using (var tran = conn.BeginTransaction())
                        //{
                        //    var result = conn.Categories.BulkUpdate(categories,tran);

                        //    tran.Commit();
                        //}

                        //conn.Categories.Insert(new Category
                        //{
                        //    ButtonColor = "$33",
                        //    Name = "123123"

                        //});

                        //System.Console.WriteLine("Item added");

                        //var category = conn.Categories.FindById(id);

                        //System.Console.WriteLine("Item fetched");

                        //category.DeletedAt = null;
                        //category.SyncedAt = DateTime.UtcNow;
                        //conn.Categories.Update(category);
                        //System.Console.WriteLine("Item updated");

                        //conn.Categories.Delete(category);
                        //System.Console.WriteLine("Item deleted");
                        //watch.Stop();

                        var category = (conn.Items.FindFields(x=>x.DeletedAt)); 
                        System.Console.WriteLine(Convert.ToDecimal(watch.ElapsedMilliseconds) / 1000);

                        System.Console.WriteLine(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    System.Console.Write(ex.ToString());


                }
                if (conn.Logger.Logs != null)
                {
                    File.WriteAllText("sql.txt", string.Join("\r\n===============================================\r\n", conn.Logger.Logs));
                    System.Console.Write(string.Join("\r\n\r\n", conn.Logger.Logs));
                }

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
