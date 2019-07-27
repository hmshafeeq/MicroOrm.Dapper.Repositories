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
        public virtual SqlQuery GetBulkInsert(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties =
                (IsIdentity
                    ? SqlProperties.Where(p => !p.PropertyName.Equals(IdentitySqlProperty.PropertyName, StringComparison.OrdinalIgnoreCase))
                    : SqlProperties).ToList();

            var query = new SqlQuery();

            var values = new List<string>();
            var parameters = new Dictionary<string, object>();
            var keyProperty = KeySqlProperties.Where(s => s.PropertyInfo.PropertyType == typeof(Guid) || s.PropertyInfo.PropertyType == typeof(Guid?)).FirstOrDefault();
            for (var i = 0; i < entitiesArray.Length; i++)
            {
                #region If There Is No Identity Property 
                if (!IsIdentity && keyProperty != null)
                    keyProperty.PropertyInfo.SetValue(entitiesArray[i], Guid.NewGuid());
                #endregion

                if (HasCreatedAt)
                    CreatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);

                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);

                if (TrackSyncStatus)
                    SyncStatusProperty.SetValue(entitiesArray[i], SyncStatusProperty.PropertyType.IsDateTime() ? null : "0");

                foreach (var property in properties)
                    // ReSharper disable once PossibleNullReferenceException
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                values.Add(string.Format("({0})", string.Join(", ", properties.Select(p => "@" + p.PropertyName + i))));
            }

            query.SqlBuilder.AppendFormat("INSERT INTO {0} ({1}) VALUES {2}", TableName, string.Join(", ", properties.Select(p => p.ColumnName)), string.Join(",", values)); // values

            query.SetParam(parameters);

            return query;
        }



    }
}
