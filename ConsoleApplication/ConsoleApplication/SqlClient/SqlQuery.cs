using System.Data;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    /// MSSql命令类
    /// </summary>
    public class SqlQuery
    {
        #region Private properties

        /// <summary>
        /// Default command timeout.
        /// </summary>
        protected const int COMMAND_TIMEOUT_DEFAULT = 30;
        /// <summary>
        /// Comamnd text.
        /// </summary>
        protected string m_CommandText;
        /// <summary>
        /// Command timeout.
        /// </summary>
        protected int m_CommandTimeout = COMMAND_TIMEOUT_DEFAULT;
        /// <summary>
        /// Command type.
        /// </summary>
        protected CommandType m_CommandType = CommandType.Text;
        /// <summary>
        /// parameters
        /// </summary>
        protected SqlQueryParameterCollection m_Parameters = new SqlQueryParameterCollection();

        #endregion

        #region Public properties

        /// <summary>
        /// MSSql命令
        /// </summary>
        public virtual string CommandText
        {
            set
            {
                this.m_CommandText = value;
            }
            get
            {
                return this.m_CommandText;
            }
        }

        /// <summary>
        /// MSSql命令超时时限
        /// </summary>
        public int CommandTimeout
        {
            set
            {
                if (value < 10)
                    this.m_CommandTimeout = COMMAND_TIMEOUT_DEFAULT;
                else
                    this.m_CommandTimeout = value;
            }
            get
            {
                return this.m_CommandTimeout;
            }
        }

        /// <summary>
        /// MSSql命令类型
        /// </summary>
        public CommandType CommandType
        {
            set
            {
                this.m_CommandType = value;
            }
            get
            {
                return this.m_CommandType;
            }
        }

        /// <summary>
        /// MSSql命令参数
        /// </summary>
        public SqlQueryParameterCollection Parameters
        {
            set
            {
                this.m_Parameters = value;
            }
            get
            {
                return this.m_Parameters;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造
        /// </summary>
        public SqlQuery()
        {

        }

        #endregion
    }
}