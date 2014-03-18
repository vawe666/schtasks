namespace ConsoleApplication
{
    /// <summary>
    /// ���ݿ�������
    /// </summary>
    public struct DatabaseProperty
    {
        private string m_ConnectionString;

        /// <summary>
        /// ���ݿ�����
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
        /// ���ݿ�����
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
        /// ��������ݿ�������ʵ��
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
