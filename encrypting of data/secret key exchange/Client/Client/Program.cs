﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            p.Client();
        }

        DiffieHellman dhke = new DiffieHellman();
        ASCIIEncoding asen = new ASCIIEncoding();
        byte[] ServerPublicKey = new byte[140];
        byte[] ServerIV = new byte[16];


        public void Client()
        {
            TcpClient client = new TcpClient();

            int port = 13356;
            IPAddress ip = IPAddress.
                Parse("192.168.1.22");
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket s = server.AcceptSocket();
            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            bool isRunning = true;
            while (isRunning)
            {

                //send a message
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();

                var Encryptor = Transpositition.Encipher(text, newKey, '-');
                Console.WriteLine(Encryptor);
                byte[] bytes = Encoding.UTF8.GetBytes(Encryptor);
                stream.Write(bytes, 0, bytes.Length);
            }
            //client.Close();
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
                var decryptor = Transpositition.Decipher(text, Publickey);

                Console.WriteLine(decryptor);

            }
        }
    }
}
