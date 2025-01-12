using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoVid.Model
{
    internal abstract class Platform
    {
        public string clientSecret => GetClientSecret();
        public string clientKey => GetClientKey();

        public abstract void Authenticate();

        protected abstract string GetClientSecret();

        protected abstract string GetClientKey();
    }
}
