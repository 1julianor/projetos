using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {


        static Criptografia.Crypt cript = new Criptografia.Crypt();
        static Criptografia.Decrypt dec = new Criptografia.Decrypt();
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string ret = cript.Encrypt512("qwerty", "123");
            string decript = dec.Decrypt512(ret, "123");
        

        
        }

        
    }
}
