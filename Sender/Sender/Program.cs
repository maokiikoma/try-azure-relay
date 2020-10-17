using System;
using System.Threading.Tasks;

namespace Sender
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("start");

            var sender = new SenderLib.Sender();
            await sender.RunAsync("hoge");

            Console.WriteLine("end.");
        }
    }
}
