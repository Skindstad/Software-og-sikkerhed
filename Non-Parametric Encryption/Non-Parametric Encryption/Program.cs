using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Non_Parametric_Encryption
{
    class Program
    {
       public static void Main(string[] args)
        {
            Program p = new Program();

            string message = "Hej med dig";
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] += 3;
            }

            message = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(message);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] -= 3;
            }

            message = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(message);

            p.MyServer();
        }

        public List<TcpClient> clients = new List<TcpClient>();
        public void MyServer()
        {
            IPAddress ip = IPAddress.Parse("192.168.1.22");
            int port = 13356;
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            AcceptClients(listener);

            bool isRunning = true;
            while (isRunning)
            {
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] += 3;

                }
                    //stream.Write(buffer, 0, buffer.Length);
                    foreach (TcpClient client in clients)
                    {
                        client.GetStream().Write(buffer, 0, buffer.Length);
                    }
                
            }
        }
        public async void AcceptClients(TcpListener listener)
        {
            bool isRunning = true;
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();
                ReceiveMessage(stream);
            }
        }
        public async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];
            bool isRunning = true;
            while (isRunning)
            {
                int Read = await stream.ReadAsync(buffer, 0, buffer.Length);
                string text = Encoding.UTF8.GetString(buffer, 0, Read);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] -= 3;
                }
                text = Encoding.UTF8.GetString(buffer);
                Console.Write("client writes: " + text);
            }
        }
    }
}
