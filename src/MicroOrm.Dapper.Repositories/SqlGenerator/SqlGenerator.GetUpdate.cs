using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MicroOrm.Dapper.Repositories.Extensions;

namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.Ignore).ToArray();
            if (!properties.Any())
                throw new ArgumentException("Can't update without [Key]");

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder.Append(" WHERE ");

            query.SqlBuilder.Append(string.Join(" AND ", KeySqlProperties.Where(p => !p.Ignore)
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            return query;
        }


        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.Ignore).ToArray();

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder
                .Append(" ");

            AppendWherePredicateQuery(query, predicate, QueryType.Update);

            var parameters = new Dictionary<string, object>();
            var entityType = entity.GetType();
            foreach (var property in properties)
                parameters.Add(property.PropertyName, entityType.GetProperty(property.PropertyName).GetValue(entity, null));

            if (query.Param is Dictionary<string, object> whereParam)
                parameters.AddRange(whereParam);

            query.SetParam(parameters);

            return query;
        }


        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(Expression<Func<TEntity, bool>> predicate, object entity)
        {

            var objectProperties = entity.GetType().GetProperties().ToList();

            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.Ignore)
                .Where(s => objectProperties.Any(x => x.Name == s.PropertyName) || UpdatedAtProperty.Name == s.PropertyName).ToArray();
             
            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder
                .Append(" ");

            AppendWherePredicateQuery(query, predicate, QueryType.Update);

            var parameters = new Dictionary<string, object>();
            var entityType = entity.GetType();
            foreach (var property in properties)
            {
                if (property.PropertyName == UpdatedAtProperty.Name)
                    parameters.Add(property.PropertyName, $"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}");
                else
                    parameters.Add(property.PropertyName, entityType.GetProperty(property.PropertyName).GetValue(entity, null));
            }

            if (query.Param is Dictionary<string, object> whereParam)
                parameters.AddRange(whereParam);

            query.SetParam(parameters);

            return query;
        }
    }
}
