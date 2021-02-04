using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class cipher
    {

        public readonly byte[] publicKey = new byte[32];

        public static char Caesar(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);


        }
        public static string Encipher(string text, int key)
            {
            string output = string.Empty;

            foreach (char ch in text)
                output += Caesar(ch, key);

            return output;
        }
        public static string Decipher(string text, int key)
            {
            
            return Encipher(text, 26 - key);
        }




    }
}
