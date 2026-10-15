using System.Collections.Concurrent;
using SkiaSharp;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using CalciumSDK.Models;

namespace CalciumSDK.Scenes;

public static class ForbiddenAllowedUbuntu
{
    public static string Do()
    {
        var all_ret_sb = new StringBuilder();
        var default_tile_sb = new StringBuilder();
        var tile_data_sb = new StringBuilder();
        default_tile_sb.AppendLine("public static int GetDefaultTileForScene(int scene_num)");
        default_tile_sb.AppendLine("    {");
        default_tile_sb.AppendLine("        var ret = -1;");

        List<int> all_scene_nums = new List<int>();

        string scenesPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                            Path.DirectorySeparatorChar + "scenes";

        var files = Directory.GetFiles(scenesPath).ToList().Where((s) => s.EndsWith(".bmp")).ToList();
        var tsks = new List<Task<string>>();

        List<string> default_tile_sb_snippets = new List<string>();

        var scene_name_scene_num_map = new ConcurrentDictionary<string, int>();
        files.ForEach((f) =>
        {
            var this_scene_num = f.Split(Path.DirectorySeparatorChar).ToList().Last().Replace("scene_", "")
                .Replace(".bmp", "");
            while (this_scene_num[0] == '0')
            {
                this_scene_num = this_scene_num.Substring(1);
            }

            scene_name_scene_num_map[f] = Convert.ToInt32(this_scene_num);
            all_scene_nums.Add(Convert.ToInt32(this_scene_num));
        });
        files.ForEach((f) =>
        {
            var this_tsk = Task.Run(async () =>
            {
                await Task.Delay(1);

                var path = f.Replace(".bmp", ".json");

                using (SKBitmap bitmap = SKBitmap.Decode(File.ReadAllBytes(f)))
                {
                    var this_scene_num = scene_name_scene_num_map[f];

                    var s_n = f.Split(Path.DirectorySeparatorChar).ToList().Last().Replace("scene_", "")
                        .Replace(".bmp", "");
                    while (s_n[0] == '0')
                    {
                        s_n = s_n.Substring(1);
                    }

                    var ret = new StringBuilder();
                    var this_width = bitmap.Width;
                    var this_height = bitmap.Height;

                    var scene_assets_path = f.Replace(".bmp", ".json");
                    var deserialized = JsonSerializer.Deserialize<SceneBlueprint>(
                        MagicFileEncoding.FileEncoding.ReadAllText(path), AppJsonContext.Default.SceneBlueprint);

                    var forgiveness_threshold =
                        Convert.ToDouble(Program.RootConfig.pixel_matching_forgiveness_threshold);
                    var target_value = Convert.ToDouble((53 * 53)) - ((53 * 53) * (forgiveness_threshold * 0.01));

                    ConcurrentDictionary<int, int> forbidden_mapping = new ConcurrentDictionary<int, int>();
                    ConcurrentDictionary<string, int> forbidden_coords = new ConcurrentDictionary<string, int>();

                    deserialized.assets_used.ForEach((au) => { forbidden_mapping[au] = 0; });
                    deserialized.forbidden_assets.ForEach((au) =>
                    {
                        string asset_path = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar +
                                            Program.SELECTED_PROJECT +
                                            Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar +
                                            "asset_" + Helpers.GetPaddedNum(au) + ".bmp";
                        
                        using (SKBitmap bmp = SKBitmap.Decode(File.ReadAllBytes(asset_path)))
                        {
                            for (int i = 0; i < this_width; i += 53)
                            {
                                for (int k = 0; k < this_height; k += 53)
                                {
                                    var same_pixel_tally = 0;
                                    for (int i2 = 0; i2 < 53; i2++)
                                    {
                                        for (int k2 = 0; k2 < 53; k2++)
                                        {
                                            
                                            var asset_pixel = bmp.GetPixel(i2, k2);
                                            var map_pixel = bitmap.GetPixel(i + i2, k + k2);

                                            var asset_red = ((int)asset_pixel.Red);
                                            var asset_green = ((int)asset_pixel.Green);
                                            var asset_blue = ((int)asset_pixel.Blue);

                                            var map_red = ((int)map_pixel.Red);
                                            var map_green = ((int)map_pixel.Green);
                                            var map_blue = ((int)map_pixel.Blue);

                                            var red_diff = Math.Abs(map_red - asset_red);
                                            var green_diff = Math.Abs(map_green - asset_green);
                                            var blue_diff = Math.Abs(map_blue - asset_blue);

                                            var red_same = (red_diff < 5);
                                            var green_same = (green_diff < 5);
                                            var blue_same = (blue_diff < 5);

                                            if (red_same && green_same && blue_same)
                                            {
                                                same_pixel_tally++;
                                            }
                                        }
                                    }

                                    var forgiveness_threshold =
                                        Convert.ToDouble(Program.RootConfig.pixel_matching_forgiveness_threshold);
                                    var target_value = Convert.ToDouble((53 * 53)) -
                                                       ((53 * 53) * (forgiveness_threshold * 0.01));
                                    
                                    if (same_pixel_tally >= target_value)
                                    {
                                        forbidden_mapping[au]++;
                                        forbidden_coords[(i / 53).ToString() + "_" + (k / 53).ToString()] = -1;
                                    }
                                }
                            }
                        }
                    });

                    var all_tiles_cnt = ((this_width / 53) * (this_height / 53) * 1.0);
                    var half_tiles_cnt = all_tiles_cnt * 0.5;
                    var mostly_forbidden = (Convert.ToDouble(forbidden_coords.Keys.Count) > half_tiles_cnt);
                    var map_width = this_width / 53;
                    var map_height = this_height / 53;
                    var keys = forbidden_coords.Keys.ToList();
                    
                    var ret_list = new List<int>();
                    if (mostly_forbidden)
                    {
                        
                        for (int i = 0; i < map_width; i++)
                        {
                            for (int k = 0; k < map_height; k++)
                            {
                                var idx = keys.FindIndex((fc) => fc == (i.ToString() + "_" + k.ToString()));
                                if (idx == -1)
                                {
                                       ret_list.Add(-1 * ((i * 10000) + k));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < map_width; i++)
                        {
                            for (int k = 0; k < map_height; k++)
                            {
                                var idx = keys.FindIndex((fc) => fc == (i.ToString() + "_" + k.ToString()));
                                if (idx > -1)
                                {
                                    ret_list.Add(((i * 10000) + k));
                                }
                            }
                        }
                    }
                    ret.AppendLine("        if(scene_num == " + s_n + ")");
                    ret.AppendLine("        {");
                    ret.Append("                return new List<int>(){");
                    ret.Append(String.Join(',', ret_list.ToArray()));
                    ret.Append("};");
                    ret.AppendLine();
                    ret.AppendLine("        }");
                    return ret.ToString();
                }
            });
            tsks.Add(this_tsk);
        });
        Task.WaitAll(tsks);
        all_ret_sb.AppendLine("    public static List<int> GetForbiddenAllowedTilesForScene(int scene_num)");
        all_ret_sb.AppendLine("    {");
        all_ret_sb.AppendLine("        // if it's negative, assume mostly forbidden. if it's positive, assume mostly allowed.");
        tsks.ForEach((t) =>
        {
            all_ret_sb.AppendLine(t.Result);
            all_ret_sb.AppendLine();
        });
        all_ret_sb.AppendLine("        return new List<int>();");
        all_ret_sb.AppendLine("    }");

        return all_ret_sb.ToString();
    }
}