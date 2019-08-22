using System.Text;

namespace MicroOrm.Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     A object with the generated sql and dynamic params.
    /// </summary>
    public class SqlQuery
    {
        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        public SqlQuery()
        {
            SqlBuilder = new StringBuilder();
            ExecutedOn = System.DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="type"></param>
        public SqlQuery(string table, QueryType type = QueryType.Select)
            : this()
        {
            Table = table;
            Type = type; 
        }

        /// <inheritdoc /> 
        public SqlQuery(string table, object param, QueryType type = QueryType.Select)
            : this()
        {
            Param = param;
            Table = table;
            Type = type; 
        }

        /// <summary>
        ///     SqlBuilder
        /// </summary>
        public StringBuilder SqlBuilder { get; }

        /// <summary>
        ///     Gets the param
        /// </summary>
        public object Param { get; private set; }
    
        /// <summary>
        ///     Gets the Table Name
        /// </summary>
        public string Table { get; private set; }

        /// <summary>
        ///     Gets the Query Type
        /// </summary>
        public QueryType Type { get; private set; }

        /// <inheritdoc />
        public System.DateTime ExecutedOn { get; set; }

        /// <summary>
        ///     Gets the SQL.
        /// </summary>
        public string GetSql()
        {
            return SqlBuilder.ToString().Trim();
        }

        /// <inheritdoc />
        public void SetParam(object param)
        {
            Param = param;
        }

        /// <inheritdoc />
        public void SetTable(string table)
        {
            Table = table;
        }

        /// <inheritdoc />
        public void SetType(QueryType type)
        {
            Type = type;
        }
 

    }
}