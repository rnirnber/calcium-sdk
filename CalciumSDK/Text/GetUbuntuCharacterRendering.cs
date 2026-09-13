using CalciumSDK.Models;

namespace CalciumSDK.Text;
using System.Text;
using SkiaSharp;
using System.Reflection;

public class GetUbuntuCharacterRendering
{
    public static string Get(List<string> distinct_chars)
        {
            int WORD_SPACE_SIZE = 5;
            var font_size_mapping = new Dictionary<int, int>();
            var all_sb = new StringBuilder();

            using (SKBitmap bitmap = new SKBitmap(500, 500))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    using (SKPaint paint = new SKPaint())
                    {
                        paint.IsAntialias = true;
                        paint.Color = SKColors.Black;
                        var assembly = Assembly.GetExecutingAssembly();
                        using (Stream stream = assembly.GetManifestResourceStream("CalciumSDK.v2_assets.font.ttf"))
                        {
                            using (SKFont f = new SKFont(SKTypeface.FromStream(stream)))
                            {
                                f.Subpixel = true;                 // <-- This replaces SubpixelText
                                f.Edging = SKFontEdging.SubpixelAntialias;
                                distinct_chars.ForEach((c) =>
                                {
                                    var i = (int)c[0];
                                    var rep = new LetterRepresentation();
                                    var font_lines = new List<Line>();
                                    f.Size = 11;
                                    canvas.Clear(SKColors.White);
                                    var blob = SKTextBlob.Create(c.ToString(), f);
                                    canvas.DrawText(blob, 50, 50 - f.Metrics.Ascent, paint);
                                    var width = f.MeasureText(c.ToString(), paint);
                                    width++;

                                    for (int k = 0; k < 100; k++)
                                    {
                                        for (int m = 0; m < 100; m++)
                                        {
                                            var pix = bitmap.GetPixel(k, m);
                                            if (PixelHelpers.IsBlack(pix))
                                            {
                                                var black_start = m - 50;
                                                var black_inc = 1;
                                                var height = 1;
                                                var new_line = new Line();
                                                new_line.x = k - 50;
                                                new_line.y = m - 50;
                                                new_line.letter = (int)c[0];
                                                new_line.width = ((int) width);

                                                var white_encountered = false;
                                                while (!white_encountered)
                                                {
                                                    var pix2 = bitmap.GetPixel(k, m + black_inc);
                                                    if (PixelHelpers.IsBlack(pix2))
                                                    {
                                                        black_inc++;
                                                    }
                                                    else
                                                    {
                                                        new_line.height = (black_inc);
                                                        font_lines.Add(new_line);
                                                        white_encountered = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    rep.Lines = font_lines;
                                    if (rep.Lines.Count == 0)
                                    {
                                        font_size_mapping[(int)c[0]] = WORD_SPACE_SIZE;
                                    }
                                    else
                                    {
                                        var max_x_item = rep.Lines.OrderByDescending((l) => l.x).First();
                                        var farthest_to_the_right = max_x_item.x + max_x_item.width;
                                        var min_x_item = rep.Lines.OrderBy((l) => l.x).First();
                                        var farthest_to_the_left = min_x_item.x;

                                        var char_width = Math.Abs(max_x_item.x + 1);

                                        font_lines.ForEach((fl) =>
                                        {
                                            font_size_mapping[(int)c[0]] = char_width;
                                        });

                                    }
                                    rep.character_int = ((int)c[0]);

                                    var char_bytes = ((byte)((int)c[0]));
                                    var utf_8_str = Encoding.UTF8.GetString(new List<byte>() { char_bytes }.ToArray());
                                    //rep.character_char = utf_8_str;                        
                                    var x = 5;
                                    var lines_array = new StringBuilder();
                                    rep.Lines.ForEach((l) =>
                                    {
                                        lines_array.Append(l.x.ToString());
                                        lines_array.Append(",");
                                        lines_array.Append(l.y.ToString());
                                        lines_array.Append(",");
                                        lines_array.Append(l.height.ToString());
                                        if (l.GetHashCode() != rep.Lines.Last().GetHashCode())
                                        {
                                            lines_array.Append(",");
                                        }
                                    });

                                    var sb = new StringBuilder();
                                    sb.AppendLine("  public static void  Render" + ((int) c[0]).ToString() + "(int x, int y, int r, int g, int b, Context ctx)");
                                    sb.AppendLine("  {");
                                    sb.AppendLine("      var r_clr = _GetRedAsDecimal(r);");
                                    sb.AppendLine("      var g_clr = _GetGreenAsDecimal(g);");
                                    sb.AppendLine("      var b_clr = _GetBlueAsDecimal(b);");
                                    sb.AppendLine("");
                                    sb.AppendLine("      ctx.SetSourceRgb(r_clr, g_clr, b_clr);");
                                    sb.AppendLine();
                                    if(lines_array.Length == 0)
                                    {
                                        sb.AppendLine("int[] rects = [];");
                                        
                                    }
                                    else
                                    {
                                        sb.AppendLine("    int[] rects = [" + lines_array.ToString() + "];");
                                    }

                                    sb.AppendLine();

                                    sb.AppendLine("    for(int i = 0; i < " + (rep.Lines.Count * 3).ToString() + "; i += 3)");
                                    sb.AppendLine("    {");
                                    sb.AppendLine("        var x_start = (rects[i] + x) * _XScaleFactor;");
                                    sb.AppendLine("        var y_start = (rects[i + 1] + y) * _YScaleFactor;");
                                    sb.AppendLine("        var y_end = y_start + (_YScaleFactor * rects[i + 2]);");
                                    sb.AppendLine("        ctx.Rectangle(x_start, y_start, _XScaleFactor, (y_end - y_start));");
                                    sb.AppendLine("        ctx.Fill();");
                                    sb.AppendLine("    }");
                                    sb.AppendLine("  }");
                                    i++;
                                    sb.AppendLine("");
                                    sb.AppendLine();
                                    all_sb.AppendLine(sb.ToString());
                                });
                                var sb = new StringBuilder();
                                sb.AppendLine("  public static void RenderChar(int char_code, int x, int y, int r, int g, int b, Context ctx");
                                sb.AppendLine("  {");
                                distinct_chars.ForEach((dc) =>
                                {
                                    sb.AppendLine("    if(char_code == " + ((int)dc[0]).ToString() + ")");
                                    sb.AppendLine("    {");
                                    sb.AppendLine("         Render" + ((int) dc[0]).ToString() + "(x, y, r, g, b, ctx);");
                                    sb.AppendLine("    }");
                                });
                                sb.AppendLine(" }");
                            }
                        }                        
                    }
                    
                }
            }
            all_sb.AppendLine("");
            all_sb.AppendLine("  public static int GetCharacterWidth(int char_code)");
            all_sb.AppendLine("  {");
            var keys = font_size_mapping.Keys.ToList();
            all_sb.AppendLine("    var char_width_mapping = new Dictionary<int, int>()");
            all_sb.AppendLine("    {");
            keys.ForEach((k) =>
            {
                all_sb.Append("        {" + k.ToString() + ", " + font_size_mapping[k].ToString() + "}");
                if(k.GetHashCode() != keys.Last().GetHashCode())
                {
                    all_sb.Append(", ");
                }
                else
                {
                    all_sb.Append("};");
                }
                all_sb.AppendLine();
            });
            all_sb.AppendLine();
            all_sb.AppendLine("        return char_width_mapping[char_code];");
            all_sb.AppendLine("  }");
            all_sb.AppendLine("");

            return all_sb.ToString();
        }
}