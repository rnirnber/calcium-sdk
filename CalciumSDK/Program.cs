using System.Runtime.InteropServices;

namespace CalciumSDK
{
    public static partial class Program
    {
        static void Main(string[] args)
        {
            Program.ShowWelcome();
            var breakMainLoop = false;
            var proceedToSelectProject = false;
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
                            if(!success)
                            {
                                Console.Clear();
                                Console.WriteLine("There was an error creating the project. Please try again.");
                                System.Threading.Thread.Sleep(2000);
                            }
                            break;
                        }
                        break;
                    case "2":
                        breakMainLoop = true;
                        proceedToSelectProject = true;
                        break;
                    default:
                        Console.WriteLine("You must type \"1\" or \"2\" to provide a valid action.");
                        System.Threading.Thread.Sleep(2000);
                        break;
                }
                if(breakMainLoop)
                {
                    break;
                }
            }
            var hasSelected = false;
            while(!hasSelected)
            {
                Console.Clear();
                var directories = Directory.EnumerateDirectories(Helpers.GET_ROOT_SDK_PATH() + Path.DirectorySeparatorChar).ToList();
                if (directories.Count > 9)
                {
                    Console.WriteLine("It appears that there are more than 9 projects. Unfortunately, you'll need to delete one in order to proceed. Press any key to try again.");
                    Console.Clear();
                    var _ = Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please type a number and hit enter for selecting your project:");
                    Console.WriteLine();
                    Console.WriteLine();

                    var project_names = directories.Select((dir) => dir.Split(Path.DirectorySeparatorChar).ToList().Last()).ToList();
                    var final_project_names = new List<string>();
                    final_project_names.AddRange(project_names);

                    if(final_project_names.Count < 10)
                    {
                        var last_idx = final_project_names.Count;
                        for(int i = last_idx; i < 9; i++)
                        {
                            final_project_names.Add("Empty Space [No Project]");
                        }   
                    }
                    var idx = 1;
                    final_project_names.ForEach((dir) =>
                    {
                        Console.WriteLine(idx.ToString() + "): " + dir);
                        idx++;
                    });
                    Console.WriteLine();                    
                    var begin_idx = 1;
                    var end_idx = project_names.Count();

                    var selection_is_valid = false;
                    while(!selection_is_valid)
                    {
                        Console.Clear();
                        Console.WriteLine("Please type a number and hit enter for selecting your project:");
                        
                        var idx2 = 1;
                        final_project_names.ForEach((dir) =>
                        {
                            Console.WriteLine(idx2.ToString() + "): " + dir);
                            idx2++;
                        });
                        var selected_project = Console.ReadLine().Trim();
                        try
                        {
                            if (Convert.ToInt32(selected_project) >= begin_idx && Convert.ToInt32(selected_project) <= end_idx)
                            {
                                Console.Clear();
                                Console.WriteLine("Selected Project Valid, Proceeding...");
                                Thread.Sleep(1000);
                                selection_is_valid = true;
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("You cannot select an empty project:");
                                Thread.Sleep(2000);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Selected project is invalid");
                            Thread.Sleep(2000);
                        }
                    }
                    hasSelected = true;
                }
            }
            Console.Clear();
            Console.WriteLine("To be continued...");
        }
        
    }
}
