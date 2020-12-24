using Connection.Identifier.Quoting;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Reflection;
using SystemTableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace Dapper.Contrib.Extensions.Tablename
{
    public static class TablenameExtensions
    {
        private static TablenameConfig _config;
        public static IServiceCollection ReadTablenamesFromConfig(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<TablenameConfig>(configSection);
            _config = configSection.Get<TablenameConfig>();
            SqlMapperExtensions.TableNameMapper = Tablename;
            return services;
        }

        private static string Tablename(Type type) => _config.TableNames[type.FullName];

        public static string Tablename<T>(this IDbConnection connection)
        {
            var type = typeof(T);
            var tableName = null
                ?? type.GetCustomAttribute<TableAttribute>()?.Name
                ?? type.GetCustomAttribute<SystemTableAttribute>()?.Name
                ?? Tablename(type)
            ;
            tableName = tableName.QuoteIdentifier(connection);
            return tableName;
        }
    }
}
