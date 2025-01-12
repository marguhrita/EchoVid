using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace EchoVid.Model.Platforms
{
    internal class TikTok : Platform
    {

        public string OAUTH_Endpoint => "https://www.google.com";
        private string Redirect_URI => "http://localhost:3455/callback/";

        public string test = "HELLO";

        public override void Authenticate()
        {
            test = GetClientSecret();
        }

        protected override string GetClientSecret()
        {
            string? client_secret = Platform.Config?.Tiktok?.ClientSecret;

            if (client_secret == null)
            {
                throw new InvalidOperationException("Client secret was null");
            }

            return client_secret;
        }

        protected override string GetClientKey()
        {
            return "";
        }

        private byte[] RandomBytes()
        {
            byte[] bytes = new byte[30];

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return bytes;
        }

        private string CodeChallenge()
        {
            string codeVerifier = RandomString();

            byte[] verifierBytes = Encoding.UTF8.GetBytes(codeVerifier);

            //Compute SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] hashBytes = sha256.ComputeHash(verifierBytes);

                //Convert to hex
                string codeChallenge = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                Console.WriteLine(BitConverter.ToString(hashBytes));

                return codeChallenge;
            }

        }

        private string RandomString()
        {
            string result = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            int charLength = chars.Length;
            Random rnd = new Random();

            for (int i = 0; i < charLength; i++)
            {
                result += chars[rnd.Next(0, charLength - 1)];
            }

            return result;
        }

    }

}
