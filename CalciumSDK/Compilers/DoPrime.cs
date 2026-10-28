using CalciumSDK.Scenes;
using CalciumSDK.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CalciumSDK.Compilers
{
    public static class Prime
    {
        private static string GetPlayerAndFollowerCode()
        {
            var ret = new StringBuilder();
        
            new List<string>(){"player", "follower"}.ForEach((dir_name) =>
            { 
                new List<string>(){"up", "down", "left", "right"}.ForEach((dir) =>
                {
                    ret.AppendLine("EXPORT ZZZ_RENDER_" + dir_name + "_" + dir + "(x, y, G)");
                    ret.AppendLine("BEGIN");
                    ret.AppendLine("  LOCAL alpha_color := RGB(255, 255, 255);");
                    ret.AppendLine("  LOCAL fore_color := RGB(0, 0, 0);");
                    ret.AppendLine("");

                    var this_asset = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT + Path.DirectorySeparatorChar + "player" + Path.DirectorySeparatorChar + dir_name + "_" + dir + ".bmp";
                    var asset_bmp = SkiaSharp.SKBitmap.Decode(this_asset);
                    var black_lines = new List<int>();
                    for (int n = 0; n < 53; n++)
                    {
                        var line_scan = PixelHelpers.GetBlackLines(asset_bmp, n, 0);
                        line_scan.ForEach((ls) =>
                        {
                            black_lines.Add(ls.y);
                            black_lines.Add(ls.start);
                            black_lines.Add(ls.end);
                        });
                    }

                    var main_fill = "white";
                    var lines_to_use = black_lines;
                
                    var rects_json = new StringBuilder();
                    rects_json.Append("{");
                
                    var line_tally = 0;
                    lines_to_use.ForEach((l) =>
                    {
                        rects_json.Append(l.ToString());
                        if ((line_tally != lines_to_use.Count - 1) && lines_to_use.Count > 0)
                        {
                            rects_json.Append(",");
                        }
                        line_tally++;
                    });
                    rects_json.Append("}");
                    if (lines_to_use.Count == 0)
                    {
                        rects_json.Clear();
                        rects_json.Append("{}");
                    }

                    ret.AppendLine("");
                    ret.AppendLine("  RECT_P(G, x * 53, y * 53, (x * 53) + 53, (y * 53) + 53, alpha_color, alpha_color);");
                    ret.AppendLine("  LOCAL i := 1;");
                    ret.AppendLine("  LOCAL y_start := -1;");
                    ret.AppendLine("  LOCAL y_end := -1;");
                    ret.AppendLine("  LOCAL x_start := -1;");
                    ret.AppendLine("  LOCAL x_end := -1;");
                    ret.AppendLine();
                    ret.AppendLine("  LOCAL rects := [" + rects_json.ToString().Replace("{", "").Replace("}", "") + "];");
                    ret.AppendLine("  LOCAL stop_at := " + (lines_to_use.Count - 3).ToString() + ";");
                    ret.AppendLine("  FOR i FROM 1 TO stop_at STEP 3 DO"); ;
                    ret.AppendLine("    y_start := (y * 53) + rects[i];");
                    ret.AppendLine("    y_end := y_start;");
                    ret.AppendLine("    x_start := (x * 53) + rects[i + 1];");
                    ret.AppendLine("    x_end := (x * 53) + rects[i + 2];");
                    ret.AppendLine("    RECT_P(G, x_start, y_start, x_end, y_end, fore_color, fore_color);");
                    ret.AppendLine("  END;");
                    ret.AppendLine("END;");
                    ret.AppendLine("");
                }); 
            });
        
            return ret.ToString();
        }
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
                    if (main_fill == "white")
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
                    var initial_ds = GetPrimeInitialDialogs.Get();

                    code = code.Replace("[__RECTS]", rects_sb.ToString().Trim().Replace("\r", "").Replace("\n", ""));
                    code = code.Replace("[__STOP_AT]", ((lines_to_use.Count - 2)).ToString());
                    code = code.Replace("[__INITIAL_DIALOGS_CODE]", initial_ds);
                    code = code.Replace("[__PLAYER_RENDERING_CODE]", GetPlayerAndFollowerCode());

                    var ts2 = Scenes.ParseSceneDataPrime.GetTileData();

                    var publish_path = Helpers.GetDocsFolder() + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "prime" + Path.DirectorySeparatorChar + "support.ppl";                    

                    File.WriteAllText(publish_path, code);
                }
            }
            Assets.DoPrime.Do();

            using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.prime_scenes.txt"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byte[] resourceBytes = ms.ToArray();
                    var code = Encoding.UTF8.GetString(resourceBytes);
                    var tile_data_code = Scenes.ParseSceneDataPrime.GetTileData();
                    tile_data_code.Wait();
                    code = code.Replace("[__TILE_DATA_FNs]", tile_data_code.Result + "\n\n" + Scenes.ForbiddenAllowedPrime.Do());

                    var publish_path2 = Helpers.GetDocsFolder() + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "prime" + Path.DirectorySeparatorChar + "scenes.ppl";
                    File.WriteAllText(publish_path2, code);

                }
            }
        }
    }
}
