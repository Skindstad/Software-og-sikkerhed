using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Server();
        }

        public static void Server()
        {
            int port = 13356;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndpoint = new IPEndPoint(ip, port);

            TcpListener listener = new TcpListener(localEndpoint);
            listener.Start();

            Console.WriteLine("Awaiting client");
            TcpClient client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();
            
            ReceiveMessage(stream);

            Console.Write("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            stream.Write(buffer, 0, buffer.Length);

            Console.ReadKey();
        }

        public static async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];

            int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

            Console.Write("\n" + receivedMessage);
        }


    }
}
