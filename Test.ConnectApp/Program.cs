using Makaretu.Dns;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Test.ConnectApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started!");
            RunServer();
            Console.ReadLine();
        }

        private static void RunServer()
        {
            var service = "_appletv.local.appletv._local.appletv.local.appletv.local";
            var mdns = new MulticastService();
            mdns.QueryReceived += (s, e) =>
            {
                var msg = e.Message;
                if (msg.Questions.Any(q => q.Name == service))
                {
                    var res = msg.CreateResponse();
                    var addresses = MulticastService.GetIPAddresses()
                        .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                    foreach (var address in addresses)
                    {
                        var ar = new ARecord
                        {
                            Name = service,
                            Address = address
                        };
                        var writer = new PresentationWriter(new StringWriter());
                        writer.WriteString("123qweq");
                        ar.WriteData(writer);
                        ar.Name = "qweqeqweq";
                        res.Answers.Add(ar);
                    }
                    mdns.SendAnswer(res);
                }
            };
            mdns.Start();
        }
    }
}
