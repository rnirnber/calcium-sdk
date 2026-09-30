using CalciumSDK.Models;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CalciumSDK.Scenes
{
    public static class ParseSceneDataPrime
    {
        private static StringBuilder _SB;
        
        private static void AppendToSB(string msg)
        {
            // 2. Wrap all modifications/reads inside the lock block
            lock (_SBLock)
            {
                _SB.AppendLine(msg);
            }
        }

        private static readonly object _SBLock = new();
        public static string GetTileData()
        {
            _SB = new StringBuilder();

            string scenesPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                            Path.DirectorySeparatorChar + "scenes";

            var files = Directory.GetFiles(scenesPath).ToList().Where((s) => s.EndsWith(".bmp")).ToList();
            var tsks = new List<Task<string>>();

            var scene_name_scene_num_map = new ConcurrentDictionary<string, int>();
            files.ForEach((f) =>
            {
                var this_scene_num = f.Split(Path.DirectorySeparatorChar).ToList().Last().Replace("scene_", "").Replace(".bmp", "");
                while (this_scene_num[0] == '0')
                {
                    this_scene_num = this_scene_num.Substring(1);
                }
                scene_name_scene_num_map[f] = Convert.ToInt32(this_scene_num);
            });
            files.ForEach((f) =>
            {
                tsks.Add(Task.Run(async() =>
                {
                    await Task.Delay(1);

                    var path = f.Replace(".bmp", ".json");

                    using (SKBitmap bitmap = SKBitmap.Decode(File.ReadAllBytes(f)))
                    {
                        var this_scene_num = scene_name_scene_num_map[f];

                        var s_n = f.Split(Path.DirectorySeparatorChar).ToList().Last().Replace("scene_", "").Replace(".bmp", "");
                        while (s_n[0] == '0')
                        {
                            s_n = s_n.Substring(1);
                        }
                        var ret = new StringBuilder();
                        ret.AppendLine("EXPORT GET_SCENE_TILE_DATA_" + this_scene_num + "()");
                        ret.AppendLine("BEGIN");
                        ret.Append("  LOCAL tile_data := [");

                        var this_width = bitmap.Width;
                        var this_height = bitmap.Height;

                        var scene_assets_path = f.Replace(".bmp", ".json");
                        var deserialized = JsonSerializer.Deserialize<SceneBlueprint>(MagicFileEncoding.FileEncoding.ReadAllText(path), AppJsonContext.Default.SceneBlueprint);

                        Dictionary<string, int> MapAssetMapping = new Dictionary<string, int>();
                        deserialized.assets_used.ForEach((au) =>
                        {
                            string asset_path = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                            Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_" + Helpers.GetPaddedNum(au) + ".bmp";

                            using (SKBitmap bitmap2 = SKBitmap.Decode(File.ReadAllBytes(asset_path)))
                            {
                                var same_black_tally = 0;
                                var same_white_tally = 0;

                                var black_coordinate_positive_mapping = new Dictionary<string, bool>();
                                var white_coordinate_postive_mapping = new Dictionary<string, bool>();

                                Dictionary<string, int> MapMapping = new Dictionary<string, int>();

                                for (int i = 0; i < this_width; i += 53)
                                {
                                    for (int k = 0; k < this_height; k += 53)
                                    {
                                        var same_pixel_tally = 0;
                                        for (int ii = 0; ii < 53; ii++)
                                        {
                                            for (int kk = 0; kk < 53; kk++)
                                            {
                                                var asset_pixel = bitmap2.GetPixel(ii, kk);
                                                var map_pixel = bitmap.GetPixel(i + ii, k + kk);

                                                var asset_red = ((int) asset_pixel.Red);
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

                                                if(red_same && green_same && blue_same)
                                                {
                                                    same_pixel_tally++;   
                                                }
                                            }
                                        }
                                        var forgiveness_threshold = Convert.ToDouble(Program.RootConfig.pixel_matching_forgiveness_threshold);
                                        var target_value = Convert.ToDouble((53 * 53)) - ((53 * 53) * (forgiveness_threshold * 0.01));

                                        if (same_pixel_tally >= target_value)
                                        {
                                            var x_off = Convert.ToInt32(i / 53);
                                            var y_off = Convert.ToInt32(k / 53);

                                            MapAssetMapping[x_off.ToString() + "_" + y_off.ToString()] = au;
                                            ret.Append(((x_off * 10000) + y_off).ToString());

                                            ret.Append(",");
                                        }
                                    }
                                }
                            }
                        });
                        var new_ret = ret.ToString().Substring(0, ret.Length - 1);
                        new_ret += "];";
                        new_ret += "\n  RETURN ret;";
                        new_ret += "\nEND;";
                        return new_ret;
                    }
                }));
            });

            Task.WaitAll(tsks);
            var x = 5;
            return _SB.ToString();
        }
    }
}
    