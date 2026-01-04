using Dapper;
using System;
using System.Data;

namespace Restaurant.Infrastructure.Mappings
{
    public class DecimalHandler : SqlMapper.TypeHandler<decimal>
    {
        public override decimal Parse(object value)
        {
            if (value is decimal d) return d;
            if (value is double db) return (decimal)db;
            if (value is float f) return (decimal)f;
            if (value is long l) return l;
            if (value is int i) return i;
            if (value is string s) return decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            return Convert.ToDecimal(value);
        }

        public override void SetValue(IDbDataParameter parameter, decimal value)
        {
            parameter.Value = value;
        }
    }
}
