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
                    Thread.Sleep(1000 * 2);
                    var success = Preflight.VerifyAssetsForHydration(projectName);
                    if(!success)
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
