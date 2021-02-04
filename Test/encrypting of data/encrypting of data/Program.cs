using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encrypting_of_data
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            var key = p.key;

            Console.WriteLine("Please enter a sting for encryption");
            var str = Console.ReadLine();
            var encryptedTranspositition = Transpositition.Encipher(str, key, '-');
            Console.WriteLine($"encrypted string = {encryptedTranspositition}");

            var decryptedTranspositition = Transpositition.Decipher(encryptedTranspositition, key);
            Console.WriteLine($"decrypted string = {decryptedTranspositition}");

            Console.ReadKey();

        }

        private string key = "b14ca5898a4e4133bbce2ea2315a1916";

    }
}
