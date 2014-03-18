using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    /// MSSql数据库访问类
    /// </summary>
    public sealed class SqlDataAccess
    {

        #region Private properties

        /// <summary>
        /// 默认命令超时时限
        /// Default command timeout
        /// </summary>
        private const int DEF_CMD_TIMEOUT = 10;
        /// <summary>
        /// 默认命令超时时限
        /// </summary>
        private int m_CommandTimeOut = DEF_CMD_TIMEOUT;
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private SqlConnection m_Connection;
        /// <summary>
        /// 数据库事务
        /// </summary>
        private SqlTransaction m_Trans;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造.
        /// </summary>
        private SqlDataAccess()
        {

        }

        /// <summary>
        /// 构造.
        /// </summary>
        internal SqlDataAccess(SqlConnection conn)
            : this()
        {
            m_Connection = conn;
        }

        /// <summary>
        /// 构造.
        /// </summary>
        internal SqlDataAccess(string connectionString)
            : this()
        {
            m_Connection = new SqlConnection(connectionString);
        }

        #endregion

        /// <summary>
        /// 数据库链接字符串.
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return m_Connection;
            }
        }

        /// <summary>
        /// 启用数据库事务
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            m_Trans = m_Connection.BeginTransaction();
            return m_Trans;
        }

        /// <summary>
        /// 打开链接
        /// Open a connection
        /// </summary>
        public void Open()
        {
            if (IsClosed)
                Connection.Open();
        }
        /// <summary>
        /// 关闭链接
        /// Close the opened connection
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// 链接是否已经关闭
        /// Whether the connection is closed.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return Connection.State.Equals(ConnectionState.Closed);
            }
        }

        /// <summary>
        /// 命令操作超时
        /// Command execute timeout.
        /// </summary>
        public int CommandTimeOut
        {
            get
            {
                return m_CommandTimeOut;
            }

            set
            {
                m_CommandTimeOut = value;
            }
        }

        /// <summary>
        /// 执行命令并返回DataSet
        /// Execute query and return DataSet.
        /// </summary>
        public DataSet ExecuteDataset(SqlQuery q)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, q);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            SyncParameter(cmd, q);

            cmd.Parameters.Clear();

            return ds;
        }

        /// <summary>
        /// 执行命令并返回DataReader
        /// </summary>
        public IDataReader ExecuteReader(SqlQuery q)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, q);

            IDataReader reader = cmd.ExecuteReader();

            SyncParameter(cmd, q);

            cmd.Parameters.Clear();

            return reader;
        }

        /// <summary>
        /// 执行命令并返回首行首列内容(Object)
        /// </summary>
        public Object ExecuteScalar(SqlQuery q)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, q);

            Object oValue = cmd.ExecuteScalar();

            SyncParameter(cmd, q);

            cmd.Parameters.Clear();

            return oValue;
        }

        /// <summary>
        /// 执行命令并返回影响行数
        /// </summary>
        public int ExecuteNonQuery(SqlQuery q)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, q);

            int affectRows = cmd.ExecuteNonQuery();

            SyncParameter(cmd, q);

            cmd.Parameters.Clear();

            return affectRows;
        }

        /// <summary>
        ///执行命令并返回Xml文章
        /// </summary>
        public XmlDocument ExecuteXmlDoc(SqlQuery q)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行命令并返回XmlReader
        /// </summary>
        public XmlReader ExecuteXmlReader(SqlQuery q)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, q);

            XmlReader xmlReader = cmd.ExecuteXmlReader();

            SyncParameter(cmd, q);

            cmd.Parameters.Clear();

            return xmlReader;
        }

        /// <summary>
        /// 构造Sql命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="q"></param>
        private void PrepareCommand(SqlCommand cmd, SqlQuery q)
        {
            cmd.CommandType = q.CommandType;
            cmd.CommandText = q.CommandText;
            cmd.Connection = m_Connection;
            cmd.CommandTimeout = q.CommandTimeout;
            cmd.Transaction = m_Trans;

            if (q.Parameters.Count > 0)
            {
                int parameterNum = q.Parameters.Count;
                for (int i = 0; i < parameterNum; i++)
                {
                    cmd.Parameters.Add(q.Parameters[i]);
                }
            }
        }

        /// <summary>
        /// SyncParameter
        /// </summary>
        private void SyncParameter(SqlCommand cmd, SqlQuery q)
        {
            //int parameterCount = q.Parameters.Count;
            //if (parameterCount > 0)
            //{
            //    for (int i = 0; i < parameterCount; i++)
            //    {
            //        q.Parameters[i].SyncParameter();
            //    }

            //}
        }



        #region Static methods
        /// <summary>
        /// 执行命令并返回Dataset
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns>Dataset</returns>
        public static DataSet ExecuteDataset(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            DataSet ds = da.ExecuteDataset(q);
            da.Close();

            return ds;
        }

        /// <summary>
        /// 执行命令并返回影响行数
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            int affectRows = da.ExecuteNonQuery(q);
            da.Close();

            return affectRows;
        }

        /// <summary>
        /// 执行命令并返回DataReader
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            return da.ExecuteReader(q);

        }

        /// <summary>
        /// 执行命令并返回首行首列的Object
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns>首行首列的Object</returns>
        public static Object ExecuteScalar(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            object scalar = da.ExecuteScalar(q); 
            da.Close();

            return scalar;
        }

        /// <summary>
        /// 执行命令并返回XmlReader
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns>XmlReader</returns>
        public static XmlReader ExecuteXmlReader(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            return da.ExecuteXmlReader(q);
        }

        /// <summary>
        /// 执行命令并返回Xml文档
        /// </summary>
        /// <param name="q">SqlQuery命令</param>
        /// <param name="dp">Sql链接配置</param>
        /// <returns>Xml文档</returns>
        public static XmlDocument ExecuteXmlDoc(SqlQuery q, DatabaseProperty dp)
        {
            SqlDataAccess da = new SqlDataAccess(dp.ConnectionString);
            da.Open();
            XmlDocument doc = da.ExecuteXmlDoc(q);
            da.Close();

            return doc;
        }
        #endregion
    }
}
