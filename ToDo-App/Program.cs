using System;
using System.Linq;

namespace ToDo_App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Menu();

            Console.WriteLine("Have a nice day with all your to-dos!");
            Console.ReadLine();
        }

        private static void Menu()
        {
            string filepath = "data.json";
            Repository repository = new Repository(new FileService(filepath));

            string input = "";
            string argument = "";
            string command = "";
            bool run = true;

            Console.WriteLine("Welcome to your to-do app\nType HELP for a list of commands");
            while (run)
            {
                Console.Write("~$ ");
                input = Console.ReadLine();
                checkInput();

                switch (command)
                {
                    case "ADD":
                        if (string.IsNullOrEmpty(argument))
                        {
                            Console.WriteLine("Please type a to-do. Example: Add Complete homework assignment");
                            break;
                        }

                        ToDo Todo = repository.Add(argument);
                        Console.WriteLine("#{0} {1}", Todo.id, Todo.description);
                        break;

                    case "DO":
                        int? id = SplitHashtagAndId(argument);

                        if (id != null)
                        {
                            var removedTask = repository.Remove((int)id);
                            if (removedTask == null) { Console.WriteLine("There is no to-do with id #{0}", id); break; }
                            Console.WriteLine("Completed #{0} {1}", removedTask.id, removedTask.description);

                        }
                        else
                            Console.WriteLine("Invalid use of DO. Example: DO #5 ");
                        break;

                    case "PRINT":
                        var todoDirectory = repository.GetAll().Result;

                        if (todoDirectory != null && todoDirectory.Count != 0)
                        {
                            Console.WriteLine("\nTo-do list:");
                            foreach (var element in todoDirectory.OrderBy(x => x.Key))
                            {
                                Console.WriteLine("#{0} {1}", element.Value.id, element.Value.description);
                            }
                        }
                        else
                            Console.WriteLine("Lucky you! You have no to-dos.");
                        break;

                    case "HELP":
                        Console.WriteLine("\nCommands:" +
                            "\nAdd to-do     Add a new to-do" +
                            "\nDO #id        Complete a to-do" +
                            "\nPRINT         Prints all remaining to-dos" +
                            "\nExit          Exit app");

                        break;

                    case "EXIT":
                        run = false;
                        break;

                    default:
                        Console.WriteLine("{0} is not recognized as an command\nType HELP for a list of commands", command);
                        break;
                }
            }

            void checkInput()
            {
                int index = input.IndexOf(' ');
                if (index == -1) // No argument
                {
                    command = input.ToUpper();
                    argument = "";
                }
                else
                {
                    command = input.Substring(0, index).ToUpper();
                    argument = input.Substring(index + 1);
                }
            }
        }

        static public int? SplitHashtagAndId(string argument)
        {
            string idAsString = argument.Split('#').Skip(1).FirstOrDefault();

            if (!string.IsNullOrEmpty(idAsString))
            {
                bool parsed = int.TryParse(idAsString, out int id);
                if (parsed)
                {
                    return id;
                }
            }
            return null;
        }
    }
}
