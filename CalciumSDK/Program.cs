namespace CalciumSDK
{
    public static partial class Program
    {
        static void Main(string[] args)
        {
            Program.ShowWelcome();
            while(true)
            {
                var main_action = Program.PromptForInitialAction();
                switch(main_action)
                {
                    case "1":
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
}
