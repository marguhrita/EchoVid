using EchoVid.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace EchoVid.Model
{
    public abstract class Platform
    {
        public string ClientSecret => GetClientSecret();
        public string ClientKey => GetClientKey();

        private static ApiConfig? _Config;

        //create config instance if it does not already exist, return config if it has already been created
        protected static ApiConfig Config
        {
            get
            {
                if (_Config == null)
                {
                    string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "MyFolder", "myfile.txt");
                    string jsonContent = File.ReadAllText(filePath);

                    _Config = JsonSerializer.Deserialize<ApiConfig>(jsonContent);
                }

                if (_Config == null)
                {
                    throw new InvalidOperationException("The configuration could not be loaded.");
                }
                return _Config;
            }
        }
        

        public abstract void Authenticate();

        protected abstract string GetClientSecret();

        protected abstract string GetClientKey();
    }
}
