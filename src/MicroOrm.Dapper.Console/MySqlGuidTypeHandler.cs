using Dapper;
using System;
using System.Data;

namespace MicroOrm.Dapper.Console
{
    public class MySqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }

        public override Guid Parse(object value)
        {
            return value != null ? new Guid((string)value) : default(Guid);
        }
    }
}
