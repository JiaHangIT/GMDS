using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JiaHang.Projects.Admin.Common
{
    public static class AppConfig
    {
        private static IConfigurationRoot config => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, false).Build();

        public static string getconnecting(string str) => config.GetConnectionString(str);

        public static string getsection(string str1, string str2) => config[$"{str1}:{str2}"];
    }
}
