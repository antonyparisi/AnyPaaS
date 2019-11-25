using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.storagecsp.Library.Helper
{
    public static class StringExtention
    {
        public static void CheckString(this string i, string inputString, string messageException)
        {
            if (String.IsNullOrEmpty(inputString.Trim()))
            {
                throw new ArgumentNullException(messageException);
            }
        }
    }
}
