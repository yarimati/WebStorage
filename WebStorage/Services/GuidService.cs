using System;
using System.Text.RegularExpressions;
using WebStorage.Services.Interfaces;

namespace WebStorage.Services
{
    public class GuidService : IGuidService
    {
        private readonly Guid serviceGuid;

        public GuidService()
        {
            serviceGuid = Guid.NewGuid();
        }
        public string GenLongUniqueName() => serviceGuid.ToString();
        public string GenShortUniqueName() =>
            Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
    }
}
