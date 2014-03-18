using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    /// Sql筛选命令类
    /// </summary>
    internal class SqlSelectSql
    {
        #region Private properties

        private string m_OriginalSql;
        private string m_Table;
        private string m_Conditions;

        private List<string> m_Fields = new List<string>();
        private Dictionary<string, string> m_Orders = new Dictionary<string, string>();

        #endregion

        #region Public properties
        /// <summary>
        /// 表名
        /// </summary>
        public string Table
        {
            get
            {
                return this.m_Table;
            }
        }

        /// <summary>
        /// 字段（集）
        /// exp: id,name,sex,tel
        /// </summary>
        public string Fields
        {
            get
            {
                string fieldClause = string.Empty;
                for( int i = 0; i < this.m_Fields.Count; i++ )
                {
                    fieldClause += this.m_Fields[i].ToString() + ",";
                }
                return fieldClause.TrimEnd( ',' );
            }
        }

        /// <summary>
        /// 排序字段（集）
        /// exp: id desc,hit asc
        /// </summary>
        public string OrderFields
        {
            get
            {
                string orderFields = string.Empty;

                foreach( string field in this.m_Orders.Keys )
                {
                    orderFields += field + ",";
                }
                return orderFields.TrimEnd( ',' );
            }
        }

        /// <summary>
        /// 是否存在排序子句
        /// </summary>
        public bool HasOrderByClause
        {
            get
            {
                return this.m_Orders.Count > 0 ? true : false;
            }
        }

        /// <summary>
        /// 是否选择（显示）所有的字段
        /// </summary>
        private bool IsAllFieldSelected
        {
            get
            {
                bool isAllFieldSelected = false;
                if( this.m_Fields.Count == 1 )
                {
                    if( this.m_Fields[0].ToString() == "*" )
                    {
                        isAllFieldSelected = true;
                    }
                }
                return isAllFieldSelected;
            }
        }

        /// <summary>
        /// 是否存在条件子句
        /// </summary>
        public bool HasWhereClause
        {
            get
            {
                return this.m_Conditions == null ? false : true;
            }
        }

        /// <summary>
        /// 条件子句
        /// </summary>
        public string WhereClause
        {
            get
            {
                if( this.HasWhereClause )
                    return " WHERE " + this.m_Conditions;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 排序子句
        /// </summary>
        public string OrderByClause
        {
            get
            {
                if( this.HasOrderByClause )
                {
                    string orderByClause = " ORDER BY ";

                    foreach( KeyValuePair<string, string> kvPair in this.m_Orders )
                    {
                        orderByClause += kvPair.Key + " " + kvPair.Value + ",";
                    }

                    return orderByClause.TrimEnd( ',' );
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 构造排序子句
        /// </summary>
        public string ReverseOrderByClause
        {
            get
            {
                string orderByClause = " ORDER BY ";

                foreach( KeyValuePair<string, string> kvPair in this.m_Orders )
                {
                    orderByClause += kvPair.Key + " " + ( ( kvPair.Value == "ASC" ) ? "DESC" : "ASC" ) + ",";
                }

                return orderByClause.TrimEnd( ',' );
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 构造
        /// </summary>
        public SqlSelectSql( string sql )
        {
            // Check...
            if( string.IsNullOrEmpty( sql ) )
                throw new ArgumentException( "Sql should not be null or empty!", "sql" );

            // Remove unwanted blanks...
            this.m_OriginalSql = Regex.Replace( sql.ToUpper().Trim(), "[ ]+", " " ).Replace( "\t", string.Empty );

            this.ParseSql();
        }
        #endregion

        #region Public functions
        /// <summary>
        /// 检查是否有排序字段存在于条件字段中
        /// Check weather a field contains in the order by clause.
        /// </summary>
        public bool IsFieldContainsInOrderByClause( string field )
        {
            return this.m_Orders.ContainsKey( GetPureField( field ) );
        }

        /// <summary>
        /// 添加一个排序子句
        /// Add an order to the order by clause.
        /// </summary>
        public void AddOrderBy( string field, string order )
        {
            this.m_Orders.Add( GetPureField( field ), order );
        }
        #endregion

        #region Private functions
        /// <summary>
        /// 构建SQL命令
        /// </summary>
        private void ParseSql()
        {
            int startPos = 0;
            int endPos = 0;
            string substring = null;

            // Get fields...
            startPos = 7;
            endPos = this.m_OriginalSql.IndexOf( " FROM ", 7 );
            ParseFields( this.m_OriginalSql.Substring( startPos, endPos - startPos ) );

            substring = this.m_OriginalSql.Substring( endPos + 6 );
            endPos = substring.IndexOf( " WHERE " );
            if( endPos > 0 ) // Has 'WHERE'.
            {
                this.m_Table = substring.Substring( 0, endPos );
                startPos = endPos + 7;
                endPos = substring.IndexOf( " ORDER BY " );
                if( endPos > 0 ) // Has 'ORDER BY'.
                {
                    ParseConditions( substring.Substring( startPos, endPos - startPos ) );
                    ParseOrders( substring.Substring( endPos + 10 ) );
                }
                else
                {
                    ParseConditions( substring.Substring( startPos ) );
                }
            }
            else // NO 'WHERE'
            {
                endPos = substring.IndexOf( " ORDER BY " );
                if( endPos > 0 ) // Has 'ORDER BY'
                {
                    this.m_Table = substring.Substring( 0, endPos );
                    ParseOrders( substring.Substring( endPos + 10 ) );
                }
                else
                {
                    this.m_Table = substring;
                }
            }
        }

        /// <summary>
        /// 构建条件
        /// </summary>
        /// <param name="conditions">条件子句</param>
        private void ParseConditions( string conditions )
        {
            this.m_Conditions = conditions;
        }

        /// <summary>
        /// 构建排序
        /// </summary>
        /// <param name="orders">排序子句</param>
        private void ParseOrders( string orders )
        {
            string[] orderArray = orders.Split( ',' );
            foreach( string sOrder in orderArray )
            {
                string strOrder = sOrder.Trim();
                if( strOrder.EndsWith( " ASC" ) )
                {
                    this.m_Orders.Add( GetPureField( strOrder.Substring( 0, strOrder.Length - 4 ) ), "ASC" );
                }
                else if( strOrder.EndsWith( " DESC" ) )
                {
                    this.m_Orders.Add( GetPureField( strOrder.Substring( 0, strOrder.Length - 5 ) ), "DESC" );
                }
                else
                {
                    this.m_Orders.Add( strOrder, "ASC" );
                }
            }
        }

        /// <summary>
        /// 构建（显示）字段子句
        /// </summary>
        /// <param name="fields">字段子句</param>
        private void ParseFields( string fields )
        {
            string[] fieldArray = fields.Split( ',' );
            for( int i = 0; i < fieldArray.Length; i++ )
            {
                this.m_Fields.Add( GetPureField( fieldArray[i].Trim() ) );
            }
        }

        /// <summary>
        /// 得到剔除了空格、小句号，中括号的字段名
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        private string GetPureField( string field )
        {
            string pureField = field;
            int lastDotPos = pureField.LastIndexOf( '.' );
            if( lastDotPos > 0 )
            {
                pureField = pureField.Substring( lastDotPos + 1 );
            }
            return pureField.Trim( ' ', '[', ']' );
        }

        #endregion

    }
}
