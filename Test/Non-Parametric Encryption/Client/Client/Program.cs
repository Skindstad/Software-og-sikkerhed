using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Client();
        }

        public void Client()
        {
            TcpClient client = new TcpClient();

            int port = 13356;
            IPAddress ip = IPAddress.
                Parse("192.168.1.22");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            bool isRunning = true;
            while (isRunning)
            {
                //send a message
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();
                byte[] bytes = Encoding.UTF8.GetBytes(text);

                Encryption(text, bytes);

                stream.Write(bytes, 0, bytes.Length);
            }
            //client.Close();
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
