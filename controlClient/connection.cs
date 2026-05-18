namespace control
{
    public static class Connection
    {
        public static string GetCurrentConnection(string[] filterNames)
        {
            foreach (string item in filterNames)
            {
                if (item.Trim().StartsWith("SSID"))
                {
                    string ssid = item.Split(':')[1].Trim();
                    return ssid;
                }
            }
            return "No SSID found";
        }

    }
}