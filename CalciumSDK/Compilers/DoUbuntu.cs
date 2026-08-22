using System.Reflection;
using System.Text;

namespace CalciumSDK.Compilers;

public static class Ubuntu
{
    public static void Generate(string projectName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var new_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu";
        
        using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.ubuntu.txt"))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] resourceBytes = ms.ToArray();
                var code = Encoding.UTF8.GetString(resourceBytes, 0, resourceBytes.Length);
                
                // do the main menu

                File.WriteAllBytes(new_path + Path.DirectorySeparatorChar + "internals" + Path.DirectorySeparatorChar + "ubuntu" + "ubuntu", resourceBytes);
            }
        }
    }
}