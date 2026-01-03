using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Restaurant.Infrastructure.Mappings
{
    internal static class Extensions
    {
        public static IServiceCollection ApplyMappings(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new SqliteGuidHandler());
            SqlMapper.AddTypeHandler(new EmailHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            return services;
        }
    }
}
