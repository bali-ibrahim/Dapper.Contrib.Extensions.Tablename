using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dapper.Contrib.Extensions.Tablename
{
    public static class TablenameExtensions
    {
        private static TablenameConfig _config;
        public static IServiceCollection ReadTablenamesFromConfig(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<TablenameConfig>(configSection);
            _config = configSection.Get<TablenameConfig>();
            SqlMapperExtensions.TableNameMapper = TableName;
            return services;
        }

        private static string TableName(Type type) => _config.TableNames[type.FullName].Replace("`", "");
        public static string TableName<T>() => TableName(typeof(T));
    }
}
