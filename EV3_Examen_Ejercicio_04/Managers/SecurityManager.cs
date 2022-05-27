using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EV3_Examen_Ejercicio_04.Managers
{
    public static class SecurityManager
    {
        /// <summary>
        /// Hashea un string introducido por el usuario
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns>Una array de bytes de ese string haseado</returns>
        private static byte[] ComputeHash(string inputMessage)
        {
            SHA512 sha512Algorithm = SHA512.Create();

            byte[] hashedBytes;
            byte[] inputButes = System.Text.Encoding.UTF8.GetBytes(inputMessage);


            try
            {
                hashedBytes = sha512Algorithm.ComputeHash(inputButes);
            }
            finally
            {
                if (sha512Algorithm != null)
                {
                    sha512Algorithm.Dispose();
                }
            }


            return hashedBytes;
        }

        /// <summary>
        /// Convierte un array de bytes a formato hexadecimal
        /// </summary>
        /// <param name="hashedBytes"></param>
        /// <returns></returns>
        private static string ToHexString(byte[] hashedBytes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.AppendFormat("{0:X2}", hashedBytes[i]);
            }
            return sb.ToString();

        }

        /// <summary>
        /// Convierte un string introducido por el usuario en un string hasheado en formato hexadecimal
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        private static string GetHexHash(string pass)
        {
            byte[] hashedBytes = ComputeHash(pass);
            string hexHasedBytes = string.Empty;
            if (hashedBytes != null)
            {
                hexHasedBytes = ToHexString(hashedBytes);
            }
            return hexHasedBytes;
        }
    
        /// <summary>
        /// Comprueba si el Hash introducido por el usuario es igual al que deberia ser 
        /// </summary>
        /// <param name="inputPass"></param>
        /// <param name="ValidPass"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void VerifyHash(string inputPass, string ValidPass)
        {
            if (string.IsNullOrEmpty(inputPass))
            {
                throw new ArgumentException("No se ha introducido ningun hash");
            }

            string inputHash = GetHexHash(inputPass);

            if (string.Equals(inputHash, ValidPass))
            {
                Console.WriteLine("Acceso Concedido");
            }
            else
            {
                Console.WriteLine("Acceso Denegado");
            }
        }

    }
}
