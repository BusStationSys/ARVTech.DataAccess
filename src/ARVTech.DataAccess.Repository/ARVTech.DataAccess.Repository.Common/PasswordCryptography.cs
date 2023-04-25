// http://pt.stackoverflow.com/questions/25335/gerar-senha-md5-de-32-caracteres-com-o-cryptsharp

namespace ARVTech.DataAccess.Repository.Common
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Class responsible for functions and methos related to password and cryptography.
    /// </summary>
    public static class PasswordCryptography
    {
        /// <summary>
        /// Get hexadecimal string for MD5.
        /// </summary>
        /// <param name="inputValue">String to be passed.</param>
        /// <returns>Content with the hexadecimal string.</returns>
        public static string GetHashMD5(string inputValue)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the valor string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inputValue));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sbBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sbBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sbBuilder.ToString();
            }
        }
    }
}