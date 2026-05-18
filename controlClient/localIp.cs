using System.Text.RegularExpressions;

namespace control
{
    public static class LocalIp
    {
        public static string GetLocalIpAddress()
        {
            string output = NetshOutput.GetNetshOutputData("interface ip show config");
            Match match = Regex.Match(output, @"IP Address:\s+(\d+\.\d+\.\d+\.\d+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "No IP address found";
        }
    }
}
