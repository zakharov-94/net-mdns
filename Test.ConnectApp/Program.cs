using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Test.ConnectApp
{


    class Program
    {
        public static Thread advertiseThread;
        public static ServiceDiscovery sd;

        static void Main(string[] args)
        {
            Console.WriteLine("Started!");
            RunServer();
            SendMessage("test hello", "192.168.88.239", 14005);
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
            advertiseThread = new Thread(new ThreadStart(() =>
            {
                sd = new ServiceDiscovery(mdns);
                var sp = new ServiceProfile("_itxpt_multicast._tcp.", "_itxpt._tcp", 5353, new List<IPAddress> { IPAddress.Parse("192.168.88.239") });
                //sp.AddProperty("host", "192.168.88.239");
                //sp.AddProperty("port", "14005");
                sp.Resources.Add(new ARecord { Name = "_itxpt_multicast._tcp.", Address = IPAddress.Parse("192.168.88.239"), Class = DnsClass.ANY });
                sp.Resources.Add(new SRVRecord { Name = "_itxpt_multicast._tcp.", Port = 5353, Priority = 0, Weight = 0, Class = DnsClass.ANY });
                sp.Resources.Add(new PTRRecord { Name = "_itxpt_multicast._tcp.", DomainName = "192.168.88.239" });
                sd.Advertise(sp);
            }));
            advertiseThread.Start();

            mdns.Start();
        }

        protected static void SendMessage(string message, string multicastIp, int port)
        {
            byte[] bytes = Encoding.Default.GetBytes(message);
            using (UdpClient client = new UdpClient(AddressFamily.InterNetwork))
            {
                IPAddress address = IPAddress.Parse(multicastIp);
                IPEndPoint endPoint = new IPEndPoint(address, port);
                client.Send(bytes, bytes.Length, endPoint);
                client.Close();
            }
        }

    }



}
