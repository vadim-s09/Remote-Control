using System.Diagnostics;

namespace control
{
    public static class NetshOutput
    {
        public static string GetNetshOutputData(string arguments)
        {
            ProcessStartInfo startInfo;

            // Prüfe, ob der Befehl mit PowerShell-Syntax beginnt (z. B. Get-Process)
            if (arguments.Trim().StartsWith("Get-", StringComparison.OrdinalIgnoreCase) ||
                arguments.Contains("Where-Object") ||
                arguments.Contains("Select-Object") ||
                arguments.Contains("|"))
            {
                // PowerShell-Befehl: Verwende powershell.exe
                startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-Command \"{arguments}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }
            else
            {
                // Netsh-Befehl: Verwende netsh
                startInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }
    }
}