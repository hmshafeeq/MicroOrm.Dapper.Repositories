using System;
using System.Linq;
using System.Reflection;
using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using MicroOrm.Dapper.Repositories.Extensions;

namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        private void InitLogicalDeleted()
        {
            var deleteProperty = SqlProperties.FirstOrDefault(p => p.PropertyInfo.GetCustomAttributes<DeletedAttribute>().Any());
            if (deleteProperty == null)
                return;
             
            LogicalDeleteProperty = deleteProperty.PropertyInfo;
            LogicalDeletePropertyMetadata = deleteProperty; 
        }
    }
}
