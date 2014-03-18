using System;
using System.Collections.Generic;
 
using System.Text;

namespace ConsoleApplication.Comp
{
    public class ValueUtil
    {
        public static string AddComma(string[] str)
        {
            string temp = "";
            foreach (string s in str)
            {
                temp = temp + s + ",";
            }
               char[] charsToTrim = { ',', '.', ' ' };
               return temp.TrimEnd(charsToTrim);
        }

        public static bool IsEmpty(string str)
        {
            if (str == string.Empty || str == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int TryInt(string str, int defaultValue)
        {
            int intValue = defaultValue;

            if (!IsEmpty(str))
            {
                try
                {
                    intValue = Convert.ToInt32(decimal.Parse(str));
                }
                catch
                {

                }
            }

            return intValue;
        }

        public static int TryInt(object obj, int defaultValue)
        {
            int intValue = defaultValue;
            if (obj != null)
            {
                try
                {
                    intValue = Convert.ToInt32(obj);
                }
                catch
                {
                }
            }
            return intValue;
        }
        public static string TryString(object obj, string defaultValue)
        {
            if (obj != null)
            {
                return obj.ToString();
            }

            return defaultValue;
        }

        public static string TryString(string str, string defaultValue)
        {
            if (str != null)
            {
                return str;
            }
            return defaultValue;
        }
        

    }
}
