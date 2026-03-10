using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the IP address (localhost) and Port number
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            
            // Initialize the TCP Listener
            TcpListener listener = new TcpListener(ipAddress, port);

            try
            {
                // Start listening for incoming connections
                listener.Start();
                Console.WriteLine($"Server is running on port {port}...");
                Console.WriteLine("Waiting for a connection...");

                // Accept the incoming socket connection (Blocking call)
                using Socket clientSocket = listener.AcceptSocket();
                Console.WriteLine($"Connection accepted from {clientSocket.RemoteEndPoint}");

                // Buffer to store received data
                byte[] buffer = new byte[1024];
                int receivedBytes = clientSocket.Receive(buffer);
                
                // Convert bytes to string and trim any whitespace
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();
                Console.WriteLine($"Client sent: {receivedMessage}");

                string response = "";

                // Split the message using '|' to separate Command from Data
                string[] parts = receivedMessage.Split('|');
                string command = parts[0].ToUpper();

                // Logic to handle different commands
                switch (command)
                {
                    case "SUM": // Operation: Sum two numbers
                        if (parts.Length > 1)
                        {
                            string[] numbers = parts[1].Split(',');
                            if (numbers.Length == 2 && int.TryParse(numbers[0], out int n1) && int.TryParse(numbers[1], out int n2))
                            {
                                response = $"The sum of {n1} and {n2} is: {n1 + n2}";
                            }
                            else 
                            { 
                                response = "Error: Invalid format. Use SUM|n1,n2"; 
                            }
                        }
                        break;

                    case "RANDOM": // Operation: Generate a random number
                        Random rnd = new Random();
                        response = $"Your random number is: {rnd.Next(1, 1001)}";
                        break;

                    case "DATE": // Operation: Get server local date and time
                        response = $"Server local time: {DateTime.Now}";
                        break;

                    default:
                        response = "Unknown command. Try: SUM|x,y , RANDOM| or DATE|";
                        break;
                }

                // Convert response to bytes and send it back to the client
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                clientSocket.Send(responseData);
                Console.WriteLine("Response sent to the client.");

                // Close the client socket
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error: {ex.Message}");
            }
            finally
            {
                // Stop the listener
                listener.Stop();
            }
        }
    }
}