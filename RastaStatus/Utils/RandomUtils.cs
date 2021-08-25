using System;
using System.Text;

namespace RastaStatus.Utils
{
    public interface RandomUtils
    {
        public static string GenRandomString(int lenght) {

            string AlphaNumericString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789";

            Random random = new Random();

            StringBuilder sb = new StringBuilder(lenght);
            for (int i = 0; i < lenght; i++) {
                int index = random.Next(0, AlphaNumericString.Length);

                sb.Append(AlphaNumericString.ToCharArray()[index]);
            }
            
            return sb.ToString();
        }
    }
}