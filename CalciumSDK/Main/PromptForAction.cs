using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static string PromptForInitialAction()
        {
            Console.Clear();
            Console.WriteLine("1. Create a new project");
            Console.WriteLine("2. Open an existing project");
            Console.Out.Flush();
            return Console.ReadLine().Trim().ToCharArray().ToList().First().ToString();
        }
    }
}
