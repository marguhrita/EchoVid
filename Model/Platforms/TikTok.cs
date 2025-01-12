using System;
using System.Collections.Generic;
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
        public string Authenticated => "Not Logged in!!";

        public async Task<Boolean> Authenticate()
        {

            //Get code

            string code_verifier = RandomString();
            string code_challenge = CodeChallenge(code_verifier);

            //Send request and get auth code
            string auth_response = await RequestOauth();

            var auth_response_data = JsonSerializer.Deserialize<OAuthResponse>(auth_response);

            if (string.IsNullOrEmpty(auth_response_data.code))
            {
                throw new AuthenticationException("Failed to receive authorization code from TikTok");
            }

            //Update code verifier 
            code_verifier = RandomString();

            //send request to get access token for user
            string token_response = await (RequestAccessToken(code_verifier, auth_response_data.code));

            //deserialize response
            var token_response_data = JsonSerializer.Deserialize<TokenResponse>(token_response);

            if (string.IsNullOrEmpty(token_response_data.access_token))
            {
                throw new AuthenticationException("Failed to receive authorization code from TikTok");
            }

            //return true if access token exists!
            return true;

        }

        public async Task<string> RequestAccessToken(string code_verifier, string code)
        {

            string api_key = Constants.TIKTOK_API_KEY;
            string secret = Constants.TIKTOK_SECRET;
            string grant_type = "authorization_code";
            string redirect_uri = Constants.TIKTOK_REDIRECT_URI;

            //Prepare values
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "client_key", api_key },
                {"client_secret", secret },
                {"code", code },
                { "grant_type", grant_type },
                { "redirect_uri", redirect_uri },
                { "code_verifier", code_verifier },
            };

            var content = new FormUrlEncodedContent(values);

            //Send request
            try
            {
                var response = await client.PostAsync($"{Constants.TIKTOK_BASE_URL}token/", content);
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the error appropriately
                throw new Exception($"OAuth request failed: {ex.Message}", ex);
            }

        }

        public async Task<string> RequestOauth()
        {
            string code_verifier = RandomString();
            string code_challenge = CodeChallenge(code_verifier);


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

            return authUrl;
        }


        private byte[] RandomBytes()
        {
            byte[] bytes = new byte[30];

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return bytes;
        }

        private string CodeChallenge(string code_verifier)
        {

            byte[] verifier_bytes = Encoding.UTF8.GetBytes(code_verifier);

            //Compute SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] hash_bytes = sha256.ComputeHash(verifier_bytes);

                //Convert to hex
                string codeChallenge = BitConverter.ToString(hash_bytes).Replace("-", "").ToLower();

                Console.WriteLine(BitConverter.ToString(hash_bytes));

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

        // Helper class to deserialize the OAuth response
        private class OAuthResponse
        {
            [JsonPropertyName("code")]
            public string code { get; set; }

            [JsonPropertyName("scopes")]
            public string scopes { get; set; }

            [JsonPropertyName("state")]
            public string state { get; set; }

            [JsonPropertyName("error")]
            public string error { get; set; }

            [JsonPropertyName("error_description")]
            public string error_description { get; set; }


        }


        public class TokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string open_id { get; set; }
            public int refresh_expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }



    }

}


