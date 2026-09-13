using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CalciumSDK.Models;
using MagicFileEncoding;

namespace CalciumSDK.Text;

public static class GetUbuntuInitialDialogs
{
    public static string Get()
    {
        string scenesPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                            Path.DirectorySeparatorChar + "scenes";

        var sb = new StringBuilder();
        sb.AppendLine("public static Dictionary<int, List<int>> InitialDialogs = new Dictionary<int, List<int>>()");
        sb.AppendLine("    {");
        var second_sb = new StringBuilder();
        second_sb.AppendLine("    public static List<int> GetAlternateInitialDialogs(int scene_num)");
        second_sb.AppendLine("    {");
        
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
                var trimmed = this_scene.initial_dialog.Trim().Replace("\r", "");
                var lines = trimmed.Split("\n").ToList();
                var mini_sb = new StringBuilder();
                mini_sb.Append("        {\n");
                mini_sb.Append("            " + i.ToString() + ",\n             new List<int>(){");
                lines.ForEach((line) =>
                {
                    var chars = line.ToCharArray().ToList();
                    chars.ForEach((c) =>
                    {
                        mini_sb.Append(((int) c).ToString());
                        mini_sb.Append(",");
                    });
                    if (line.GetHashCode() != lines.Last().GetHashCode())
                    {
                        mini_sb.Append("-1,");
                    }
                });
                var subbed = mini_sb.ToString().Substring(0, mini_sb.ToString().Length - 1);
                subbed += "}\n        },\n";   
                sb.Append(subbed);
            }
            
            if (this_scene.alternate_initial_dialog != null && this_scene.alternate_initial_dialog.Trim().Length > 0)
            {
                var trimmed = this_scene.alternate_initial_dialog.Trim().Replace("\r", "");
                var lines = trimmed.Split("\n").ToList();
                var mini_sb = new StringBuilder();
                mini_sb.AppendLine("            if(scene_num == " + i.ToString() + ")");
                mini_sb.AppendLine("            {");
                mini_sb.Append("                if(true &&");
                this_scene.alternate_initial_dialog_conditions["ubuntu"].ForEach((cnd) =>
                {
                    mini_sb.Append(" " + cnd + " && ");
                });
                mini_sb.AppendLine("true)");
                mini_sb.AppendLine("                {");
                mini_sb.Append("                    return new List<int>(){");
            
                lines.ForEach((line) =>
                {
                    var chars = line.ToCharArray().ToList();
                    chars.ForEach((c) =>
                    {
                        mini_sb.Append(((int) c).ToString());
                        mini_sb.Append(",");
                    });
                    if (line.GetHashCode() != lines.Last().GetHashCode())
                    {
                        mini_sb.Append("-1,");
                    }
                });
                var subbed = mini_sb.ToString().Substring(0, mini_sb.ToString().Length - 1);
                subbed += "};\n";
                subbed += "                }\n            }\n";
                second_sb.Append(subbed);
            }
        }
        var subbed1 = sb.ToString().Substring(0, sb.ToString().Length - 2);
        subbed1 += "\n    };";

        second_sb.AppendLine("          return null;");
        second_sb.AppendLine("    }");
        var subbed2 = second_sb.ToString();
        return subbed1 + "\n" + subbed2;
    }
}