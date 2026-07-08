using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static void DoMainAction(string action)
        {
            switch(action)
            {
                case "A":
                    Console.WriteLine("Hydrating Assets...");
                    Thread.Sleep(3000 * 100);
                    break;
                default:
                    break;
            }
        }
    }
}
