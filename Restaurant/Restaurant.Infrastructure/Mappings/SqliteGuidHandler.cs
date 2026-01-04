using Dapper;
using System;
using System.Data;

namespace Restaurant.Infrastructure.Mappings
{
    internal class SqliteGuidHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
            parameter.DbType = DbType.String;
        }

        public override Guid Parse(object value)
        {
            return value switch
            {
                Guid g => g,
                string s => Guid.Parse(s),
                byte[] b => new Guid(b),
                _ => throw new DataException($"Cannot convert {value.GetType()} to Guid")
            };
        }
    }
}
