using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a TcpClient and connect to the server's IP and Port
                using TcpClient client = new TcpClient();
                Console.WriteLine("Connecting to server (127.0.0.1:8080)...");
                client.Connect("127.0.0.1", 8080); 
                Console.WriteLine("Connected successfully!\n");

                // Display operation menu
                Console.WriteLine("=== OPERATION MENU ===");
                Console.WriteLine("1. Sum two numbers");
                Console.WriteLine("2. Get a random number");
                Console.WriteLine("3. Get server current date");
                Console.Write("Choose an option (1-3): ");
                string choice = Console.ReadLine() ?? string.Empty;

                string messageToSend = "";

                // Format the request based on the protocol "COMMAND|data"
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter first number: ");
                        string n1 = Console.ReadLine() ?? "0";
                        Console.Write("Enter second number: ");
                        string n2 = Console.ReadLine() ?? "0";
                        messageToSend = $"SUM|{n1},{n2}"; 
                        break;
                    case "2":
                        messageToSend = "RANDOM|";
                        break;
                    case "3":
                        messageToSend = "DATE|";
                        break;
                    default:
                        messageToSend = "UNKNOWN|";
                        break;
                }
                
                // Get the network stream and send the encoded message
                using NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(messageToSend);
                
                Console.WriteLine("Sending request...");
                stream.Write(data, 0, data.Length);

                // Buffer to receive the server's response
                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, 1024);
                
                string serverResponse = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                Console.WriteLine("\n[SERVER RESPONSE]: " + serverResponse);

                // Closing connection automatically handled by the 'using' statement
                Console.WriteLine("\nConnection closed. Press Enter to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client Error: {ex.Message}");
            }
        }
    }
}