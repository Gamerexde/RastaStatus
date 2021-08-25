using System;

namespace RastaStatus.Utils
{
    public interface DateUtils
    {
        public static string GenDatetime()
        {
            DateTime dateTime = DateTime.Now;

            string formated = dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day + " " +
                              dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
            return formated;
        }
    }
}