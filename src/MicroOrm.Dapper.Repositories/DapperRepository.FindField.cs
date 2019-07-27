using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;


namespace MicroOrm.Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> FindFields(params Expression<Func<TEntity, object>>[] fields)
        {
            var queryResult = SqlGenerator.GetAllSelectedFields(null, fields);

            LogQuery<TEntity>(queryResult);

            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> FindFields(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] fields)
        {
            var queryResult = SqlGenerator.GetAllSelectedFields(predicate, fields);

            LogQuery<TEntity>(queryResult);

            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> FindFieldsAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] fields)
        {
            var queryResult = SqlGenerator.GetAllSelectedFields(predicate, fields);

            LogQuery<TEntity>(queryResult);

            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> FindFieldsAsync(params Expression<Func<TEntity, object>>[] fields)
        {
            var queryResult = SqlGenerator.GetAllSelectedFields(null, fields);

            LogQuery<TEntity>(queryResult);

            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param);
        }


    }
}
