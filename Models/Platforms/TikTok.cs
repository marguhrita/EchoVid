using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Formats.Asn1.AsnWriter;


namespace EchoVid.Model.Platforms
{
    internal class TikTok
    {

        private static readonly HttpClient client = new HttpClient();
        public string code;

        public async Task<Boolean> Authenticate()
        {
            string code_verifier = RandomString();
            string code_challenge = CodeChallenge();


            string api_key = Constants.TIKTOK_API_KEY;
            string scope = "user.info.basic,video.upload";
            string redirect_uri = Constants.TIKTOK_REDIRECT_URI;
            string state = Guid.NewGuid().ToString("N");
            string response_type = "code";
            string code_challenge_method = "S256";

            var authUrl = $"{Constants.TIKTOK_BASE_URL}authorize/" +
                 $"?client_key={api_key}" +
                 $"&scope={Uri.EscapeDataString(scope)}" +
                 $"&redirect_uri={Uri.EscapeDataString(redirect_uri)}" +
                 $"&state={state}" +
                 $"&response_type={response_type}" +
                 $"&code_challenge={code_challenge}" +
                 $"&code_challenge_method={code_challenge_method}";

            try
            {
                Debug.WriteLine("Authenticating");
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri(authUrl),
                    new Uri("myapp://localhost:3455/callback/"));

                code = authResult?.AccessToken;
                
                return true;
            }
            catch (TaskCanceledException e)
            {
                // Use stopped auth
            }
            return false;
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




