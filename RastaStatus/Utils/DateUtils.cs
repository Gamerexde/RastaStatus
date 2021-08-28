using System;

namespace RastaStatus.Utils
{
    public class DateUtils
    {
        public static string MysqlDateFormat = "yyyy-MM-dd HH:mm:ss";
        public static string GenDatetime()
        {
            DateTime dateTime = DateTime.Now;
            
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}