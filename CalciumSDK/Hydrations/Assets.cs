using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Hydrations
    {
        public static partial class Assets
        {
            public static bool ListAndConfirm()
            {
                Console.Clear();
                Console.WriteLine("Scanning assets...");
                Thread.Sleep(750);

                var bitmamp_paths = new List<string>();
                var signature_paths = new List<string>();

                string assetsPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + "assets";

                var valid_asset_names = new List<string>();
                var valid_bitmap_names = new List<string>();
                var valid_signature_names = new List<string>();
                for(int i = 1; i <= 9999; i++)
                {
                    valid_asset_names.Add("asset_" + Helpers.GetPaddedNum(i));
                    valid_bitmap_names.Add("asset_" + Helpers.GetPaddedNum(i) + ".bmp");
                    valid_signature_names.Add("asset_" + Helpers.GetPaddedNum(i) + ".signature");
                }

                var bitmaps = Directory.GetFiles(assetsPath).ToList().Where((f) => f.IndexOf(".bmp") > -1).ToList();

                return true;
            }
        }
    }
}
