namespace CalciumSDK
{
    public static partial class Program
    {
        static void Main(string[] args)
        {
            Program.ShowWelcome();
            var breakMainLoop = false;

            while (true)
            {
                
                var main_action = Program.PromptForInitialAction();
                switch(main_action)
                {
                    case "1":
                        var success = false;
                        while(!success)
                        {
                            var project_name = Program.PromptForProjectName();
                            success = Program.TryCreateProject(project_name);
                        }
                        breakMainLoop = true;
                        break;
                    default:
                        break;
                }
                if(breakMainLoop)
                {
                    break;
                }
            }
            Console.WriteLine("finished...");
        }
        
    }
}
