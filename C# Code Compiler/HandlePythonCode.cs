using System.Text;

public class HandlePython
{
    public static void HandlePythonCode()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\tWhat Python code do you want to use?");
        Console.WriteLine("\t\t\t\t[1] Input custom code");
        Console.WriteLine("\t\t\t\t[2] Load code from file");
        Console.WriteLine("\t\t\t\t[3] Back to main menu");
        Console.WriteLine();

        Console.Write($"root@Compiler:~#");
        string pythonCode = "";
        string outputPath = "";

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("\n[+] Enter your Python code (write '#end' at the end of your code):");
                var codeBuilder = new StringBuilder();
                string line;
                while ((line = Console.ReadLine()) != "#end")
                {
                    codeBuilder.AppendLine(line);
                }
                pythonCode = codeBuilder.ToString();

                Console.Write("[+] Enter the output path for your Python file: ");
                outputPath = Console.ReadLine();
                File.WriteAllText(outputPath, pythonCode);
                break;

            case ConsoleKey.D2:
                Console.Write("\n[+] Enter the Path to the Python file: ");
                string filePath = Console.ReadLine();
                while (!File.Exists(filePath))
                {
                    Console.WriteLine("[-] File not Found!");
                    Console.Write("[+] Enter the Path to the Python file: ");
                    filePath = Console.ReadLine();
                }
                outputPath = filePath;
                break;

            case ConsoleKey.D3:
                Program.Main();
                return;

            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] Invalid Key Pressed.");
                Console.ReadKey();
                HandlePythonCode();
                return;
        }

        Console.WriteLine();
        Console.WriteLine("\t\t\tPlease choose one of the following options:");
        Console.WriteLine("\t\t\t[1] Run Python script");
        Console.WriteLine("\t\t\t[2] Create executable (using PyInstaller)");
        Console.WriteLine("\t\t\t[3] Back to main menu");
        Console.WriteLine();

        Console.Write($"root@Compiler:~#");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                PythonStuff.RunPythonScript(outputPath);
                break;
            case ConsoleKey.D2:
                PythonStuff.CreatePythonExecutable(outputPath);
                break;
            case ConsoleKey.D3:
                Program.Main();
                break;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] Invalid Key Pressed.");
                Console.ReadKey();
                HandlePythonCode();
                break;
        }
        Program.ChooseQuitOrBack();
    }
}