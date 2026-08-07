using SkiaSharp;

namespace CalciumSDK;

public static partial class Program
{
    public static partial class Preflight
    {
        public static bool VerifyScenesForHydration(string projectName)
        {
            var all_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                                               Path.DirectorySeparatorChar + "scenes").ToList();
            var all_valid = new List<string>();
            for (int i = 1; i <= 9999; i++)
            {
                all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                              Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + "scene_" +
                              Helpers.GetPaddedNum(i) + ".bmp");
                all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                              Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + "scene_" +
                              Helpers.GetPaddedNum(i) + ".signature");
                all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName +
                              Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + "scene_" +
                              Helpers.GetPaddedNum(i) + ".json");
            }

            var ret_early0 = false;
            var success0 = true;
            all_files.ForEach((f) =>
            {
                if (ret_early0)
                {
                    return;
                }

                if (!all_valid.Contains(f))
                {
                    Console.Clear();
                    Console.WriteLine("A foreign file was detected.\n");
                    Console.WriteLine("The following file should not exist in the scenes folder:");
                    Console.WriteLine(f.Split(Path.DirectorySeparatorChar).ToList().Last());
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 8);
                    success0 = false;
                    ret_early0 = true;

                }
            });

            var valid_bitmap_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar +
                                                        projectName + Path.DirectorySeparatorChar + "scenes").ToList()
                .Where((f) => f.IndexOf(".bmp") > 0)
                .ToList();

            var valid_bitmap_names = new List<string>();
            for (int i = 1; i <= 9999; i++)
            {
                valid_bitmap_names.Add("scene_" + Helpers.GetPaddedNum(i) + ".bmp");
            }

            var valid_width_and_height = true;
            var wrong_filename = "";
            valid_bitmap_files.ForEach((vb) =>
            {
                if (valid_width_and_height)
                {
                    using (var stream = File.OpenRead(vb))
                    {
                        // Decode only the metadata headers
                        var imageInfo = SKBitmap.DecodeBounds(stream);

                        if (!imageInfo.IsEmpty)
                        {
                            int width = imageInfo.Width; // Get the width
                            int height = imageInfo.Height; // Get the height

                            valid_width_and_height = ((width % 53 == 0) && (height % 53 == 0));
                            wrong_filename = vb.Split(Path.DirectorySeparatorChar).ToList().Last();
                        }
                    }
                }
            });
            if (!valid_width_and_height)
            {
                Console.Clear();
                Console.WriteLine(
                    "An image dimensions error occured. The following file has a width and/or height that is not evenly divisible by 53 pixels: \n\n" +
                    wrong_filename);
                Console.Out.Flush();
                Thread.Sleep(5000 * 2);
                var x2 = Console.ReadLine();
                Environment.Exit(0);
            }

            var signature_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar +
                                                     projectName + Path.DirectorySeparatorChar + "scenes").ToList()
                .Where((f) => f.IndexOf(".signature") > 0)
                .ToList();

            var valid_signature_names = new List<string>();
            for (int i = 1; i <= 9999; i++)
            {
                valid_signature_names.Add("scene_" + Helpers.GetPaddedNum(i) + ".signature");
            }

            var signature_names = signature_files
                .Select((f) => f.Replace(".signature", "").Split(Path.DirectorySeparatorChar).ToList().Last()).ToList();
            var bad_sig_file =
                signature_names.FirstOrDefault((n) => (!valid_signature_names.Contains(n + ".signature")));
            if (bad_sig_file != null)
            {
                Console.Clear();
                Console.WriteLine("There was an invalid filename for a signature in the assets folder. File: " +
                                  bad_sig_file + ".signature");
                Console.Out.Flush();
                Thread.Sleep(1000 * 5);
                return false;
            }

            var ret_early1 = false;
            var success1 = true;
            valid_bitmap_files.ForEach((vbf) =>
            {
                var a_name = vbf.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".bmp", "");
                if (ret_early1)
                {
                    return;
                }

                var replaced = vbf.Replace(".bmp", ".signature");
                if (!File.Exists(replaced))
                {
                    Console.Clear();
                    Console.WriteLine("A signature for a scene bitmap file was missing.");
                    Console.WriteLine("");
                    Console.WriteLine("Bitmap File: " + a_name + ".bmp");
                    Console.WriteLine("No .signature file was found. Please create an empty file called:\n" + a_name +
                                      ".signature");
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 10);
                    success1 = false;
                    ret_early1 = true;
                }
            });
            if (!success1)
            {
                return false;
            }

            var ret_early3 = false;
            var success3 = true;

            valid_bitmap_files.ForEach((vbf) =>
            {
                var a_name = vbf.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".bmp", "");
                if (ret_early3)
                {
                    return;
                }

                var replaced = vbf.Replace(".bmp", ".json");
                if (!File.Exists(replaced))
                {
                    Console.Clear();
                    Console.WriteLine("A JSON for a scene bitmap file was missing.");
                    Console.WriteLine("");
                    Console.WriteLine("Bitmap File: " + a_name + ".bmp");
                    Console.WriteLine("No .json file was found. Please create an empty file called:\n" + a_name +
                                      ".json");
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 10);
                    success3 = false;
                    ret_early3 = true;
                }
            });
            if (ret_early3)
            {
                return false;
            }

            return true;

        return true;
    }
    }
}