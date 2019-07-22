using System;
using System.Data;
using System.Linq;
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
        public virtual object Insert(TEntity instance)
        {
            return Insert(instance, null);
        }

        /// <inheritdoc />
        public virtual object Insert(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);

            LogQuery<TEntity>(queryResult.GetSql());

            if (SqlGenerator.IsIdentity)
            {
                var newId = Connection.Query<long>(queryResult.GetSql(), queryResult.Param, transaction).FirstOrDefault();
                return SetValue(newId, instance);
            }

            return Connection.Execute(queryResult.GetSql(), instance, transaction) > 0 ? GetLastInsertId(instance) : false;
        }

        /// <inheritdoc />
        public virtual Task<object> InsertAsync(TEntity instance)
        {
            return InsertAsync(instance, null);
        }

        /// <inheritdoc />
        public virtual async Task<object> InsertAsync(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);

            LogQuery<TEntity>(queryResult.GetSql());

            if (SqlGenerator.IsIdentity)
            {
                var newId = (await Connection.QueryAsync<long>(queryResult.GetSql(), queryResult.Param, transaction)).FirstOrDefault();
                return SetValue(newId, instance);
            }
            return await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction) > 0 ? GetLastInsertId(instance) : false; ;
        }

        private bool SetValue(long newId, TEntity instance)
        {
            var added = newId > 0;
            if (added)
            {
                var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyInfo.PropertyType);
                SqlGenerator.IdentitySqlProperty.PropertyInfo.SetValue(instance, newParsedId);
            }
            return added;
        }

        private object GetLastInsertId(TEntity instance)
        {
            var keyProperty = SqlGenerator.KeySqlProperties.FirstOrDefault();
            if (keyProperty != null)
                return keyProperty.PropertyInfo.GetValue(instance);
            return null;
        }
    }
}
