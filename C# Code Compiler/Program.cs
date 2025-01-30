using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    public static void Main()
    {
        Console.Clear();
        Console.Title = "Code Compiler";
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\t\t\t\tWhat type of code do you want to use?");
        Console.WriteLine("\t\t\t\t[1] C# Code");
        Console.WriteLine("\t\t\t\t[2] Python Code");
        Console.WriteLine("\t\t\t\t[3] Exit");
        Console.WriteLine();

        Console.Write($"root@Compiler:~#");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                HandleCSharpCode();
                break;
            case ConsoleKey.D2:
                HandlePython.HandlePythonCode();
                break;
            case ConsoleKey.D3:
                Environment.Exit(0);
                break;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] Invalid Key Pressed.");
                Console.ReadKey();
                Main();
                break;
        }
    }

    static void HandleCSharpCode()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\tWhat C# code do you want to use?");
        Console.WriteLine("\t\t\t\t[1] Input custom code");
        Console.WriteLine("""                                [2] Load code from file (remove any "" from your code)""");
        Console.WriteLine("\t\t\t\t[3] Default code");
        Console.WriteLine("\t\t\t\t[4] Back to main menu");
        Console.WriteLine();

        Console.Write($"root@Compiler:~#");
        string code;
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("\n[+] Enter your code (write '#end' at the end of your code):");
                var codeBuilder = new StringBuilder();
                string line;
                while ((line = Console.ReadLine()) != "#end")
                {
                    codeBuilder.AppendLine(line);
                }
                code = codeBuilder.ToString();
                ProgramCode.code = code;
                break;
            case ConsoleKey.D2:
                Console.Write("\n[+] Enter the Path to the code file: ");
                string filePath = Console.ReadLine();
                while (!File.Exists(filePath))
                {
                    Console.WriteLine("[-] File not Found!");
                    Console.Write("[+] Enter the Path to the code file: ");
                    filePath = Console.ReadLine();
                }
                code = File.ReadAllText(filePath);
                ProgramCode.code = code;
                break;
            case ConsoleKey.D3:
                code = ProgramCode.code;
                break;
            case ConsoleKey.D4:
                Main();
                return;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] Invalid Key Pressed.");
                Console.ReadKey();
                HandleCSharpCode();
                return;
        }

        Console.WriteLine();
        Console.WriteLine("\t\t\tPlease choose one of the following options:");
        Console.WriteLine("\t\t\t[1] Build with Framework included (big file size + will take longer)");
        Console.WriteLine("\t\t\t[2] Build normal (need .net 8 installed)");
        Console.WriteLine("\t\t\t[3] Back to main menu");
        Console.WriteLine();

        Console.Write($"root@Compiler:~#");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[+] Compiling 'with Framework included'" +
                    " please be patient, this could take a while..." +
                    "\n[i] also, if you have an slow PC, you CPU usage will be high for a couple of seconds");
                SelfContained.CompileCodeSelfContained(ProgramCode.code);
                break;
            case ConsoleKey.D2:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[+] Compiling 'Normal', please be patient...");
                CompileNormal.CompileCode(ProgramCode.code);
                break;
            case ConsoleKey.D3:
                Main();
                break;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] Invalid Key Pressed.");
                Console.ReadKey();
                HandleCSharpCode();
                break;
        }
        ChooseQuitOrBack();
    }

    public static void ChooseQuitOrBack()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n\t\t\tPlease choose one of the following options:");
        Console.WriteLine("\t\t\t[1] Back to Main");
        Console.WriteLine("\t\t\t[2] Exit");
        Console.WriteLine();
        Console.Write($"root@Compiler:~#");

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
                Main();
                break;
            case ConsoleKey.D2:
                Environment.Exit(0);
                break;
        }
    }
}