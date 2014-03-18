using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApplication.SqlClient
{
    /// <summary>
    /// MSSql命令参数集合类
    /// </summary>
    public sealed class SqlQueryParameterCollection : MarshalByRefObject
    {
        private int m_IntitialCapacity = 10;
        private ArrayList m_Items;

        /// <summary>
        /// 构造
        /// </summary>
        public SqlQueryParameterCollection()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        public SqlQueryParameterCollection(int initCapacity)
        {
            m_IntitialCapacity = initCapacity;
        }

        /// <summary>
        /// 参数项
        /// </summary>
        private ArrayList Items
        {
            get
            {
                if (this.m_Items == null)
                {
                    this.m_Items = new ArrayList(m_IntitialCapacity);
                }
                return this.m_Items;
            }
        }

        /// <summary>
        /// 参数统计
        /// </summary>
        public int Count
        {
            get
            {
                if (this.m_Items == null)
                {
                    return 0;
                }
                return this.m_Items.Count;

            }
        }


        /// <summary>
        /// 追加一个命令参数
        /// </summary>
        /// <param name="param">SqlParameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter Add(SqlParameter param)
        {
            this.Items.Add(param);
            return param;
        }

        /// <summary>
        /// 追加一个命令参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter Add(string parameterName, object value)
        {
            return this.Add(new SqlParameter(parameterName, value));
        }

        /// <summary>
        /// 追加一个命令参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter Add(string parameterName, object value, SqlDbType dbType)
        {
            SqlParameter para = new SqlParameter(parameterName, value);
            para.SqlDbType = dbType;

            return this.Add(para);
        }

        /// <summary>
        /// 追加一个命令参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public SqlParameter Add(string parameterName, object value, SqlDbType dbType, int size)
        {
            SqlParameter para = new SqlParameter(parameterName, value);
            para.SqlDbType = dbType;
            para.Size = size;

            return this.Add(para);
        }

        /// <summary>
        /// 追加一个命令参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数输入、输出方式</param>
        /// <returns></returns>
        public SqlParameter Add(string parameterName, object value, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter para = new SqlParameter(parameterName, value);
            para.SqlDbType = dbType;
            para.Size = size;
            para.Direction = direction;

            return this.Add(para);
        }


        /// <summary>
        /// 索引号
        /// </summary>
        public SqlParameter this[int index]
        {
            get
            {
                this.RangeCheck(index);
                return ((SqlParameter)this.m_Items[index]);
            }

            set
            {
                this.RangeCheck(index);
                this.m_Items[index] = value;
            }
        }

        /// <summary>
        /// 索引号
        /// </summary>
        public SqlParameter this[string parameterName]
        {
            get
            {
                int num1 = this.RangeCheck(parameterName);
                return ((SqlParameter)this.m_Items[num1]);
            }

            set
            {
                int num1 = this.RangeCheck(parameterName);
                this.m_Items[num1] = value;
            }
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        /// <param name="Value"></param>
        private void ValidateType(object Value)
        {
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="index">索引号</param>
        /// <param name="Value"></param>
        private void Validate(int index, SqlParameter Value)
        { }

        /// <summary>
        /// 索引号范围检查
        /// </summary>
        /// <param name="index">索引号</param>
        private void RangeCheck(int index)
        {
            if ((index < 0) || (this.Count <= index))
            {
                throw new IndexOutOfRangeException("Number " + index.ToString() + " is out of Range");

            }
        }

        /// <summary>
        /// 命令参数范围检查
        /// </summary>
        /// <param name="parameterName">命令参数</param>
        /// <returns></returns>
        private int RangeCheck(string parameterName)
        {
            int num1;
            num1 = this.IndexOf(parameterName);
            if (num1 < 0)
            {
                throw new IndexOutOfRangeException("ParameterName " + parameterName + " dose not exist");
            }
            return num1;
        }

        /// <summary>
        /// 检查是否包含参数
        /// </summary>
        /// <param name="parameterName">命令参数</param>
        /// <returns>索引号</returns>
        public int IndexOf(string parameterName)
        {
            int index = -1;
            if (this.m_Items != null)
            {
                for (int i = 0; i < this.m_Items.Count; i++)
                {
                    if (((SqlParameter)m_Items[i]).ParameterName.Equals(parameterName))
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// 清除SQL参数
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
        }
    }
}
