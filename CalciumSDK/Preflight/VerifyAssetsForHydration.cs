using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static partial class Preflight
        {
            public static bool VerifyAssetsForHydration(string projectName)
            {
                var valid_bitmap_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets").ToList()
                .Where((f) => f.IndexOf(".bmp") > 0)
                .ToList();

                var valid_bitmap_names = new List<string>();
                for (int i = 1; i <= 9999; i++)
                {
                    valid_bitmap_names.Add("asset_" + Helpers.GetPaddedNum(i) + ".bmp");
                }

                var bitmap_names = valid_bitmap_files.Select((f) => f.Replace(".bmp", "").Split(Path.DirectorySeparatorChar).ToList().Last()).ToList();
                var bad_file = bitmap_names.FirstOrDefault((n) => (!valid_bitmap_names.Contains(n + ".bmp")));
                if(bad_file != null)
                {
                    Console.Clear();
                    Console.WriteLine("There was an invalid filename for a bitmap in the assets folder. File: " + bad_file + ".bmp");
                    Thread.Sleep(1000 * 5);
                    return false;
                }

                var signature_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets").ToList()
                .Where((f) => f.IndexOf(".signature") > 0)
                .ToList();

                var valid_signature_names = new List<string>();
                for(int i = 1; i <= 9999; i++)
                {
                    valid_signature_names.Add("asset_" + Helpers.GetPaddedNum(i) + ".signature");
                }

                var signature_names = signature_files.Select((f) => f.Replace(".signature", "").Split(Path.DirectorySeparatorChar).ToList().Last()).ToList();
                var bad_sig_file = signature_names.FirstOrDefault((n) => (!valid_signature_names.Contains(n + ".signature")));
                if (bad_sig_file != null)
                {
                    Console.Clear();
                    Console.WriteLine("There was an invalid filename for a signature in the assets folder. File: " + bad_sig_file + ".signature");
                    Thread.Sleep(1000 * 5);
                    return false;
                }

                var valid_width_and_height = true;
                var wrong_filename = "";
                valid_bitmap_files.ForEach((vb) =>
                {
                    if(valid_width_and_height)
                    {
                        using (var stream = File.OpenRead(vb))
                        {
                            // Decode only the metadata headers
                            var imageInfo = SKBitmap.DecodeBounds(stream);

                            if (!imageInfo.IsEmpty)
                            {
                                int width = imageInfo.Width;   // Get the width
                                int height = imageInfo.Height; // Get the height

                                valid_width_and_height = (width == 53 && height == 53);
                                wrong_filename = vb.Split(Path.DirectorySeparatorChar).ToList().Last();
                                return;
                            }
                        }
                    }                    
                });
                if(!valid_width_and_height)
                {
                    Console.Clear();
                    Console.WriteLine("An image dimensions error occured. The following file has a width and/or height that is not 53 pixels: \n\n" + wrong_filename);
                    Thread.Sleep(5000);
                }



                var x = 5;

                return true;
            }
        }
    }
}
