using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace MultiServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
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
            while (isRunning) { 
            Console.Write("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

             //stream.Write(buffer, 0, buffer.Length);
             foreach(TcpClient client in clients)
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
                Console.Write("client writes: " + text);
            }
        }

    }
}
