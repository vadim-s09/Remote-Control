using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace controlHost
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;
            TcpListener server = null;

            while (true)
            {
                try
                {
                    // Create a TCP/IP socket.
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    Console.WriteLine($"Server started on port {port}.");
                    break; // Exit the loop if successful
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Console.WriteLine($"Port {port} is already in use. Trying port {port + 1}...");
                    port++;
                    if (port > 5100) // Prevent infinite loop
                    {
                        Console.WriteLine("Unable to find an available port. Exiting.");
                        return;
                    }
                }
            }

            while (true)
            {
                // Wait for a client to connect.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                NetworkStream stream = client.GetStream();

                // Lock object for synchronizing stream access
                object streamLock = new object();

                // Cancellation token for stopping threads
                CancellationTokenSource cts = new CancellationTokenSource();

                // Thread for sending messages to the client
                Thread sendThread = new Thread(() =>
                {
                    try
                    {
                        while (!cts.Token.IsCancellationRequested && client.Connected)
                        {
                            string message = Console.ReadLine();
                            if (string.IsNullOrEmpty(message) || message.ToLower() == "exit")
                                break;

                            byte[] data = Encoding.UTF8.GetBytes(message);
                            lock (streamLock)
                            {
                                stream.Write(data, 0, data.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Send Error: " + ex.Message);
                    }
                });
                sendThread.Start();

                // Handle the client connection in a separate thread.
                Thread clientThread = new Thread(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Read data from the client until the connection is closed.
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            Console.WriteLine("Received: " + message);

                            // Optional: Echo back if needed, but synchronized
                            // Uncomment the lines below if you want to echo messages back
                            // byte[] response = Encoding.UTF8.GetBytes("Echo: " + message);
                            // lock (streamLock)
                            // {
                            //     stream.Write(response, 0, response.Length);
                            // }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Receive Error: " + ex.Message);
                    }
                    finally
                    {
                        lock (streamLock)
                        {
                            client.Close();
                        }
                        Console.WriteLine("Client disconnected.");
                        // Cancel the send thread
                        cts.Cancel();
                    }
                });
                clientThread.Start();
            }
        }
    }
}
