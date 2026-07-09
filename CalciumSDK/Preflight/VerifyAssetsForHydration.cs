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
                var all_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets").ToList();
                var all_valid = new List<string>();
                for(int i = 1; i <= 9999; i++)
                {
                    all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_" + Helpers.GetPaddedNum(i) + ".bmp");
                    all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + "asset_" + Helpers.GetPaddedNum(i) + ".signature");
                }

                var ret_early0 = false;
                var success0 = true;
                all_files.ForEach((f) =>
                {
                    if(ret_early0)
                    {
                        return;
                    }
                    if(!all_valid.Contains(f))
                    {
                        Console.Clear();
                        Console.WriteLine("A foreign file was detected.\n");
                        Console.WriteLine("The following file should not exist in the assets folder:");
                        Console.WriteLine(f.Split(Path.DirectorySeparatorChar).ToList().Last());
                        Console.Out.Flush();
                        Thread.Sleep(1000 * 8);
                        success0 = false;
                        ret_early0 = true;
                    }
                });
                if(!success0)
                {
                    return false;
                }


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

                // deprecated
                if(bad_file != null && false)
                {
                    Console.Clear();
                    Console.WriteLine("There was an invalid filename for a bitmap in the assets folder. File: " + bad_file + ".bmp");
                    Console.Out.Flush();
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
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 5);
                    return false;
                }

                var ret_early1 = false;
                var success1 = true;
                valid_bitmap_files.ForEach((vbf) =>
                {
                    var a_name = vbf.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".bmp", "");
                    if(ret_early1)
                    {
                        return;
                    }
                    var replaced = vbf.Replace(".bmp", ".signature");
                    if(!File.Exists(replaced))
                    {
                        Console.Clear();
                        Console.WriteLine("A signature for an asset bitmap file was missing.");
                        Console.WriteLine("");
                        Console.WriteLine("Bitmap File: " + a_name + ".bmp");
                        Console.WriteLine("No .signature file was found. Please create an empty file called:\n" + a_name + ".signature");
                        Console.Out.Flush();
                        Thread.Sleep(1000 * 10);
                        success1 = false;
                        ret_early1 = true;
                    }
                });
                if(!success1)
                {
                    return false;
                }



                var ret_early2 = false;
                var success2 = true;
                signature_files.ForEach((vbf) =>
                {
                    var a_name = vbf.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".signature", "");
                    if (ret_early2)
                    {
                        return;
                    }
                    var replaced = vbf.Replace(".signature", ".bmp");
                    if (!File.Exists(replaced))
                    {
                        Console.Clear();
                        Console.WriteLine("A signature for an asset was found, but the bitmap was missing.");
                        Console.WriteLine("");
                        Console.WriteLine("Signature File: " + a_name + ".signature");
                        Console.WriteLine("Please add a bitmap file called :\n" + a_name + ".bmp");
                        Console.Out.Flush();
                        Thread.Sleep(1000 * 8);
                        success2 = false;
                        ret_early2 = true;
                    }
                });
                if (!success1)
                {
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
                    Console.Out.Flush();
                    Thread.Sleep(5000);
                }



                var x = 5;

                return true;
            }
        }
    }
}
