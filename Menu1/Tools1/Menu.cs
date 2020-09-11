/////////////////////////////////////////
// 
// Uses "Console App(.NET Core)" template
// C:\Users\ron\OneDrive\Desktop\My first git>dotnet --list-sdks
// 3.1.302 [C:\Program Files\dotnet\sdk]
//
/////////////////////////////////////////

using System;
using Utilities;

class Menu
{
    ManageArchiveClass mac = new ManageArchiveClass();

    public void MakeMenuCalls()
    {
        string command;
        while (true)
        {
            Console.WriteLine("Enter command");
            command = Console.ReadLine();
            switch (command)
            {
                case "m":
                    mac.ExecuteProcess();
                        break;
                case "traverse":
//                    StackBasedIteration.TraverseTest();
                    // Submit command to generate list of files
                    break;
                case "t2":
                    // Submit command to generate list of files
                    // uing new, improved logic
//                   StackBasedIteration.TraverseTest2();
                    break;
                case "readdirlist":
                // Legacy Commands
                case "read":
//                   ReadFiles.ReadFileNames();
                    break;
                case "test":
                    Console.WriteLine("This is a test command");
                    break;
                case "p":
//                   StackBasedIteration.GetLengthOfPaths();
                    break;
                case "quit":
                case "q":
                case "exit":
                    Console.WriteLine("Ending program");
                    return;
                default:
                    Console.WriteLine("BAD COMMAND. Type \"quit\" or try again");
                    break;
            }
        }
    }

}
    