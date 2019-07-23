namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     Config for SqlGenerator
    /// </summary>
    public class SqlGeneratorConfig
    {
        /// <summary>
        ///     Type Sql provider
        /// </summary>
        public SqlProvider SqlProvider { get; set; }

        /// <summary>
        ///     Use quotation marks for TableName and ColumnName
        /// </summary>
        public bool UseQuotationMarks { get; set; }


        /// <summary>
        /// Field with attribte SyncStatus, will be set to null/false on each insert, update, logical delete
        /// if TrackSyncStatus is set to true
        /// </summary>
        public bool TrackSyncStatus { get; set; } = true;


        /// <summary>
        /// Track CreatedAt and UpdatedAt timestamps
        /// </summary>
        public bool TrackTimeStamps { get; set; } = true;
    }
}
