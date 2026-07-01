using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static void ExitEarly(string msg)
        {
            Console.Clear();
            Console.WriteLine("Error: " + msg);
            Console.WriteLine("\n\nPress any key to exit...");
            var _ = Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
