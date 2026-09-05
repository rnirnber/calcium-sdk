using CalciumSDK.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CalciumSDK.Text
{
    public static partial class GetPrimeCharacterRendering
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
                                        font_size_mapping[(int)c[0]] = 0;
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
                                    if (c[0] == ' ')
                                    {
                                        font_size_mapping[(int)c[0]] = WORD_SPACE_SIZE;
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
                                    sb.AppendLine("EXPORT ZZZ_RENDER_" + rep.character_int.ToString() + "(X, Y, R, G, B, G_BUFFER)");
                                    sb.AppendLine("BEGIN");
                                    sb.AppendLine("  LOCAL clr := RGB(R, G, B);");
                                    sb.AppendLine("  LOCAL i := 1;");
                                    sb.AppendLine("  LOCAL stop_at := " + ((rep.Lines.Count * 3)).ToString() + ";");
                                    sb.AppendLine();
                                    if(lines_array.Length == 0)
                                    {
                                        sb.AppendLine("  RETURN;");
                                        sb.AppendLine("  LOCAL rects := -1;");
                                    }
                                    else
                                    {
                                        sb.AppendLine("  LOCAL rects := [" + lines_array.ToString() + "];");
                                    }
                                    sb.AppendLine("  FOR i FROM 1 TO stop_at STEP 3 DO");
                                    sb.AppendLine("    LOCAL x_start := rects[i];");
                                    sb.AppendLine("    LOCAL y_start := rects[i + 1];");
                                    sb.AppendLine("    LOCAL y_end := y_start + rects[i + 2];");
                                    sb.AppendLine("    RECT_P(G_BUFFER, X + x_start, Y + y_start, X + x_start, Y + y_end, clr, clr);");
                                    sb.AppendLine("  END;");
                                    sb.AppendLine("END;");
                                    i++;
                                    sb.AppendLine("");
                                    sb.AppendLine();
                                    all_sb.AppendLine(sb.ToString());
                                });
                            }
                        }                        
                    }
                    
                }
            }
            return all_sb.ToString();
        }
    }
}
