using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            p.MyServer();


        }

        private string key = "b14ca5898a4e4133bbce2ea2315a1916";

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

                var Encryptor = Transpositition.Encipher(text, key, '-');
                Console.WriteLine(Encryptor);

                byte[] bytes = Encoding.UTF8.GetBytes(Encryptor);
                

                //stream.Write(buffer, 0, buffer.Length);
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(bytes, 0, bytes.Length);
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
            byte[] bytes = new byte[252];
            bool isRunning = true;
            while (isRunning)
            {
                int Read = await stream.ReadAsync(bytes, 0, bytes.Length);
                string text = Encoding.UTF8.GetString(bytes, 0, Read);
                Console.WriteLine(text);
                var decryptor = Transpositition.Decipher(text, key);

                Console.WriteLine(decryptor);


            }
        }

    }
}
