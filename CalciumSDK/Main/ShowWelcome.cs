using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static void ShowWelcome()
        {
            Console.Clear();
            "Welcome to Calcium SDK".ToCharArray().ToList().ForEach(c =>
            {
                Console.Write(c);
                Thread.Sleep(40);
            });
            Thread.Sleep(500);
        }
    }
}
