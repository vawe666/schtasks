using ConsoleApplication.SqlClient;

namespace ConsoleApplication
{
    /// <summary>
    /// ���ݷ��ʹ���
    /// </summary>
    public sealed class DataAccessFactory
    {
        /// <summary>
        /// �������ݷ���ʵ�� Create an SQLServer dataaccess
        /// </summary>
        /// <param name="connString">���ݿ����ӣ����ã�</param>
        /// <returns></returns>
        public static SqlDataAccess CreateSqlDataAccess(string connString)
        {
            return new SqlDataAccess(connString);
        }

        /// <summary>
        /// �������ݷ���ʵ�� Create an SQLServer dataaccess
        /// </summary>
        /// <param name="dp">���ݿ����ӣ����ã�</param>
        /// <returns></returns>
        public static SqlDataAccess CreateSqlDataAccess(DatabaseProperty dp)
        {
            return new SqlDataAccess(dp.ConnectionString);
        }

    }
}