using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MicroOrm.Dapper.Repositories.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// 
        /// </summary>
        List<string> Logs { get; }
        
        /// <summary>
        /// 
        /// </summary>
        System.Action<string> LogReceived { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="method"></param>
        void LogQuery<T>(string query, [CallerMemberName] string method = null);

        /// <summary>
        /// 
        /// </summary>
        void Start();
        
        /// <summary>
        /// 
        /// </summary>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        void Clear();
    }
}
