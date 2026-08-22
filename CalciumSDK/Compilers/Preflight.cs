using System.Runtime.CompilerServices;
using System.Text.Json;
using CalciumSDK.Models;

namespace CalciumSDK.Compilers;

public static class Preflight
{
    public static bool VerifySceneTargets()
    {
        var targets = Program.RootConfig.compilation_targets;
        var ret = true;
        var ret_early = false;
        targets.ForEach((target) =>
        {
            if (ret_early)
            {
                return;
            }
            string scenesPath = Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar + Program.SELECTED_PROJECT +
                                Path.DirectorySeparatorChar + "scenes";

            var root_break_early = false;
            for (int i = 1; i <= 10000; i++)
            {
                if (root_break_early)
                {
                    continue;
                }
                var path = scenesPath + Path.DirectorySeparatorChar + "scene_" + Helpers.GetPaddedNum(i) + ".json";
                if (!File.Exists(path))
                {
                    return;
                }
                var this_scene = JsonSerializer.Deserialize<SceneBlueprint>(
                    File.ReadAllText(path), AppJsonContext.Default.SceneBlueprint);

                if (!this_scene.on_exit_statements.ContainsKey(target))
                {
                    Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_exit_statements\". An empty array should suffice. (" + target + ")");
                    Console.Out.Flush();
                    ret_early = true;
                    ret = false;
                    root_break_early = true;
                }
                if (!this_scene.on_entrance_statements.ContainsKey(target))
                {
                    Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_entrance_statements\". An empty array should suffice. (" + target + ")");
                    Console.Out.Flush();
                    ret_early = true;
                    ret = false;
                    root_break_early = true;
                }
                if (!this_scene.follower_enabled_statements.ContainsKey(target))
                {
                    Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"follower_enabled_statements\". An empty array should suffice. (" + target + ")");
                    Console.Out.Flush();
                    ret_early = true;
                    ret = false;
                    root_break_early = true;
                }

                var ret_early2 = false;
                this_scene.dialogs.ForEach((dialog) =>
                {
                    if (ret_early2)
                    {
                        return;
                    }
                    if (!dialog.on_finished_callback_statements.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_finished_callback_statements\" inside a dialog. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early2 = true;
                        ret = false;
                        root_break_early = true;
                    }
                    if (!dialog.eligibility_statements.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"eligibility_statements\" inside a dialog. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early2 = true;
                        ret = false;
                        root_break_early = true;
                    }
                    if (!dialog.on_eligibility_failed_statements.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_eligibility_failed_statements\" inside a dialog. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early2 = true;
                        ret = false;
                        root_break_early = true;
                    }
                });
                var ret_early3 = false;
                this_scene.warps.ForEach((warp) =>
                {
                    if (ret_early3)
                    {
                        return;
                    }
                    if (!warp.allowed_conditions.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"allowed_conditions\" inside a warp. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early3 = true;
                        ret = false;
                        root_break_early = true;
                        
                    }
                    if (!warp.on_finished_statements.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_finished_statements\" inside a warp. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early3 = true;
                        ret = false;
                        root_break_early = true;
                    }
                    if (!warp.on_failed_statements.ContainsKey(target))
                    {
                        Console.WriteLine("Scene #" + Helpers.GetPaddedNum(i) + " does not have data for \"on_failed_statements\" inside a warp. An empty array should suffice. (" + target + ")");
                        Console.Out.Flush();
                        ret_early3 = true;
                        ret = false;
                        root_break_early = true;
                    }
                });
            }
        });
        if (!ret)
        {
            Thread.Sleep(1000 * 10);
        }
        return ret;
    }
}