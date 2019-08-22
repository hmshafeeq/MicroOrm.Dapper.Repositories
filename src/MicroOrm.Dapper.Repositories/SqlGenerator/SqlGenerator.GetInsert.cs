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
        public virtual SqlQuery GetInsert(TEntity entity)
        {
            var properties =
                (IsIdentity
                    ? SqlProperties.Where(p => !p.PropertyName.Equals(IdentitySqlProperty.PropertyName, StringComparison.OrdinalIgnoreCase))
                    : SqlProperties).ToList();

            var keyProperty = KeySqlProperties.Where(s => s.PropertyInfo.PropertyType == typeof(Guid) || s.PropertyInfo.PropertyType == typeof(Guid?)).FirstOrDefault();

            #region If There Is No Identity Property 
            if (!IsIdentity && keyProperty != null)
            {
                var oldKey = keyProperty.PropertyInfo.GetValue(entity, null);
                if (oldKey == null || (Guid?)oldKey == default(Guid?))
                    keyProperty.PropertyInfo.SetValue(entity, Guid.NewGuid());
            }
            #endregion

            if (HasCreatedAt)
                CreatedAtProperty.SetValue(entity, DateTime.UtcNow);

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);
             
            var query = new SqlQuery(TableName, entity, QueryType.Insert); 

            query.SqlBuilder.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2})", TableName, string.Join(", ", properties.Select(p => p.ColumnName)),
                                            string.Join(", ", properties.Select(p => "@" + p.PropertyName))); // values

            if (IsIdentity)
                switch (Config.SqlProvider)
                {
                    case SqlProvider.MSSQL:
                        query.SqlBuilder.Append(" SELECT SCOPE_IDENTITY() AS " + IdentitySqlProperty.ColumnName);
                        break;

                    case SqlProvider.MySQL:
                        query.SqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " + IdentitySqlProperty.ColumnName);
                        break;

                    case SqlProvider.PostgreSQL:
                        query.SqlBuilder.Append(" RETURNING " + IdentitySqlProperty.ColumnName);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            return query;
        }
    }
}
