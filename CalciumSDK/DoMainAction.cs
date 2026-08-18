using System;
using System.Collections.Generic;
using System.Text;

namespace CalciumSDK
{
    public static partial class Program
    {
        public static void DoMainAction(string action, string projectName)
        {
            switch(action)
            {
                case "A":
                    Console.WriteLine("Hydrating Assets Preflight Check...");
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 2);
                    var success = Preflight.VerifyAssetsForHydration(projectName);
                    if(!success)
                    {
                        return;
                    }
                    var pending_changes = Hydrations.Assets.GetPendingChangesList();
                    if(pending_changes == null || pending_changes.Count == 0)
                    {
                        Console.WriteLine("No pending asset changes.");
                        Console.Out.Flush();
                        Thread.Sleep(1000 * 3);
                        break;
                    }
                    pending_changes.ForEach((pc) =>
                    {
                        Console.WriteLine("Detected change w/" + pc);
                        Console.Out.Flush();
                    });
                    Thread.Sleep(1000 * 2);

                    pending_changes.ForEach((pc) =>
                    {
                        var path = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + SELECTED_PROJECT + Path.DirectorySeparatorChar + "assets" + Path.DirectorySeparatorChar + pc + ".bmp";
                        var bytes = File.ReadAllBytes(path);
                        var hash = Helpers.GetDigest(bytes);
                        File.WriteAllText(path.Replace(".bmp", ".signature"), hash);

                        var asset_sb = Assets.GenerateAssetPPL(path);

                    });
                    break;
                case "B":
                    var success2 = Preflight.VerifyScenesForHydration(projectName);
                    if (!success2)
                    {
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
