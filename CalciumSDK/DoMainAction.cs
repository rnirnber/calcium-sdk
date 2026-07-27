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
                    Thread.Sleep(1000 * 3);
                    break;
                default:
                    break;
            }
        }
    }
}
