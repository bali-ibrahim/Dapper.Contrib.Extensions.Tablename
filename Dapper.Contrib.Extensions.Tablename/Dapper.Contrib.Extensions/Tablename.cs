using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dapper.Contrib.Extensions
{
    public static class Tablename
    {
        private static TableNameConfig _config;
        public static void ReadTablenamesFromConfig(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<TableNameConfig>(configSection);
            _config = configSection.Get<TableNameConfig>();
            SqlMapperExtensions.TableNameMapper = TableName;
        }

        private static string TableName(Type type) => _config.TableNames[type.FullName].Replace("`", "");
        public static string TableName<T>() => TableName(typeof(T));
    }
}
