using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using MicroOrm.Dapper.Repositories.Attributes.Joins;
using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using MicroOrm.Dapper.Repositories.Extensions;
using MicroOrm.Dapper.Repositories.SqlGenerator.QueryExpressions;

namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity> : ISqlGenerator<TEntity>
        where TEntity : class
    {

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator()
            : this(new SqlGeneratorConfig
            {
                SqlProvider = SqlProvider.MSSQL,
                UseQuotationMarks = false
            })
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlProvider sqlProvider, bool useQuotationMarks = false)
            : this(new SqlGeneratorConfig
            {
                SqlProvider = sqlProvider,
                UseQuotationMarks = useQuotationMarks
            })
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlGeneratorConfig sqlGeneratorConfig)
        {
            // Order is important
            InitProperties();
            InitConfig(sqlGeneratorConfig);
            InitLogicalDeleted();
        }

        /// <inheritdoc />
        public PropertyInfo[] AllProperties { get; protected set; }

        /// <inheritdoc />
        public bool HasUpdatedAt => UpdatedAtProperty != null && (Config == null || Config.TrackTimeStamps);

        /// <inheritdoc />
        public PropertyInfo UpdatedAtProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata UpdatedAtPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public bool HasCreatedAt => CreatedAtProperty != null && (Config == null || Config.TrackTimeStamps);

        /// <inheritdoc />
        public PropertyInfo CreatedAtProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata CreatedAtPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public bool IsIdentity => IdentitySqlProperty != null;

        /// <inheritdoc />
        public string TableName { get; protected set; }

        /// <inheritdoc />
        public string TableSchema { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata IdentitySqlProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata[] KeySqlProperties { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata[] SqlProperties { get; protected set; }

        /// <inheritdoc />
        public SqlJoinPropertyMetadata[] SqlJoinProperties { get; protected set; }

        /// <inheritdoc />
        public SqlGeneratorConfig Config { get; protected set; }

        /// <inheritdoc />
        public bool LogicalDelete => LogicalDeleteProperty != null;

        /// <inheritdoc />
        public PropertyInfo LogicalDeleteProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata LogicalDeletePropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public PropertyInfo SyncStatusProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata SyncStatusPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public bool TrackSyncStatus => SyncStatusProperty!=null && (Config == null || Config.TrackSyncStatus);


        /// <summary>
        ///     Get join/nested properties
        /// </summary>
        /// <returns></returns>
        private static SqlJoinPropertyMetadata[] GetJoinPropertyMetadata(PropertyInfo[] joinPropertiesInfo)
        {
            // Filter and get only non collection nested properties
            var singleJoinTypes = joinPropertiesInfo.Where(p => !p.PropertyType.IsConstructedGenericType).ToArray();

            var joinPropertyMetadatas = new List<SqlJoinPropertyMetadata>();

            foreach (var propertyInfo in singleJoinTypes)
            {
                var joinInnerProperties = propertyInfo.PropertyType.GetProperties().Where(q => q.CanWrite)
                    .Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();
                joinPropertyMetadatas.AddRange(joinInnerProperties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new SqlJoinPropertyMetadata(propertyInfo, p)).ToArray());
            }

            return joinPropertyMetadatas.ToArray();
        }

        private static string GetTableNameWithSchemaPrefix(string tableName, string tableSchema, string startQuotationMark = "", string endQuotationMark = "")
        {
            return !string.IsNullOrEmpty(tableSchema)
                ? startQuotationMark + tableSchema + endQuotationMark + "." + startQuotationMark + tableName + endQuotationMark
                : startQuotationMark + tableName + endQuotationMark;
        }

        private enum QueryType
        {
            Select,
            Delete,
            Update
        }
    }
}
