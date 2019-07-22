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
        private List<string> _logs = new List<string> { };

        /// <summary>
        /// A callback which gets invoked when queries and other information are logged.
        /// </summary>
        public System.Action<string> LogReceived { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Logs
        {
            get
            {
                if (LogReceived == null)  throw new Exception("Logger was not started, you can not access logs.");

                return _logs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="method"></param>
        public void LogQuery<T>(string query, [System.Runtime.CompilerServices.CallerMemberName]string method = null)
            => LogReceived?.Invoke(method != null ? $"{method}<{typeof(T).Name}>: {query}" : query);
         

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        { 
            LogReceived += log => _logs.Add(log);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop() => LogReceived = null;

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => _logs.Clear();

    }
}
