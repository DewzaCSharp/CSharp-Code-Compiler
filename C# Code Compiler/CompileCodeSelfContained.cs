using System.Diagnostics;
using System.Text;

public class SelfContained
{
    private static readonly string[] RequiredUsings =
    {
    "using System;",
    "using System.IO;",
    "using System.Text;",
    "using System.Linq;",
    "using System.Collections.Generic;",
    "using System.Diagnostics;",
    "using System.Threading;",
    "using System.Threading.Tasks;",
    "using System.Net;",
    "using System.Net.Http;",
    "using System.Net.Sockets;",
    "using System.Reflection;",
    "using System.Globalization;", 
    "using System.Security.Cryptography;",
    "using System.Runtime.InteropServices;",  
    "using System.Text.RegularExpressions;",
    "using System.Collections.Concurrent;",
    "using System.ComponentModel;",
    "using System.Data;",
    "using System.Configuration;",
    "using System.Media;",  
    "using System.Drawing;",
    "using System.Runtime.Serialization;",
    "using System.Xml;",
    "using System.Xml.Linq;",
    "using System.Text.Json;" 
};
    public static void CompileCodeSelfContained(string sourceCode)
    {
        string tempProjectPath = Path.Combine(Path.GetTempPath(), "Program" + Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempProjectPath);

        try
        {
            string csprojContent = @"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>
</Project>";

            File.WriteAllText(Path.Combine(tempProjectPath, "Program.csproj"), csprojContent);

            sourceCode = EnsureRequiredUsings(sourceCode);

            File.WriteAllText(Path.Combine(tempProjectPath, "Program.cs"), sourceCode);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "publish -c Release",
                WorkingDirectory = tempProjectPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var output = new StringBuilder();
                var error = new StringBuilder();

                process.OutputDataReceived += (sender, e) => {
                    if (e.Data != null) output.AppendLine(e.Data);
                };
                process.ErrorDataReceived += (sender, e) => {
                    if (e.Data != null) error.AppendLine(e.Data);
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    string publishDir = Path.Combine(tempProjectPath, "bin", "Release", "net8.0", "win-x64", "publish");
                    string exePath = Path.Combine(publishDir, "Program.exe");
                    string targetPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedProgram.exe");

                    File.Copy(exePath, targetPath, true);
                    Console.WriteLine($"[+] Successfully compiled EXE: {targetPath}");
                    Program.ChooseQuitOrBack();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[-] Compilation Failed!");
                    Console.WriteLine("\nError Details:");
                    Console.WriteLine(error.ToString());
                    Console.WriteLine("\nBuild Output:");
                    Console.WriteLine(output.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
        }
        finally
        {
            try
            {
                Directory.Delete(tempProjectPath, true);
            }
            catch { }
        }
    }
    private static string EnsureRequiredUsings(string code)
    {
        var existingUsings = new HashSet<string>(code.Split('\n').Where(line => line.StartsWith("using ")).Select(line => line.Trim()));

        var missingUsings = RequiredUsings.Where(u => !existingUsings.Contains(u)).ToList();

        if (missingUsings.Count > 0)
        {
            code = string.Join("\n", missingUsings) + "\n\n" + code;
        }

        return code;
    }
}