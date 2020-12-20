using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Dapper.Contrib.Extensions.Tablename
{
    public static class TablenameExtensions
    {
        public static HashSet<string> whitelist;
        private static TablenameConfig _config;
        public static IServiceCollection ReadTablenamesFromConfig(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<TablenameConfig>(configSection);
            _config = configSection.Get<TablenameConfig>();
            SqlMapperExtensions.TableNameMapper = TableName;
            return services;
        }

        private static string TableName(Type type)
        {
            var tableName = _config.TableNames[type.FullName];
            if (whitelist != null)
            {
                return whitelist.Contains(tableName) ? tableName : throw new Exception($"The tablename {tableName} is not whitelisted!");
            }
            else
            {
                return tableName;
            }
        }

        public static string TableName<T>() => TableName(typeof(T));
    }
}
