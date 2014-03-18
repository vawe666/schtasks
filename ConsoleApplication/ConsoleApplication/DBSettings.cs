using System.Configuration;

namespace ConsoleApplication
{
    /// <summary>
    /// Web.Config���ݿ����Ӳ�������
    /// </summary>
    public sealed class DBSettings
    {
         #region �õ����ݿ���������
        /// <summary>
        /// �õ����ݿ���������
        /// </summary>
        /// <param name="name">�ڵ�����</param>
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