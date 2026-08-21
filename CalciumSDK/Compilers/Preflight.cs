namespace CalciumSDK.Compilers;

public class Preflight
{
    public static void VerifySceneVariables()
    {
        var targets = Program.RootConfig.compilation_targets;
        targets.ForEach((target) =>
        {
            
        });
    }
}