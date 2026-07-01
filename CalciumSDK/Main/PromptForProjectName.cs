using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static string PromptForProjectName()
        {
            Console.Clear();
            Console.WriteLine("Please enter the name of your new project: A-Z and UNDERSCORE characters only. Underscore must not be first character");
            return Console.ReadLine().Trim();
        }
    }
}
