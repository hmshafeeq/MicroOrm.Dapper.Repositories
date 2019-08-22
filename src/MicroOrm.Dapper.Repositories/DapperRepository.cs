using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories.Logger;
using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace MicroOrm.Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity> : IDapperRepository<TEntity>
        where TEntity : class
    {


        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, ILogger logger = null)
        {
            Logger = logger;
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(SqlProvider.MSSQL);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlProvider sqlProvider, ILogger logger = null)
        {
            Logger = logger;
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(sqlProvider);

        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator, ILogger logger = null)
        {
            Logger = logger;
            Connection = connection;
            SqlGenerator = sqlGenerator;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlGeneratorConfig config, ILogger logger = null)
        {
            Logger = logger;
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(config);
        }

        /// <inheritdoc />
        public ILogger Logger { get; }

        /// <inheritdoc />
        public IDbConnection Connection { get; }

        /// <inheritdoc />
        public ISqlGenerator<TEntity> SqlGenerator { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="methodName"></param>
        internal void LogQuery<T>(SqlQuery sqlQuery, [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            if (Logger?.LogReceived != null)
            {  
                Logger?.LogQuery<T>(sqlQuery, methodName);
            }

        }

    }
}
