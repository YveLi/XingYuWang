namespace AstWeb.Common.Configuration
{
    public class WebConfigApplicationSettings
    {
        public static string GetAppSettings(string key, string defaultVal = "")
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            return value == null ? defaultVal : value;
        }
    }
}
