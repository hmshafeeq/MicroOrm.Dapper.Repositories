using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using MicroOrm.Dapper.Repositories.Attributes;
using MicroOrm.Dapper.Repositories.Attributes.Joins;

namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     Metadata from PropertyInfo
    /// </summary>
    public class SqlPropertyMetadata
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlPropertyMetadata(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            var alias = PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (!string.IsNullOrEmpty(alias?.Name))
            {
                Alias = alias.Name;
                ColumnName = Alias;
            }
            else
            {
                ColumnName = PropertyInfo.Name;
            }

            Ignore = PropertyInfo.GetCustomAttribute<IgnoreAttribute>() != null ||
                    PropertyInfo.GetCustomAttribute<JoinAttributeBase>() != null;
        }

        /// <summary>
        ///     Original PropertyInfo
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        ///     Alias for ColumnName
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        ///     ColumnName
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Exclude property 
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        ///     PropertyName
        /// </summary>
        public virtual string PropertyName => PropertyInfo.Name;
    }
}
