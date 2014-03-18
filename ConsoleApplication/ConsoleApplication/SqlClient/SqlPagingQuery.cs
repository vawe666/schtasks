using System;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    /// MSSql��ҳ������
    /// </summary>
    public sealed class SqlPagingQuery : SqlQuery
    {
        #region Private properties

        private int m_PageSize;
        private int m_PageNumber;
        private int m_RecordCount;
        private int m_PageCount;

        private string m_KeyField;

        private SqlSelectSql m_SQL;

        private PagingType m_PagingType = PagingType.NotInclude;

        #endregion

        #region Public Properties

        /// <summary>
        /// ҳͳ�� PageCount
        /// </summary>
        public int PageCount
        {
            set
            {
                this.m_PageCount = value;
            }
            get
            {
                return this.m_PageCount;
            }
        }

        /// <summary>
        /// ��¼ͳ�� RecordCount
        /// </summary>
        public int RecordCount
        {
            set
            {
                this.m_RecordCount = value;
            }
        }

        /// <summary>
        /// ��ҳ���� PagingType
        /// </summary>
        public PagingType PagingType
        {
            get
            {
                return this.m_PagingType;
            }
            set
            {
                this.m_PagingType = value;
            }
        }

        /// <summary>
        /// ÿ�еĹؼ��ֶΣ�ͨ����������
        /// The field to identify each rows, usually is the primary key.
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
        /// ÿҳ��С
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
        /// ҳ��
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
        /// SQL����
        /// The command used to achieve single page data,You can simply set it to your 
        /// original command without paging logic, and when you retrieve this property you
        /// will get the command with paging logic.
        /// </summary>
        public override string CommandText
        {
            get
            {
                return GenerateCmdText();
            }
            set
            {
                this.m_CommandText = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// SQL��ҳ����
        /// User NotInclude logic to build the paging query command.
        /// </summary>
        public SqlPagingQuery(string cmdText, string keyField, int pageSize, int pageNumber)
            : this(cmdText, keyField, pageSize, pageNumber, 0, 0, PagingType.NotInclude)
        {
        }

        /// <summary>
        /// SQL��ҳ����
        /// User ReverseOrder logic to build the paging query command.
        /// </summary>
        public SqlPagingQuery(string cmdText, string keyField, int pageSize, int pageNumber, int pageCount, int recordCount)
            : this(cmdText, keyField, pageSize, pageNumber, pageCount, recordCount, PagingType.ReverseOrder)
        {
        }

        /// <summary>
        /// SQL��ҳ����
        /// PagingQuery
        /// </summary>
        public SqlPagingQuery(string cmdText, string keyField, int pageSize, int pageNumber, int pageCount, int recordCount, PagingType pagingType)
        {
            this.CommandText = cmdText;
            this.KeyField = keyField;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.PageCount = pageCount;
            this.PagingType = pagingType;
            this.RecordCount = recordCount;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Gernerate page sql command
        /// </summary>
        private string GenerateCmdText()
        {
            string cmdText = null;
            this.m_SQL = new SqlSelectSql(this.m_CommandText);
            switch (m_PagingType)
            {
                case PagingType.NotInclude:
                    cmdText = GenerateNotIncludePagingCmdText();
                    break;
                case PagingType.ReverseOrder:
                    cmdText = GenerateReverseOrderByPagingCmdText();
                    break;
            }
            return cmdText;
        }

        private string GenerateFirstPagePagingCmdText()
        {
            System.Text.StringBuilder cmdText = new System.Text.StringBuilder();
            cmdText.Append("SELECT TOP ").Append(m_PageSize).Append(" ").Append(this.m_SQL.Fields);
            cmdText.Append(" FROM ").Append(this.m_SQL.Table);
            cmdText.Append(this.m_SQL.WhereClause);
            cmdText.Append(this.m_SQL.OrderByClause);
            return cmdText.ToString();
        }

        private string GenerateNotIncludePagingCmdText()
        {
            if (this.m_PageNumber == 1)
            {
                return GenerateFirstPagePagingCmdText();
            }
            else
            {
                System.Text.StringBuilder cmdText = new System.Text.StringBuilder();
                cmdText.Append("SELECT TOP ").Append(m_PageSize).Append(" ").Append(this.m_SQL.Fields);
                cmdText.Append(" FROM ").Append(this.m_SQL.Table);
                cmdText.Append(" WHERE ").Append(m_KeyField).Append(" NOT IN (");
                cmdText.Append("SELECT TOP ").Append(m_PageSize * (m_PageNumber - 1)).Append(" ").Append(m_KeyField);
                cmdText.Append(" FROM ").Append(this.m_SQL.Table);
                cmdText.Append(this.m_SQL.WhereClause);
                cmdText.Append(this.m_SQL.OrderByClause);
                cmdText.Append(")");
                cmdText.Append(this.m_SQL.OrderByClause);
                return cmdText.ToString();
            }
        }

        private string GenerateReverseOrderByPagingCmdText()
        {
            if (this.m_RecordCount < 1) throw new ApplicationException("RecordCount should larger than zero!");

            if (!this.m_SQL.IsFieldContainsInOrderByClause(m_KeyField))
                this.m_SQL.AddOrderBy(m_KeyField, "ASC");

            if (this.m_PageNumber == 1)
            {
                return GenerateFirstPagePagingCmdText();
            }
            else
            {
                int topNumber = this.m_PageSize;
                if (this.m_PageNumber >= this.m_PageCount)
                {
                    this.m_PageNumber = this.m_PageCount;
                    topNumber = this.m_RecordCount - (this.m_PageNumber - 1) * this.m_PageSize;
                }

                System.Text.StringBuilder cmdText = new System.Text.StringBuilder();

                cmdText.Append("SELECT ").Append(this.m_SQL.Fields);
                cmdText.Append(" FROM ").Append(this.m_SQL.Table);
                cmdText.Append(" WHERE ").Append(this.m_KeyField).Append(" IN ( ");
                cmdText.Append("SELECT TOP ").Append(topNumber).Append(" ").Append(this.m_KeyField);
                cmdText.Append(" FROM ( ");
                cmdText.Append("SELECT TOP ").Append(m_PageNumber * m_PageSize).Append(" ").Append(this.m_SQL.OrderFields);
                cmdText.Append(" FROM ").Append(this.m_SQL.Table);
                cmdText.Append(this.m_SQL.WhereClause);
                cmdText.Append(this.m_SQL.OrderByClause);
                cmdText.Append(" ) SUBTABLE");
                cmdText.Append(this.m_SQL.ReverseOrderByClause);
                cmdText.Append(" )");
                cmdText.Append(this.m_SQL.OrderByClause);

                return cmdText.ToString();
            }
        }
        #endregion

    }
}
