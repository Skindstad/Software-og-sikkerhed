using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        public static void Main(string[] args)
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
            while (isRunning)
            {
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                Encryption(text, bytes);

                //stream.Write(buffer, 0, buffer.Length);
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void Encryption(string e, byte[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                m[i] += 3;
            }

            e = Encoding.UTF8.GetString(m);
            Console.WriteLine(e);

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
            byte[] bytes = new byte[252];
            bool isRunning = true;
            while (isRunning)
            {
                int Read = await stream.ReadAsync(bytes, 0, bytes.Length);
                string text = Encoding.UTF8.GetString(bytes, 0, Read);
                Decryptions(text, bytes);


            }
        }


        public void Decryptions(string e, byte[] m)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(e);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] -= 3;
            }

            e = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(e);
        }
    }
}
