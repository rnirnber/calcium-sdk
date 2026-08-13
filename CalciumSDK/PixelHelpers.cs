using CalciumSDK.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace CalciumSDK;
using SkiaSharp;

public static class PixelHelpers
{
    public static Func<SKColor, int> GetRed = ((SKColor clr) =>
    {
        return Convert.ToInt32(clr.Red);

    });
    public static Func<SKColor, int> GetGreen = ((SKColor clr) =>
    {
        return Convert.ToInt32(clr.Green);

    });
    public static Func<SKColor, int> GetBlue = ((SKColor clr) =>
    {
        return Convert.ToInt32(clr.Blue);

    });

    public static Func<SKColor, bool> isBlack = ((SKColor clr) =>
    {
        return !isWhite(clr);
    });
    public static Func<SKColor, bool> isWhite = ((SKColor clr) =>
    {
        var red = GetRed(clr);
        var green = GetGreen(clr);
        var blue = GetBlue(clr);

        return red > 175 || green > 175 || blue > 175;
    });
    public static List<RectangleItem> GetBlackLines(SKBitmap bmp, int line_num, int current_idx = 0, RectangleItem current_line = null, List<RectangleItem> ret = null)
    {
        if (current_idx == 53)
        {
            if (isBlack(bmp.GetPixel(52, line_num)))
            {
                current_line.end = 52;
                ret.Add(current_line);
            }
            ret = ret.Where((r) =>
            {
                return (r.end > -1);
            }).ToList();

            return ret;
        }
        if (ret == null)
        {
            ret = new List<RectangleItem>();
        }
        if (current_line == null)
        {
            current_line = new RectangleItem();
            current_line.start = current_idx;
            current_line.y = line_num;
            current_line.end = current_line.start;
        }
        if (isBlack(bmp.GetPixel(current_idx, line_num)))
        {
            current_line.end++;
        }
        else
        {
            current_line.end = current_idx - 1;
            var diff = current_line.end - current_line.start;
            if (diff > -1)
            {
                var json = JsonSerializer.Serialize(current_line, AppJsonContext.Default.RectangleItem);
                var copy = JsonSerializer.Deserialize<RectangleItem>(json, AppJsonContext.Default.RectangleItem);
                ret.Add(copy);
            }

            current_line = null;
        }
        return GetBlackLines(bmp, line_num, current_idx + 1, current_line, ret);

    }
    public static List<RectangleItem> GetWhiteLines(SKBitmap bmp, int line_num, int current_idx = 0, RectangleItem current_line = null, List<RectangleItem> ret = null)
    {
        if (current_idx == 53)
        {
            if (isWhite(bmp.GetPixel(52, line_num)))
            {
                current_line.end = 52;
                ret.Add(current_line);
            }
            ret = ret.Where((r) =>
            {
                return (r.end > -1);
            }).ToList();

            return ret;
        }
        if (ret == null)
        {
            ret = new List<RectangleItem>();
        }
        if (current_line == null)
        {
            current_line = new RectangleItem();
            current_line.start = current_idx;
            current_line.y = line_num;
            current_line.end = current_line.start;
        }
        if (isWhite(bmp.GetPixel(current_idx, line_num)))
        {
            current_line.end++;
        }
        else
        {
            current_line.end = current_idx - 1;
            var diff = current_line.end - current_line.start;
            if (diff > -1)
            {
                var json = JsonSerializer.Serialize(current_line, AppJsonContext.Default.RectangleItem);
                var copy = JsonSerializer.Deserialize<RectangleItem>(json, AppJsonContext.Default.RectangleItem);
                ret.Add(copy);
            }

            current_line = null;
        }
        return GetWhiteLines(bmp, line_num, current_idx + 1, current_line, ret);

    }
}