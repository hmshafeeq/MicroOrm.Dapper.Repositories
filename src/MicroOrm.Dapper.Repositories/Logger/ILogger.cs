using MicroOrm.Dapper.Repositories.SqlGenerator;
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
        System.Action<string, SqlQuery> LogReceived { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="method"></param>
        void LogQuery<T>(SqlQuery query, [CallerMemberName] string method = null);
 
    }
}
