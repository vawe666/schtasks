using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// SQLServer
        /// </summary>
        MSSQLServer = 0,
        /// <summary
        /// >Oracle
        /// </summary>
        Oracle = 1,
        /// <summary>
        /// OleDB
        /// </summary>
        OleDB = 2,
        /// <summary>
        /// Odbc
        /// </summary>
        Odbc = 3,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 4,
    }
}
