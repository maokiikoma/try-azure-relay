using Microsoft.Azure.Relay;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Listner
{
    public class Program
    {
        // replace {RelayNamespace} with the name of your namespace
        private const string RelayNamespace = "";

        // replace {HybridConnectionName} with the name of your hybrid connection
        private const string ConnectionName = "";

        // replace {SAKKeyName} with the name of your Shared Access Policies key, which is RootManageSharedAccessKey by default
        private const string KeyName = "";

        // replace {SASKey} with the primary key of the namespace you saved earlier
        private const string Key = "";


        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await RunAsync();

            Console.WriteLine("end.");
        }

        private static async Task RunAsync()
        {
            var cts = new CancellationTokenSource();

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            var listener = new HybridConnectionListener(new Uri(string.Format("sb://{0}/{1}", RelayNamespace, ConnectionName)), tokenProvider);

            // Subscribe to the status events.
            listener.Connecting += (o, e) => { Console.WriteLine("Connecting"); };
            listener.Offline += (o, e) => { Console.WriteLine("Offline"); };
            listener.Online += (o, e) => { Console.WriteLine("Online"); };

            // Provide an HTTP request handler
            listener.RequestHandler = (context) =>
            {
                // Do something with context.Request.Url, HttpMethod, Headers, InputStream...
                context.Response.StatusCode = HttpStatusCode.OK;
                context.Response.StatusDescription = "OK, This is pretty neat";

                var bytes = new byte[context.Request.InputStream.Length];
                var aaa = context.Request.InputStream.Read(bytes, 0, bytes.Length);
                var str = Encoding.UTF8.GetString(bytes);

                using (var sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.WriteLine(str);
                    Console.WriteLine(str);
                }

                // The context MUST be closed here
                context.Response.Close();
            };

            // Opening the listener establishes the control channel to
            // the Azure Relay service. The control channel is continuously 
            // maintained, and is reestablished when connectivity is disrupted.
            await listener.OpenAsync();
            Console.WriteLine("Server listening");

            // Start a new thread that will continuously read the console.
            await Console.In.ReadLineAsync();

            // Close the listener after you exit the processing loop.
            await listener.CloseAsync();
        }
    }
}
