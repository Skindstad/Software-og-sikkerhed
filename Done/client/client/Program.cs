using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            int number = p.rnd.Next(1000);
            string newKey = "" + p.rnd.Next(1000);
            p.Client( number, newKey);
        }
        Random rnd = new Random(); 
        public void Client(int number, string newKey)
        {
            TcpClient client = new TcpClient();

            int port = 13356;
            IPAddress ip = IPAddress.
                Parse("192.168.1.22");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();
                string publickey = "04f0d72934e866520afd67b36722560153101d9";
                string pubkey = number + publickey + number;
               string newkey = pubkey + newKey;
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(newkey);

            Console.WriteLine("Sending : " + newkey);
            stream.Write(bytesToSend, 0, bytesToSend.Length);

            //---read back the text---
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            string ServerKey = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            
            //Console.WriteLine(ServerKey);
          ReceiveMessage(stream, ServerKey);

            bool isRunning = true;
            while (isRunning)
            {
                //send a message
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();

                var Encryptor = Cipher.Encipher(text, ServerKey, '-');
                //Console.WriteLine(Encryptor);
                byte[] bytes = Encoding.UTF8.GetBytes(Encryptor);

                stream.Write(bytes, 0, bytes.Length);
            }
            //client.Close();
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
