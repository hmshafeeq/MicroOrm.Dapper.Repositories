using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroOrm.Dapper.Repositories
{

    /// <summary>
    /// Query types
    /// </summary>
    public enum QueryType
    {
        /// <inheritdoc />
        Select,
        /// <inheritdoc />
        Insert,
        /// <inheritdoc />
        Delete,
        /// <inheritdoc />
        Update
    }
}
