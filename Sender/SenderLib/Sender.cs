using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Azure.Relay;
using System.Diagnostics;

namespace SenderLib
{
    public class Sender
    {
        private const string RelayNamespace = "";
        private const string ConnectionName = "";
        private const string KeyName = "";
        private const string Key = "";

        private static HttpClient _httpClient = new HttpClient();


        public async Task RunAsync(string str)
        {
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            var uri = new Uri(string.Format("https://{0}/{1}", RelayNamespace, ConnectionName));
            var token = (await tokenProvider.GetTokenAsync(uri.AbsoluteUri, TimeSpan.FromHours(1))).TokenString;
            var content = new StringContent(str);
            content.Headers.Add("ServiceBusAuthorization", token);

            var response = await _httpClient.PostAsync(uri, content);
            var body = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(body);
        }
    }
}
