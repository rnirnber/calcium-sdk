namespace CalciumSDK;

public static partial class Program
{
    public static partial class Preflight
    {
        public static bool VerifyScenesForHydration(string projectName)
        {
            var all_files = Directory.GetFiles(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "scenes").ToList();
            var all_valid = new List<string>();
            for(int i = 1; i <= 9999; i++)
            {
                all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + "scene_" + Helpers.GetPaddedNum(i) + ".bmp");
                all_valid.Add(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + "scenes" + Path.DirectorySeparatorChar + "scene_" + Helpers.GetPaddedNum(i) + ".signature");
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
                    Console.WriteLine("The following file should not exist in the scenes folder:");
                    Console.WriteLine(f.Split(Path.DirectorySeparatorChar).ToList().Last());
                    Console.Out.Flush();
                    Thread.Sleep(1000 * 8);
                    success0 = false;
                    ret_early0 = true;
                    
                }
            });
            return success0;
        }
    }
}