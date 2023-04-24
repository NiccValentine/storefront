namespace StoreFront.Common
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class Settings
    {
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(Settings._connectionString))
                {
                    Settings.LoadSettings();
                }
                return Settings._connectionString;
            }
        }

        public static string ConnectionStringEF
        {
            get
            {
                if (string.IsNullOrEmpty(Settings._connectionStringEF))
                {
                    Settings.LoadSettings();
                }
                return Settings._connectionStringEF;
            }
        }

        public static bool UsingEF
        {
            get
            {
                if (string.IsNullOrEmpty(Settings._connectionStringEF))
                {
                    Settings.LoadSettings();
                }
                return Settings._usingEF;
            }
        }
        private static string _connectionString { get; set; }

        private static string _connectionStringEF { get; set; }

        private static bool _usingEF { get; set; }

        private static void LoadSettings()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            Settings._connectionString = configuration["ConnectionString"];
            Settings._connectionStringEF = configuration["ConnectionStringEF"];
            Settings._usingEF = Convert.ToBoolean(configuration["UsingEF"]);
        }
    }
}
