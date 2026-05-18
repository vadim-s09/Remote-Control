namespace control
{
    public static class ConnectionPassword
    {
        public static string GetConnectionPassword(string[] filterPassword)
        {
            foreach (string item in filterPassword)
            {
                if (item.Trim().StartsWith("Key Content"))
                {
                    string password = item.Split(':')[1].Trim();
                    return password;
                }
            }
            return "No password found";
        }
    }
}