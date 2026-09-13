using System.Reflection;
using System.Text;

namespace CalciumSDK.Compilers;

public static class Ubuntu
{
    public static void Generate(string projectName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var new_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu" + Path.DirectorySeparatorChar + "Program.cs";
        var sln_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu" + Path.DirectorySeparatorChar + projectName + ".slnx";
        var proj_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu" + Path.DirectorySeparatorChar + projectName + ".csproj";
        var publish_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CalciumProjects" + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "ubuntu" + Path.DirectorySeparatorChar + "publish.sh";
        
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

        var sln_file = new StringBuilder();
        sln_file.AppendLine("<Solution>");
        sln_file.AppendLine("\t<Project Path=\"" + projectName + ".csproj\" />");
        sln_file.AppendLine("</Solution>");

        var cs_proj_file = new StringBuilder();
        cs_proj_file.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
        cs_proj_file.AppendLine("\t<PropertyGroup>");
        cs_proj_file.AppendLine("\t\t<OutputType>Exe</OutputType>");
        cs_proj_file.AppendLine("\t\t<TargetFramework>net10.0</TargetFramework>");
        cs_proj_file.AppendLine("\t\t<ImplicitUsings>enable</ImplicitUsings>");
        cs_proj_file.AppendLine("\t\t<Nullable>enable</Nullable>");
        cs_proj_file.AppendLine("\t\t<PublishAot>true</PublishAot>");
        cs_proj_file.AppendLine("\t\t<InvariantGlobalization>true</InvariantGlobalization>");
        cs_proj_file.AppendLine("\t</PropertyGroup>");
        cs_proj_file.AppendLine("\t<ItemGroup>");
        cs_proj_file.AppendLine("\t\t<PackageReference Include=\"GirCore.Gtk-4.0\" Version=\"0.8.1\" />");
        cs_proj_file.AppendLine("\t</ItemGroup>");
        cs_proj_file.AppendLine("</Project>");
        
        
        
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
                var distinct_chars = Program.RootConfig.characters_used.Distinct().ToList();
                    
                    
                if (!distinct_chars.Contains("\n"))
                {
                    distinct_chars.Add("\n");
                }

                if (!distinct_chars.Contains("\r"))
                {
                    distinct_chars.Add("\r");
                }
                code = code.Replace("[__TEXTRENDERING_CODE]", Text.GetUbuntuCharacterRendering.Get(distinct_chars));

                var assets_code = GetAssetsCode(projectName);
                code = code.Replace("[__ASSETS_CODE]", assets_code);

                code = code.Replace("[__INITIAL_DIALOG_CODE]", Text.GetUbuntuInitialDialogs.Get());
                
                File.WriteAllText(new_path, code);
                File.WriteAllText(proj_path, cs_proj_file.ToString());
                File.WriteAllText(sln_path, sln_file.ToString());
                
                File.WriteAllText(publish_path, "dotnet publish -c Release -r linux-x64 -p:PublishAot=true --self-contained");
            }
        }
    }

    private static string GetAssetsCode(string projectName)
    {
        var ret = new StringBuilder();
        for (int i = 1; i <= 9999; i++)
        {
            var this_asset = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_" + Helpers.GetPaddedNum(i) + ".bmp";
            if (File.Exists(this_asset))
            {
                var rects_path = this_asset.Replace(".bmp", ".rects");
                if (!File.Exists(rects_path) || true)
                {
                    ret.AppendLine("    private static Action<int, int, Context, int, int, int> render_asset_" + Helpers.GetPaddedNum(i) + " = ((int starting_x, int starting_y, Context ctx, int current_alpha_red, int current_alpha_green, int current_alpha_blue) => ");
                    ret.AppendLine("    {");
                    ret.AppendLine("        var alpha_red = _GetRedAsDecimal(current_alpha_red);");
                    ret.AppendLine("        var alpha_green = _GetGreenAsDecimal(current_alpha_green);");
                    ret.AppendLine("        var alpha_blue = _GetBlueAsDecimal(current_alpha_blue);");
                    ret.AppendLine();
                    ret.AppendLine("        var black_red = _GetRedAsDecimal(0);");
                    ret.AppendLine("        var black_green = _GetGreenAsDecimal(0);");
                    ret.AppendLine("        var black_blue = _GetBlueAsDecimal(0);");
                    ret.AppendLine();

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

                    var white_lines = new List<int>();
                    for (int n = 0; n < 53; n++)
                    {
                        var line_scan = PixelHelpers.GetWhiteLines(asset_bmp, n, 0);
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
                    if (main_fill == "white")
                    {
                        ret.AppendLine("        ctx.SetSourceRgb(alpha_red, alpha_green, alpha_blue);");
                    }
                    else
                    {
                        ret.AppendLine("        ctx.SetSourceRgb(black_red, black_green, black_blue);");
                    }
                    ret.AppendLine("        ctx.Rectangle(starting_x * 53 * _XScaleFactor, starting_y * 53 * _YScaleFactor, 53 *  _XScaleFactor, 53 * _YScaleFactor);");
                    ret.AppendLine("        ctx.Fill();");
                    ret.AppendLine("");
                    
                    if (main_fill == "white")
                    {
                        ret.AppendLine("        ctx.SetSourceRgb(black_red, black_green, black_blue);");
                    }
                    else
                    {
                        ret.AppendLine("        ctx.SetSourceRgb(alpha_red, alpha_green, alpha_blue);");
                    }
                    var rects_json = new StringBuilder();
                    rects_json.Append("[");
                    
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
                    rects_json.Append("]");
                    if (lines_to_use.Count == 0)
                    {
                        rects_json.Clear();
                        rects_json.Append("[]");
                    }
                    
                    ret.AppendLine("        if(!_AssetsCache.ContainsKey(" + i.ToString() + "))");
                    ret.AppendLine("        {");
                    ret.AppendLine("            var rects_json = \"" + rects_json.ToString() + "\";");
                    ret.AppendLine("            _AssetsCache[" + i.ToString() + "] = JsonSerializer.Deserialize<List<int>>(rects_json, JSON_CTX.Default.ListInt32);");  
                    ret.AppendLine("        }");
                    ret.AppendLine("        var rects = _AssetsCache[" + i.ToString() + "];");
                    ret.AppendLine("        for(int i = 0; i <= rects.Count - 3; i += 3) ");
                    ret.AppendLine("        {");
                    ret.AppendLine("            var y_start = (rects[i] * _YScaleFactor) + (starting_y * _YScaleFactor);");
                    ret.AppendLine("            var x_start = (starting_x * 53) + (rects[i + 1] * _XScaleFactor);");
                    ret.AppendLine("            var x_end = (starting_x * 53) + (rects[i + 2] * _XScaleFactor);");
                    ret.AppendLine("            ctx.Rectangle(x_start, y_start, ((x_end - x_start) + _XScaleFactor), _YScaleFactor);");
                    ret.AppendLine("            ctx.Fill();");
                    ret.AppendLine("        }");
                    ret.AppendLine("    });");
                    ret.AppendLine();
                }
            }
        }

        return ret.ToString();
    }
}