using System.Diagnostics;

public class PythonStuff
{
    public static void RunPythonScript(string scriptPath)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[+] Running Python script...");

        try
        {
            var processInfo = new ProcessStartInfo("python", scriptPath)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[-] Error running script:");
                    Console.WriteLine(error);
                }
                else
                {
                    Console.WriteLine("\n[+] Script output:");
                    Console.WriteLine(output);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[-] Error: {ex.Message}");
        }
    }

    public static void cmd(string command)
    {
        ProcessStartInfo pro = new ProcessStartInfo("cmd.exe", "/C " + command);
        pro.WindowStyle = ProcessWindowStyle.Hidden;
        pro.CreateNoWindow = true;
        Process.Start(pro);
    }

    public static void CreatePythonExecutable(string scriptPath)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[+] Creating executable using PyInstaller...");

        try
        {
            var checkPyInstaller = new ProcessStartInfo("pip", "show pyinstaller")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(checkPyInstaller))
            {
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine("\n[+] PyInstaller not found. Installing PyInstaller...");
                    var installProcess = Process.Start(new ProcessStartInfo("pip", "install pyinstaller")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    installProcess.WaitForExit();
                }
            }


            try
            {
                cmd($"pyinstaller --onefile \"{scriptPath}\"");
                Thread.Sleep(7000); // this is required because generating the exe is taking a while.
                Console.WriteLine("\n[+] Executable created successfully!");
                Console.WriteLine("[+] You can find it in the 'dist' folder");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[-] Error creating executable:");
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[-] Error: {ex.Message}");
        }
    }
}