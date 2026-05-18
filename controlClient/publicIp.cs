using System.Net.Http;

namespace control
{
    public static class PublicIp
    {
        public static async Task<string> GetPublicIpAddress()
        {
            using HttpClient client = new HttpClient();
            string ip = await client.GetStringAsync("https://api.ipify.org");
            return ip.Trim();
        }
    }
}
