using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Definir IP (localhost) e a Porta
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            
            // Inicializar o Listener
            TcpListener myList = new TcpListener(ipAd, port);

            try
            {
                // Iniciar a escuta
                myList.Start();
                Console.WriteLine("Servidor a correr na porta " + port);
                Console.WriteLine("À espera de uma conexão...");

                // O programa bloqueia aqui até um cliente se conectar
                Socket s = myList.AcceptSocket();
                Console.WriteLine("Conexão aceite de " + s.RemoteEndPoint);

                // Receber os dados do cliente
                byte[] b = new byte[100];
                int k = s.Receive(b);
                
                // Traduzir os bytes para string (ex: "5,7")
                string mensagemRecebida = Encoding.ASCII.GetString(b, 0, k);
                Console.WriteLine("O Cliente enviou: " + mensagemRecebida);

                // Lógica de negócio: Separar a string e somar
                string[] numeros = mensagemRecebida.Split(',');
                if (numeros.Length == 2 && int.TryParse(numeros[0], out int num1) && int.TryParse(numeros[1], out int num2))
                {
                    int soma = num1 + num2;
                    string resposta = $"A soma de {num1} e {num2} é: {soma}";
                    
                    // Converter a resposta para bytes e enviar
                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes(resposta));
                    Console.WriteLine("Resposta enviada ao cliente.");
                }
                else
                {
                    s.Send(Encoding.ASCII.GetBytes("Erro: Formato inválido. Envia no formato 'numero,numero'."));
                }

                // Limpar a casa (fechar ligações)
                s.Close();
                myList.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.StackTrace);
            }
        }
    }
}