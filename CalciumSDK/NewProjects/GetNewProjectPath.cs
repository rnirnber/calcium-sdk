using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static string GetNewProjectPath()
        {
            Console.Clear();
            Console.WriteLine("Please enter the path to your project: ");
            var path = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(path))
            {
                ExitEarly("Path cannot be empty.");
            }
            if (!System.IO.Directory.Exists(path))
            {
                ExitEarly("The path does not exist.");
            }
            var path_chars = path.ToCharArray().ToList().Select((c) => c.ToString()).ToList();
            if (path_chars[path_chars.Count - 1] == Path.DirectorySeparatorChar.ToString())
            {
                path_chars.RemoveAt(path_chars.Count - 1);
            }

            return string.Join("", path_chars);
        }
    }
    internal class GetNewProjectPath
    {
    }
}
