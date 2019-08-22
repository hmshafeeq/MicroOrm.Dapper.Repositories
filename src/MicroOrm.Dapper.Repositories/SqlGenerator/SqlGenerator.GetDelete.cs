using System;
using System.Linq;
using System.Linq.Expressions;
using MicroOrm.Dapper.Repositories.Extensions;
namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual SqlQuery GetDelete(TEntity entity)
        {
            var sqlQuery = new SqlQuery(TableName, QueryType.Delete);
            var whereAndSql =
                string.Join(" AND ", KeySqlProperties.Select(p => string.Format("{0}.{1} = @{2}", TableName, p.ColumnName, p.PropertyName)));

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder
                    .Append("DELETE FROM ")
                    .Append(TableName)
                    .Append(" WHERE ")
                    .Append(whereAndSql);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append("UPDATE ")
                    .Append(TableName)
                    .Append(" SET ")
                    .Append(LogicalDeletePropertyMetadata.ColumnName)
                    .Append(" = ")
                    .Append(LogicalDeleteProperty.PropertyType.IsDateTime() ? $"'{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}'" : GetLogicalDeleteValue());

                if (HasUpdatedAt)
                {
                    UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

                    sqlQuery.SqlBuilder
                        .Append(", ")
                        .Append(UpdatedAtPropertyMetadata.ColumnName)
                        .Append(" = @")
                        .Append(UpdatedAtPropertyMetadata.PropertyName);
                }

                sqlQuery.SqlBuilder
                    .Append(" WHERE ")
                    .Append(whereAndSql);
            }

            sqlQuery.SetParam(entity);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlQuery = new SqlQuery(TableName, QueryType.Delete);

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder
                    .Append("DELETE FROM ")
                    .Append(TableName);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append("UPDATE ")
                    .Append(TableName)
                    .Append(" SET ")
                    .Append(LogicalDeletePropertyMetadata.ColumnName)
                    .Append(" = ")
                    .Append(LogicalDeleteProperty.PropertyType.IsDateTime() ? $"'{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}'" : GetLogicalDeleteValue());

                if (HasUpdatedAt)
                    sqlQuery.SqlBuilder
                        .Append(", ")
                        .Append(UpdatedAtPropertyMetadata.ColumnName)
                        .Append(" = @")
                        .Append(UpdatedAtPropertyMetadata.PropertyName);


            }
            sqlQuery.SqlBuilder.Append(" ");
            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Delete);
            return sqlQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private object GetLogicalDeleteValue()
        {
            if (LogicalDeleteProperty.PropertyType.IsBool())
            {
                return 1;
            }
            else if (LogicalDeleteProperty.PropertyType.IsEnum())
            {
                var enumValue = Enum.Parse(LogicalDeleteProperty.PropertyType, LogicalDeleteProperty.Name);
                return Convert.ChangeType(enumValue, Enum.GetUnderlyingType(LogicalDeleteProperty.PropertyType));
            }

            return null;
        }
    }
}
