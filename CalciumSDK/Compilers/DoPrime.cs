using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CalciumSDK.Text;

namespace CalciumSDK.Compilers
{
    public static class Prime
    {
        public static void Generate(string projectName)
        {
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
            if (black_lines.Count > white_lines.Count)
            {
                lines_to_use = white_lines;
                main_fill = "black";
            }
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.prime_support.txt"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byte[] resourceBytes = ms.ToArray();
                    var code = Encoding.UTF8.GetString(resourceBytes);
                    if(main_fill == "white")
                    {
                        code = code.Replace("[__MAIN_BACKGROUND_RED]", Program.RootConfig.alpha_step_one[0].ToString());
                        code = code.Replace("[__MAIN_BACKGROUND_GREEN]", Program.RootConfig.alpha_step_one[1].ToString());
                        code = code.Replace("[__MAIN_BACKGROUND_BLUE]", Program.RootConfig.alpha_step_one[2].ToString());

                        code = code.Replace("[__RECT_BACKGROUND_RED]", "0");
                        code = code.Replace("[__RECT_BACKGROUND_GREEN_", "0");
                        code = code.Replace("[__RECT_BACKGROUND_BLUE]", "0");
                    }
                    else
                    {
                        code = code.Replace("[__MAIN_BACKGROUND_RED]", "0");
                        code = code.Replace("[__MAIN_BACKGROUND_GREEN]", "0");
                        code = code.Replace("[__MAIN_BACKGROUND_BLUE]", "0");

                        code = code.Replace("[__RECT_BACKGROUND_RED]", Program.RootConfig.alpha_step_one[0].ToString());
                        code = code.Replace("[__RECT_BACKGROUND_GREEN]", Program.RootConfig.alpha_step_one[1].ToString());
                        code = code.Replace("[__RECT_BACKGROUND_BLUE]", Program.RootConfig.alpha_step_one[2].ToString());

                        var distinct_chars = Program.RootConfig.characters_used.Distinct().ToList();
                        if (!distinct_chars.Contains("\n"))
                        {
                            distinct_chars.Add("\n");
                        }

                        if (!distinct_chars.Contains("\r"))
                        {
                            distinct_chars.Add("\r");
                        }
                        code = code.Replace("[__CHARACTER_CODE_FNs]", Text.GetPrimeCharacterRendering.Get(distinct_chars));
                    }
                    var rects_sb = new StringBuilder();

                    rects_sb.Append("[");
                    var idx2 = 0;
                    lines_to_use.ForEach((l) =>
                    {
                        rects_sb.Append(l.ToString());
                        if (idx2 != lines_to_use.Count - 1)
                        {
                            rects_sb.Append(",");
                        }
                        idx2++;
                    });
                    rects_sb.Append("]");
                    code = code.Replace("[__RECTS]", rects_sb.ToString().Trim().Replace("\r", "").Replace("\n", ""));
                    code = code.Replace("[__STOP_AT]", ((lines_to_use.Count - 2)).ToString());

                    var initial_dialogs = GetUbuntuInitialDialogs.Get();

                    var publish_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "prime" + Path.DirectorySeparatorChar + "support.ppl";
                    File.WriteAllText(publish_path, code);
                }
            }

        }
    }
}
