using Makaretu.Dns;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.ClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mdns = new MulticastService();
            //var domainName = "_appletv.local.appletv._local.appletv.local.appletv.local";
            //var domainName = "RRxPT-58410e704_gnsslocation._itxpt_multicast._udp._local";
            var domainName = "_gnsslocation._itxpt_multicast._tcp.local";
            mdns.NetworkInterfaceDiscovered += (s, e) => mdns.SendQuery(domainName);
            mdns.AnswerReceived += (s, e) =>
            {
                Console.WriteLine($"GET responce from server {e.RemoteEndPoint.Address}:{e.RemoteEndPoint.Port}. Message: {e.Message.Answers.FirstOrDefault()}");
            };
            mdns.Start();
            Console.ReadLine();
        }
    }
}
