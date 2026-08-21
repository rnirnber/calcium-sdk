using System.Text;




namespace CalciumSDK;

public static class MainMenu
{
    public static void Hydrate(string projectName)
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
        
        sb.AppendLine();
        sb.AppendLine("EXPORT ZZZ_render_main_menu()");
        sb.AppendLine("BEGIN");
        if (main_fill == "white")
        {
            sb.AppendLine("  LOCAL main_fill := RGB(255, 255, 255);");
            sb.AppendLine("  LOCAL rect_fill := RGB(0, 0, 0);");
        }
        else
        {
            sb.AppendLine("  LOCAL main_fill := RGB(0, 0, 0);");
            sb.AppendLine("  LOCAL rect_fill := RGB(255, 255, 255);");
            sb.AppendLine("  LOCAL black_fill := RGB(0, 0, 0);");
        }

        sb.AppendLine("");
        sb.AppendLine("  DIMGROB_P(G1, 318, 212);");
        sb.AppendLine("  RECT_P(G1, 0, 0, 317, 212, main_fill, main_fill);");
        sb.AppendLine("");
        sb.AppendLine("  // [y offset, starting x, ending x");
        sb.Append(" LOCAL rects := [");
        var idx = 0;
        lines_to_use.ForEach((l) =>
        {
            sb.Append(l.ToString());
            if (idx != lines_to_use.Count - 1)
            {
                sb.Append(",");
            }
            idx++;
        });
        sb.Append("];");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("  RECT_P(G1, 0, 0, 1, 239, black_fill, black_fill)");
        sb.AppendLine("  RECT_P(G1, 318, 0, 319, 239, black_fill, black_fill);");
        sb.AppendLine("  RECT_P(G1, 0, 212, 319, 239, black_fill, black_fill);");
        sb.AppendLine();
        sb.AppendLine("  LOCAL i := 1;");
        sb.AppendLine("  LOCAL x_start := 0;");
        sb.AppendLine("  LOCAL x_end := 0;");
        sb.AppendLine("  LOCAL y := 0;");
        sb.AppendLine("  LOCAL stop_at := " + ((lines_to_use.Count + 3) - 3).ToString() + ";");
        sb.AppendLine("  FOR i FROM 1 TO stop_at STEP 3 DO");
        sb.AppendLine("    y = rects[i];");
        sb.AppendLine("    x_start := rects[i + 1];");
        sb.AppendLine("    x_end := rects[i + 2];");
        sb.AppendLine("    RECT_P(G1, x_start, y, x_end, y, rect_fill, rect_fill);");
        sb.AppendLine("  END;");
        sb.AppendLine("  BLIT_P(G0, 1, 0, 317, 211, G1, 1, 0, 317, 211);");
        sb.AppendLine("  // FREEZE();");
        sb.AppendLine("END;");
        sb.AppendLine();

        File.WriteAllText(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                          Path.DirectorySeparatorChar + "main_menu_DO_NOT_EDIT", sb.ToString());
    }
}