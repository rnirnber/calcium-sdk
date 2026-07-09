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
            Console.WriteLine("Please enter the name of your new project: A-Z (Captials Only) and underscores only. Underscores must not be first character");
            var allowed_chars = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "_" };

            var these_chars = Console.ReadLine().Trim().ToCharArray().ToList();
            var invalid_chars = these_chars.Where(c => !allowed_chars.Contains(c.ToString())).ToList();
            if (invalid_chars.Any())
            {
                Console.Clear();
                Console.WriteLine("Invalid characters found. Please use only A-Z and underscores.");
                Console.Out.Flush();
                Thread.Sleep(2000);
                return PromptForProjectName();
            }
            return String.Join("", these_chars);
        }
    }
}
