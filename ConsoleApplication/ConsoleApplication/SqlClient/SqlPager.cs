using System;
using System.Data;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    ///  MSSql分页类
    /// </summary>
    /// <example>
    ///	SqlPager p = new SqlPager();
    ///	p.DatabaseProperty = Application.DefaultDatabaseProperty;
    ///	p.CommandText = CmmandText;
    ///	p.KeyField = KeyField;
    ///	p.PageSize = PageSize;
    ///	p.PageNumber = PageNumber;
    ///	p.PagingType = PagingType;
    ///	p.Execute();
    /// ds = p.Data; 
    ///</example>
    public sealed class SqlPager
    {
        #region Private properties
        private int m_PageSize;
        private int m_PageNumber;
        private int m_Timeout;
        private int m_PageCount;
        private int m_RecordCount;

        private bool m_IsExecuted;

        private string m_KeyField;
        private string m_CommandText;

        private SqlQueryParameterCollection m_Parameters = new SqlQueryParameterCollection(); 

        private string m_ConnectionString;
        private PagingType m_PagingType;

        private DataSet m_Data;
        private SqlDataAccess m_DataAccess;
        #endregion

        #region Public Properties

        /// <summary>
        /// SqlQueryParameter参数集
        /// </summary>
        public SqlQueryParameterCollection Parameters
        {
            get
            {
                return this.m_Parameters;
            }
        }
        /// <summary>
        /// Sets or retrieves the paging logic.
        /// </summary>
        public PagingType PagingType
        {
            set
            {
                this.m_PagingType = value;
            }
            get
            {
                return this.m_PagingType;
            }
        }

        /// <summary>
        /// Retrieves single page data.
        /// </summary>
        public DataSet Data
        {
            get
            {
                if (!m_IsExecuted) throw new ApplicationException("Pager is not executed!");
                return this.m_Data;
            }
        }

        /// <summary>
        /// Sets or retrieves ConnectionString.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
            set
            {
                this.m_ConnectionString = value;
            }
        }

        /// <summary>
        /// Sets or retrieves KeyField.
        /// Used to identify each row, usually is the primary key.
        /// </summary> 
        public string KeyField
        {
            get
            {
                return this.m_KeyField;
            }
            set
            {
                this.m_KeyField = value.ToUpper();
            }
        }

        /// <summary>
        /// Retrieves RecordCount.
        /// </summary>
        public int RecordCount
        {
            get
            {
                if (!m_IsExecuted) throw new ApplicationException("Pager is not executed!");
                return this.m_RecordCount;
            }
        }

        /// <summary>
        /// Retrieves PageCount.
        /// </summary>
        public int PageCount
        {
            get
            {
                if (!m_IsExecuted) throw new ApplicationException("Pager is not executed!");
                return this.m_PageCount;
            }
        }

        /// <summary>
        /// Sets or Retrieves PageSize.
        /// The record number shows in each page.
        /// </summary>
        public int PageSize
        {
            get
            {
                return this.m_PageSize;
            }
            set
            {
                this.m_PageSize = value;
            }
        }

        /// <summary>
        /// Sets or Retrieves PageNumber.
        /// Page Number
        /// </summary>
        public int PageNumber
        {
            get
            {
                return this.m_PageNumber;
            }
            set
            {
                this.m_PageNumber = value;
            }
        }

        /// <summary>
        /// Sets or Retrieves Timeout.
        /// </summary>
        public int Timeout
        {
            get
            {
                return this.m_Timeout;
            }
            set
            {
                this.m_Timeout = value;
            }
        }

        /// <summary>
        /// CommandText
        /// </summary>
        public string CommandText
        {
            get
            {
                return this.m_CommandText;
            }
            set
            {
                this.m_CommandText = value.ToUpper().Trim();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 构造
        /// </summary>
        public SqlPager()
        {
            this.m_Timeout = 30;
            this.m_PagingType = PagingType.ReverseOrder;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public SqlPager(string commandText, string connStr)
            : this()
        {
            this.m_ConnectionString = connStr;
            this.m_CommandText = commandText;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            CheckValue();

            m_DataAccess = DataAccessFactory.CreateSqlDataAccess(this.m_ConnectionString);
            m_DataAccess.Open();
            try
            {
                this.m_RecordCount = GetRecordCount();
                this.m_PageCount = m_RecordCount / m_PageSize + ((m_RecordCount % m_PageSize == 0) ? 0 : 1);
                if (this.m_PageNumber > this.m_PageCount)
                    this.m_PageNumber = this.m_PageCount > 0 ? this.m_PageCount : 1;
                this.m_Data = GetPageData();
                m_IsExecuted = true;
            }
            finally
            {
                if (!m_DataAccess.IsClosed)
                    m_DataAccess.Close();
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// 检查SQL参数值
        /// </summary>
        private void CheckValue()
        {
            if( string.IsNullOrEmpty( this.m_CommandText ) )
                throw new ApplicationException( "CommandText should not be null or empty!" );

            if( string.IsNullOrEmpty( this.m_ConnectionString ) )
                throw new ApplicationException( "DatabaseProperty should not be null or empty!" );

            if( string.IsNullOrEmpty( this.m_KeyField ) )
                throw new ApplicationException( "KeyField should not be null or empty!" );

            if( this.m_PageSize < 1 )
                throw new ApplicationException( "PageSize should larger than zero!" );

            if( this.m_PageNumber < 1 )
                this.m_PageNumber = 1;
        }

        /// <summary>
        /// 得到分页数据
        /// </summary>
        /// <returns>DataSet</returns>
        private DataSet GetPageData()
        {
            SqlPagingQuery q = new SqlPagingQuery(
                this.m_CommandText,
                this.m_KeyField,
                this.m_PageSize,
                this.m_PageNumber,
                this.m_PageCount,
                this.m_RecordCount,
                this.m_PagingType);

            q.Parameters = this.m_Parameters;

            if (this.m_Timeout > 0)
                q.CommandTimeout = this.m_Timeout;

            //string cmd = q.CommandText;

            return m_DataAccess.ExecuteDataset(q);
        }

        /// <summary>
        /// 得到记录集数量统计
        /// </summary>
        /// <returns>RecordCount</returns>
        private int GetRecordCount()
        {
            int posFrom = this.m_CommandText.IndexOf(" FROM ");
            int posOrder = this.m_CommandText.IndexOf(" ORDER ");

            SqlQuery q = new SqlQuery();
            if (posOrder > 0)
                q.CommandText = "SELECT COUNT(1) " + this.m_CommandText.Substring(posFrom, posOrder - posFrom).Trim();
            else
                q.CommandText = "SELECT COUNT(1) " + this.m_CommandText.Substring(posFrom);

            q.Parameters = this.m_Parameters;

            Object recordCount = m_DataAccess.ExecuteScalar(q);

            return (int)recordCount;
        }
        #endregion
    }
}

