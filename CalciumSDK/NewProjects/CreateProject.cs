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
            var ret = false;
            try
            {
                Console.WriteLine($"Creating project: {projectName}");
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects");
                }
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName))
                {
                    Console.Clear();
                    Console.WriteLine($"Project {projectName} already exists. Please choose a different name.");
                    Thread.Sleep(2000);
                    return false;
                }
                var new_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName;

                Directory.CreateDirectory(new_path);
                Directory.CreateDirectory(new_path + Path.DirectorySeparatorChar + "assets");
                Directory.CreateDirectory(new_path + Path.DirectorySeparatorChar + "scenes");
                Directory.CreateDirectory(new_path + Path.DirectorySeparatorChar + "asset_edits");

                var assembly = Assembly.GetExecutingAssembly();
                var assets_to_copy = new Dictionary<string, string>()
            {
                {"outdoors_0012.bmp", "asset_0001.bmp" },
                {"outdoors_0009.bmp", "asset_0002.bmp" },
                {"asset_black.bmp", "asset_0003.bmp" },
                {"cactus.bmp", "asset_0004.bmp"},
                {"asset_white.bmp", "asset_0005.bmp" }
            };

                assets_to_copy.Keys.ToList().ForEach((key) =>
                {
                    using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets." + key))
                    {
                        if (stream != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                byte[] resourceBytes = ms.ToArray();

                                File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + assets_to_copy[key], resourceBytes);
                            }
                        }
                    }
                });
                using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.cactus_edited.bmp"))
                {
                    if (stream != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            byte[] resourceBytes = ms.ToArray();

                            File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "asset_edits" + Path.DirectorySeparatorChar + "asset_0004.bmp", resourceBytes);
                        }
                    }
                }

                var scenes_to_copy = new Dictionary<string, string>()
            {
                {"scene1.bmp", "scene_0001.bmp"}
            };

                scenes_to_copy.Keys.ToList().ForEach((key) =>
                {
                    using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets." + key))
                    {
                        if (stream != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                byte[] resourceBytes = ms.ToArray();

                                File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + scenes_to_copy[key], resourceBytes);
                            }
                        }
                    }
                });
                Thread.Sleep(1000);
                Console.WriteLine("Project created successfully!");
                Thread.Sleep(1000);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
