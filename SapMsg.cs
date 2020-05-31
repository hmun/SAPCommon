using System;
using System.Collections.Generic;
using System.Text;

namespace SAPCommon
{
    public static class SapMsg
    {
        public static String[] ToArray(String msg)
        {
            String[] separator = { ";" };
            String[] ret = msg.Split(separator,
                   StringSplitOptions.RemoveEmptyEntries);
            return ret;
        }
    }

}
