using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

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
            Directory.CreateDirectory(new_path + Path.DirectorySeparatorChar + "assets");

            var assembly = Assembly.GetExecutingAssembly();
            string first_asset_name = "CalciumSDK.Assets.outdoors_0012.bmp";

            using (Stream stream = assembly.GetManifestResourceStream(first_asset_name))
            {
                if (stream != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        byte[] resourceBytes = ms.ToArray();

                        File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_0001.bmp", resourceBytes);
                    }
                }
            }
            string second_asset_name = "CalciumSDK.Assets.outdoors_0009.bmp";
            using (Stream stream = assembly.GetManifestResourceStream(second_asset_name))
            {
                if (stream != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        byte[] resourceBytes = ms.ToArray();

                        File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_0002.bmp", resourceBytes);
                    }
                }
            }


            // Add logic to create the project here
            Thread.Sleep(1000);
            Console.WriteLine("Project created successfully!");
            Thread.Sleep(1000);
            return true;
        }
    }
}
