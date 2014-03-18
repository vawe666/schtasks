namespace ConsoleApplication
{
    /// <summary>
    /// 数据库属性类
    /// </summary>
    public struct DatabaseProperty
    {
        private string m_ConnectionString;

        /// <summary>
        /// 数据库链接
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }

        private DatabaseType m_DatabaseType;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType
        {
            get
            {
                return m_DatabaseType;
            }
            set
            {
                m_DatabaseType = value;
            }
        }

        /// <summary>
        /// 构造空数据库属性类实例
        /// </summary>
        public static DatabaseProperty Empty
        {
            get
            {
                return new DatabaseProperty();
            }
        }
    }
}
