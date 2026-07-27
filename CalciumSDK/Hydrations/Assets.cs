using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Hydrations
    {
        public static partial class Assets
        {
            public static List<string> GetPendingChangesList()
            {
                var ret = new List<string>();

                Console.Clear();
                Console.WriteLine("Scanning assets...");
                Console.Out.Flush();
                Thread.Sleep(750);

                var bitmamp_paths = new List<string>();
                var signature_paths = new List<string>();

                string assetsPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT + Path.DirectorySeparatorChar + "assets";

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
                bitmamp_paths.ForEach((bmp) =>
                {
                    var bytes = File.ReadAllBytes(bmp);
                    var sig = File.ReadAllText(bmp.Replace(".bmp", ".signature"));
                    var this_digest = Helpers.GetDigest(bytes);

                    if (this_digest != sig)
                    {
                        ret.Add(bmp.Split(Path.DirectorySeparatorChar).ToList().Last().Replace(".bmp", ""));
                    }
                });

                return ret;
            }
        }
    }
}
