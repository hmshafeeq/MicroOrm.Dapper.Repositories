namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     Config for SqlGenerator
    /// </summary>
    public class SqlGeneratorConfig
    {
        /// <summary>
        ///     Type Sql provider
        /// </summary>
        public SqlProvider SqlProvider { get; set; }

        /// <summary>
        ///     Use quotation marks for TableName and ColumnName
        /// </summary>
        public bool UseQuotationMarks { get; set; }

        /// <summary>
        /// A callback which gets invoked when queries and other information are logged.
        /// </summary>
        public System.Action<string> LogReceived;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="method"></param>
        public  void LogQuery<T>(string query, [System.Runtime.CompilerServices.CallerMemberName]string method = null)
            => LogReceived?.Invoke(method != null ? $"{method}<{typeof(T).Name}>: {query}" : query);
    }
}
