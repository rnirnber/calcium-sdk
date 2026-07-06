using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static bool TryCreateProject(string projectName)
        {
            Console.Clear();
            Console.WriteLine($"Creating project: {projectName}");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects");
            }
            if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName))
            {
                Console.Clear();
                Console.WriteLine($"Project {projectName} already exists. Please choose a different name.");
                Thread.Sleep(2000);
                return false;
            }
            var new_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName;
            Directory.CreateDirectory(new_path);
            // Add logic to create the project here
            Thread.Sleep(1000);
            Console.WriteLine("Project created successfully!");
            Thread.Sleep(1000);
            return true;
        }
    }
}
