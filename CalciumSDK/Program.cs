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
                        var project_name = Program.PromptForProjectName();
                        CreateProject(project_name);
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
