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
            int number = p.rnd.Next(1000);
            string newKey = "" + p.rnd.Next(1000);
            p.MyServer(number, newKey);

        }

        private readonly Random rnd = new Random();
        private string key;
        public List<TcpClient> clients = new List<TcpClient>();
        public void MyServer(int number, string newKey)
        {
            IPAddress ip = IPAddress.Parse("192.168.1.22");
            int port = 13356;
            TcpListener listener = new TcpListener(ip, port);
            //Console.WriteLine("Listening...");
            listener.Start();

            AcceptClients(listener, number, newKey);


            bool isRunning = true;
            while (isRunning)
            {
                // need a key to use
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();

                var Encryptor = Cipher.Encipher(text, key, '-');
                //Console.WriteLine(Encryptor);

                byte[] bytes = Encoding.UTF8.GetBytes(Encryptor);


                //stream.Write(buffer, 0, buffer.Length);
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(bytes, 0, bytes.Length);
                }
            }
        }

        public async void AcceptClients(TcpListener listener, int number, string newKey )
        {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();

            // control over the key
             string publickey = "04f0d72934e866520afd67b36722560153101d9";
                publickey = number + publickey + number;
                 newKey =   publickey + newKey;
               string keyhold = KeyHolder(stream, client, newKey);

                    key = keyhold;

                ReceiveMessage(stream, key);
        }
        public string KeyHolder(NetworkStream stream, TcpClient client, string key)
        {
            // control over the key
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string ClientKey = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received : " + ClientKey);
            key += ClientKey;
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(key);
            Console.WriteLine("Sending back : " + key);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            return key;
        }

        public async void ReceiveMessage(NetworkStream stream, string key)
        {
            byte[] bytes = new byte[252];
            bool isRunning = true;
            while (isRunning)
            {
                int Read = await stream.ReadAsync(bytes, 0, bytes.Length);
                string text = Encoding.UTF8.GetString(bytes, 0, Read);
                //Console.WriteLine(text);
                var decryptor = Cipher.Decipher(text, key);

                Console.WriteLine(decryptor);


            }
        }
    }
}
