using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        000public static void CreateProject(string projectName)
        {
            Console.Clear();
            Console.WriteLine($"Creating project: {projectName}");
            // Add logic to create the project here
            Thread.Sleep(1000);
            Console.WriteLine("Project created successfully!");
            Thread.Sleep(1000);
        }
    }
}
