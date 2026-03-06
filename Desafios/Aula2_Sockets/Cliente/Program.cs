using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Cliente
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Criar o cliente e conectar ao IP e Porta do Servidor
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("A conectar ao servidor...");
                tcpclnt.Connect("127.0.0.1", 8080); // O Servidor tem de estar a correr primeiro!
                Console.WriteLine("Conectado!");

                // Pedir os números ao utilizador
                Console.Write("Introduz dois números separados por vírgula (ex: 4,6): ");
                string str = Console.ReadLine() ?? string.Empty;
                
                // Preparar e enviar a mensagem pela Stream
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("A enviar dados...");
                stm.Write(ba, 0, ba.Length);

                // Receber a resposta do Servidor
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                
                string resposta = Encoding.ASCII.GetString(bb, 0, k);
                Console.WriteLine("Resposta do Servidor: " + resposta);

                // Fechar a ligação
                tcpclnt.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: O servidor provavelmente não está a correr. Detalhes: " + e.Message);
            }
        }
    }
}