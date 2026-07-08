using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Helpers
    {
        public static string GetPaddedNum(int num)
        {
            if(num < 10)
            {
                return "000" + num.ToString();
            }
            if(num < 100)
            {
                return "00" + num.ToString();
            }
            if(num < 1000)
            {
                return "0" + num.ToString();
            }
            return num.ToString();
        }
        public static string GetPaddedNum(string num)
        {
            return GetPaddedNum(int.Parse(num));
        }
    }
}
