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
            Console.Write("Write your Key here: ");
            string secretKey = Console.ReadLine();

            p.MyServer(secretKey);


        }

        private string  Publickey = "b14ca5898a4e4133bbce2ea2315a1916";

        public List<TcpClient> clients = new List<TcpClient>();
        public void MyServer(string key)
        {
            IPAddress ip = IPAddress.Parse("192.168.1.22");
            int port = 13356;
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            AcceptClients(listener, key);

            bool isRunning = true;
            while (isRunning)
            {




                string newKey = Publickey + key;
                byte[] bytekey = Encoding.UTF8.GetBytes(newKey);

                Console.Write("Write your message here: ");
                string text = Console.ReadLine();

                var Encryptor = Transpositition.Encipher(text, newKey, '-');
                Console.WriteLine(Encryptor);

                byte[] bytes = Encoding.UTF8.GetBytes(Encryptor);

                clients.GetStream().Write(bytekey, 0, bytekey.Length);
                //stream.Write(buffer, 0, buffer.Length);
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(bytes, 0, bytes.Length);
                }
            }
        }

        public async void AcceptClients(TcpListener listener, string key)
        {
            bool isRunning = true;
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();
                //byte[] ClientKey = Console.WriteLine(client);
                ReceiveMessage(stream, key);
            }
        }

        public async void ReceiveMessage(NetworkStream stream, string key)
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
