using MicroOrm.Dapper.Repositories.SqlGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroOrm.Dapper.Repositories.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public class Logger : ILogger
    {
       

        /// <summary>
        /// A callback which gets invoked when queries and other information are logged.
        /// </summary>
        public System.Action<string, SqlQuery> LogReceived { get; set; }
  
        /// <inheritdoc />
        public void LogQuery<T>(SqlQuery query, [System.Runtime.CompilerServices.CallerMemberName]string method = null)
            => LogReceived?.Invoke(method != null ? $"{method}<{typeof(T).Name}>" : typeof(T).Name, query);



    }
}
