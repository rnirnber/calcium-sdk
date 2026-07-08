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

                var signature_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "assets").ToList()
                .Where((f) => f.IndexOf(".signature") > 0)
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
                var x = 5;

                return true;
            }
        }
    }
}
