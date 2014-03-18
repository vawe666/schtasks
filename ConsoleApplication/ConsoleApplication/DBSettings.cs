using System.Configuration;

namespace ConsoleApplication
{
    /// <summary>
    /// Web.Config数据库链接参数设置
    /// </summary>
    public sealed class DBSettings
    {
         #region 得到数据库链接设置
        /// <summary>
        /// 得到数据库链接设置
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <returns></returns>
        public static DatabaseProperty GetDatabaseProperty(string name)
        {
            DatabaseProperty dp = new DatabaseProperty();
            dp.DatabaseType = DatabaseType.MSSQLServer;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings == null)
            {
                dp.ConnectionString = string.Empty;
            }
            else
            {                
                dp.ConnectionString = settings.ConnectionString;
                if (settings.ProviderName == "System.Data.SqlClient")
                {
                    dp.DatabaseType = DatabaseType.MSSQLServer;
                }
            }

            return dp;
        }
        #endregion

  
    }
    public sealed class StrAppSetting
    {

        public static string GetAppSettingStr(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}