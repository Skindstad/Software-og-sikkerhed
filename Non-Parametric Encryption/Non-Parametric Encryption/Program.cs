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
            Console.WriteLine("Please write something: ");
            string message = Console.ReadLine();

            p.Encryption(message);

            Console.ReadKey();

        }

        public void Encryption(string e)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(e);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] += 3;
            }

            e = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(e);

            Decryptions(e);

        }

        public void Decryptions(string e)
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
