using System.Text;
using SkiaSharp;

namespace CalciumSDK;

public static partial class Assets
{
    public static StringBuilder GenerateAssetPPL(string path)
    {
        var ret = new StringBuilder();
        var bmp = SkiaSharp.SKBitmap.Decode(path);
        var black_lines = new List<int>();
        for (int i = 0; i < 53; i++)
        {
            var line_scan = PixelHelpers.GetBlackLines(bmp, i);
            line_scan.ForEach((ls) =>
            {
                black_lines.Add(ls.y);
                black_lines.Add(ls.start);
                black_lines.Add(ls.end);
            });
        }

        var white_lines = new List<int>();
        for (int i = 0; i < 53; i++)
        {
            var line_scan = PixelHelpers.GetWhiteLines(bmp, i);
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

        var asset_name = path.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".bmp", "");
        ret.AppendLine("EXPORT ZZZ_render_" + asset_name + "()");  
        ret.AppendLine("BEGIN");
        if (main_fill == "white")
        {
            ret.AppendLine("  LOCAL main_fill := RGB(255, 255, 255);");
            ret.AppendLine("  LOCAL rect_fill := RGB(0, 0, 0);");
        }
        else
        {
            ret.AppendLine("  LOCAL main_fill := RGB(0, 0, 0);");
            ret.AppendLine("  LOCAL rect_fill := RGB(255, 255, 255);");
        }

        ret.AppendLine("");
        ret.AppendLine("  DIMGROB_P(G1, 53, 53);");
        ret.AppendLine("  RECT_P(G1, 0, 0, 52, 52, main_fill, main_fill);");
        ret.AppendLine("");
        ret.AppendLine("  // [y offset, starting x, ending x");
        ret.Append(" LOCAL rects := [");
        var idx = 0;
        lines_to_use.ForEach((l) =>
        {
            ret.Append(l.ToString());
            if (idx != lines_to_use.Count - 1)
            {
                ret.Append(",");
            }
            idx++;
        });
        ret.Append("];");
        ret.AppendLine();
        ret.AppendLine();
        ret.AppendLine("  LOCAL i = 1;");
        ret.AppendLine("  LOCAL stop_at := SIZE(rects)[1] - 3");
        ret.AppendLine("  LOCAL x_start := 0;");
        ret.AppendLine("  LOCAL x_end := 0;");
        ret.AppendLine("  LOCAL y := 0;");
        ret.AppendLine("  FOR i FROM 1 TO stop_at STEP 3 DO");
        ret.AppendLine("    y = rects[i];");
        ret.AppendLine("    x_start := rects[i + 1];");
        ret.AppendLine("    x_end = :rects[i + 2];");
        ret.AppendLine("    RECT_P(G1, rects[x_start, y, x_end, y, rect_fill, rect_fill);");
        ret.AppendLine("  END;");
        ret.AppendLine("  // FREEZE();");
        ret.AppendLine("END;");
        return ret;
    }
}