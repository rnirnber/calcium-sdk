using CalciumSDK.Models;
using MagicFileEncoding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CalciumSDK.Text
{
    public static class GetPrimeInitialDialogs
    {
        public static string Get()
        {
            string scenesPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                            Path.DirectorySeparatorChar + "scenes";

            var sb = new StringBuilder();
            sb.AppendLine("EXPORT GetInitialDialogs(scene_num)");
            sb.AppendLine("BEGIN");
            sb.AppendLine("  LOCAL ret := [-2];");

            for (int i = 1; i <= 9999; i++)
            {
                var path = scenesPath + Path.DirectorySeparatorChar + "scene_" + Helpers.GetPaddedNum(i) + ".json";
                if (!File.Exists(path))
                {
                    continue;
                }
                var this_scene = JsonSerializer.Deserialize<SceneBlueprint>(
                    FileEncoding.ReadAllText(path), AppJsonContext.Default.SceneBlueprint);

                if (this_scene.initial_dialog != null && this_scene.initial_dialog.Trim().Length > 0)
                {
                    var mini_sb = new StringBuilder();
                    mini_sb.AppendLine("  IF scene_num == " + i.ToString() + " THEN");
                    mini_sb.Append("    ret := [");

                    var trimmed = this_scene.initial_dialog.Trim().Replace("\r", "");
                    var lines = trimmed.Split("\n").ToList();
                    lines.ForEach((line) =>
                    {

                        var this_idx = 0;
                        var chars = line.ToCharArray().ToList();
                        var tally = chars.Count();
                        chars.ForEach((c) =>
                        {
                            mini_sb.Append(((int)c).ToString());
                            if(this_idx != tally - 1)
                            {
                                mini_sb.Append(",");
                            }

                            this_idx++;
                        });
                        if (line.GetHashCode() != lines.Last().GetHashCode())
                        {
                            mini_sb.Append("-1,");
                        }

                    });
                    mini_sb.Remove(mini_sb.Length - 1, 1);
                    mini_sb.AppendLine("];");
                    mini_sb.AppendLine("  END;");
                    var conditions = "  IF (scene_num == " + i.ToString() + ")  AND (TRUE)";
                    var tally = this_scene.alternate_initial_dialog_conditions["prime"].Count();
                    var this_idx = 0;
                    this_scene.alternate_initial_dialog_conditions["prime"].ForEach((statement) =>
                    {
                        conditions += " AND  (" + statement + ") ";                        
                        
                        this_idx++;
                    });
                    conditions += "AND (TRUE) THEN\n";
                    conditions += "\n    ret := [";

                    var trimmed2 = this_scene.alternate_initial_dialog.Trim().Replace("\r", "");
                    var lines2 = trimmed2.Split("\n").ToList();

                    lines2.ForEach((line) =>
                    {

                        var this_idx = 0;
                        var chars = line.ToCharArray().ToList();
                        var tally = chars.Count();
                        chars.ForEach((c) =>
                        {
                            conditions += (((int)c).ToString());
                            if (this_idx != tally - 1)
                            {
                                conditions += ",";
                            }

                            this_idx++;
                        });
                        if (line.GetHashCode() != lines.Last().GetHashCode())
                        {
                            conditions += "-1,";
                        }
                    });
                    var new_conditions = "";
                    for(int i2 = 0; i2 < conditions.Length - 3; i2++)
                    {
                        new_conditions += conditions[i2];
                    }
                    new_conditions += "];";
                    mini_sb.AppendLine(new_conditions);
                    mini_sb.AppendLine("  END;");
                    mini_sb.AppendLine("  return ret;");
                    sb.AppendLine(mini_sb.ToString());
                }

            }
            sb.AppendLine("END;");
            return sb.ToString();
        }
    }
}
