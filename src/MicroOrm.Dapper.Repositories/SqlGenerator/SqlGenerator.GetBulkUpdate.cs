using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MicroOrm.Dapper.Repositories.Extensions;
using System.Reflection;
namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {

        /// <inheritdoc />
        public virtual SqlQuery GetBulkUpdate(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.Ignore).ToArray();
               
            var query = new SqlQuery(TableName, QueryType.Update);

            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < entitiesArray.Length; i++)
            {
                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);
 
                if (i > 0)
                    query.SqlBuilder.Append("; ");

                query.SqlBuilder.Append(string.Format("UPDATE {0} SET {1} WHERE {2}", TableName,
                                                        string.Join(", ", properties.Select(p => string.Format("{0} = @{1}{2}", p.ColumnName, p.PropertyName, i))),
                                                        string.Join(" AND ", KeySqlProperties.Where(p => !p.Ignore)
                                                                                             .Select(p => string.Format("{0} = @{1}{2}", p.ColumnName, p.PropertyName, i)))
                                                    ));

                // ReSharper disable PossibleNullReferenceException
                foreach (var property in properties)
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                foreach (var property in KeySqlProperties.Where(p => !p.Ignore))
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                // ReSharper restore PossibleNullReferenceException
            }

            query.SetParam(parameters);

            return query;
        }
    }
}
