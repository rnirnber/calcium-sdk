using System.Reflection;
using System.Text;

namespace CalciumSDK.Compilers;

public static class Ubuntu
{
    public static void Generate(string projectName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var new_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu" + Path.DirectorySeparatorChar + "Program.cs";
        
        var path = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                   Path.DirectorySeparatorChar + "main_menu.bmp";
        
        if (!File.Exists(path))
        {
            Console.WriteLine("The main menu file (main_menu.bmp) was missing from the project directory");
            Console.Out.Flush();
            Thread.Sleep(1000 * 7);
            Environment.Exit(0);
            return;
        }
        var sb = new StringBuilder();
        
        var bmp = SkiaSharp.SKBitmap.Decode(path);
        if (bmp.Width != 318 || bmp.Height != 212)
        {
            Console.WriteLine("The Main Menu Image (main_menu.bmp) was not constrained to the 318 by 212 pixel dimension");
            Console.Out.Flush();
            Thread.Sleep(1000 * 7);
            Environment.Exit(0);
            return;
        }
        
        var black_lines = new List<int>();
        for (int i = 0; i < 212; i++)
        {
            var line_scan = PixelHelpers.GetBlackLines(bmp, i, 0, null, null, 318);
            line_scan.ForEach((ls) =>
            {
                black_lines.Add(ls.y);
                black_lines.Add(ls.start);
                black_lines.Add(ls.end);
            });
        }

        var white_lines = new List<int>();
        for (int i = 0; i < 212; i++)
        {
            var line_scan = PixelHelpers.GetWhiteLines(bmp, i, 0, null, null, 318);
            line_scan.ForEach((ls) =>
            {
                white_lines.Add(ls.y);
                white_lines.Add(ls.start);
                white_lines.Add(ls.end);
            });
        }

        var main_fill = "white";
        var lines_to_use = black_lines;
        if (black_lines.Count >= white_lines.Count)
        {
            lines_to_use = white_lines;
            main_fill = "black";
        }
        
        
        using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.ubuntu.txt"))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] resourceBytes = ms.ToArray();
                var code = Encoding.UTF8.GetString(resourceBytes, 0, resourceBytes.Length);
                code = code.Replace("[__PROJECT_NAME]", projectName);
                var rects_sb = new StringBuilder();
                rects_sb.Append("[");
                var idx = 0;
                lines_to_use.ForEach((l) =>
                {
                    rects_sb.Append(l.ToString());
                    if (idx != lines_to_use.Count - 1)
                    {
                        rects_sb.Append(",");
                    }
                    idx++;
                });
                rects_sb.Append("]");
                
                code = code.Replace("[__MAIN_MENU_RECTS]", rects_sb.ToString());
                code = code.Replace("[__TRUE_ALPHA_RED]", Program.RootConfig.true_alpha[0].ToString());
                code = code.Replace("[__TRUE_ALPHA_GREEN]", Program.RootConfig.true_alpha[1].ToString());
                code = code.Replace("[__TRUE_ALPHA_BLUE]", Program.RootConfig.true_alpha[2].ToString());
                code = code.Replace("[__USE_MAIN_MENU_BLACK_BACKGROUND]", (black_lines.Count >= white_lines.Count).ToString().ToLower());
                code = code.Replace("[__MAIN_MENU_STOP_AT]", ((lines_to_use.Count - 3)).ToString());
                
                File.WriteAllText(new_path, code);
            }
        }
    }
}