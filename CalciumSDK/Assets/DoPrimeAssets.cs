using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK.Assets
{
    public static partial class DoPrime
    {
        public static void Do()
        {
            var all_assets = new StringBuilder();
            for (int i = 0; i < 9999; i++)
            {
                var bmp_path = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                               Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar +
                               "asset_" + Helpers.GetPaddedNum(i) + ".bmp";

                if (File.Exists(bmp_path))
                {
                    all_assets.Append(AssetFNs.GenerateAssetPPL(bmp_path));
                }
            }
            File.WriteAllText(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT + Path.DirectorySeparatorChar + "dist" + Path.DirectorySeparatorChar + "prime" + Path.DirectorySeparatorChar + "assets.ppl", all_assets.ToString());
        }
    }
}
