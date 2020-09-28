using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.ClientApp
{
    public delegate void AfterMdnsAnswerReceived(List<ResourceRecord> records);

    class Program
    {

        static async Task Main(string[] args)
        {
            var client = new Client();
            Console.ReadLine();
        }


    }

    public class Client
    {
        protected bool is_active = true;
        private MulticastService mdns = null;
        private UdpClient udpClient;
        public bool ForMe;
        public SRVRecord srvRec;
        public AfterMdnsAnswerReceived AfterReceived;
        private string multicastIp = "";
        private Thread receiveThread;
        private Thread advertiseThread;
        private string servicename = "";
        private ServiceDiscovery sd;

        public Client()
        {
            mdns = new MulticastService();
            //var domainName = "_appletv.local.appletv._local.appletv.local.appletv.local";
            //var domainName = "RRxPT-58410e704_gnsslocation._itxpt_multicast._udp._local";
            var domainName = "_itxpt_multicast._tcp";
            this.servicename = "gnsslocation";
            mdns.NetworkInterfaceDiscovered += (s, e) => mdns.SendQuery(domainName);
            /*mdns.QueryReceived += (s, e) =>
            {
                var msg = e.Message;
                if (e.RemoteEndPoint.Port == 2020)
                {

                }
                if (msg.Questions.Any(q => q.Name == domainName))
                {
                    var res = msg.CreateResponse();
                    var addresses = MulticastService.GetIPAddresses()
                        .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                    foreach (var address in addresses)
                    {
                        
                        Message message = e.Message.CreateResponse();
                        SRVRecord item = new SRVRecord();
                        item.Class = (DnsClass)32769;
                        item.Priority = 0;
                        item.Weight = 0;
                        item.Port = 14005;
                        item.Target = Dns.GetHostName();
                        res.Answers.Add(item);

                    }
                    mdns.SendAnswer(res);
                }
            };*/
            mdns.AnswerReceived += (s, e) =>
            {
                //if (e.RemoteEndPoint.Address.ToString() == "192.168.88.64")
                //{
                //    return;
                //}
                //if(e.Message.Na)
                if (e.RemoteEndPoint.Port == 2020)
                {

                }
                Console.WriteLine($"GET response from server {e.RemoteEndPoint.Address}:{e.RemoteEndPoint.Port}. Message: {e.Message.Answers.FirstOrDefault()}");
                List<ResourceRecord> answers = e.Message.Answers;
                this.ParseRecords(answers);

                Func<IPEndPoint, bool> predicate = null;
                foreach (ResourceRecord record in answers)
                {
                    if ((record is TXTRecord) && this.ForMe)
                    {
                        foreach (string str in ((TXTRecord)record).Strings)
                        {
                            char[] separator = new char[] { '=' };
                            string[] strArray = str.Split(separator);
                            if (strArray.Length == 2)
                            {
                                if (strArray[0] == "address")
                                {
                                    string text1 = strArray[1];
                                }
                                if (strArray[0] == "multicast")
                                {
                                    this.multicastIp = strArray[1];
                                }
                            }
                        }
                    }
                }
                answers = e.Message.AdditionalRecords;
                foreach (ResourceRecord record in answers)
                {
                    if ((record is TXTRecord) && this.ForMe)
                    {
                        foreach (string str in ((TXTRecord)record).Strings)
                        {
                            char[] separator = new char[] { '=' };
                            string[] strArray = str.Split(separator);
                            if (strArray.Length == 2)
                            {
                                if (strArray[0] == "address")
                                {
                                    string text1 = strArray[1];
                                }
                                if (strArray[0] == "multicast")
                                {
                                    this.multicastIp = strArray[1];
                                }
                            }
                        }
                    }
                }

                if ((this.srvRec != null) && (this.ForMe && ((this.srvRec.Port != 0) && (!string.IsNullOrEmpty(this.multicastIp) && (this.udpClient == null)))))
                {
                    if (predicate == null)
                    {
                        predicate = p => p.Port == this.srvRec.Port;
                    }
                    if (Enumerable.Any<IPEndPoint>(IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners(), predicate))
                    {
                        Console.WriteLine(string.Format("{0}:{1} already in use. Try again later", this.multicastIp, this.srvRec.Port));
                    }
                    else
                    {
                        Console.WriteLine("FeedbackType.Client: " + string.Format("JoinMulticastGroup {0}:{1}", this.multicastIp, this.srvRec.Port));
                        this.udpClient = new UdpClient(this.srvRec.Port);
                        this.udpClient.JoinMulticastGroup(IPAddress.Parse(this.multicastIp), 50);
                        this.receiveThread = new Thread(new ThreadStart(this.Receive));
                        this.receiveThread.Start();
                    }
                }
            };

            this.advertiseThread = new Thread(new ThreadStart(() =>
            {
                sd = new ServiceDiscovery(mdns);
                var sp = new CustomServiceProfile("gnsslocation", domainName, 14005);
                var sp1 = new CustomServiceProfile("gnsslocation1", domainName, 14005);
                sp.AddProperty("host", "192.168.88.239");
                sp.AddProperty("port", "14005");
                //sp.Resources.Add(new ARecord { Name = "_gnssslocation._itxpt_multicast._tcp", Address = IPAddress.Parse("192.168.88.239"), Class = DnsClass.IN });
                //sp.Resources.Add(new SRVRecord { Name = "_itxpt_multicast._tcp", Port = 14005, Priority = 0, Weight = 0, Class = DnsClass.IN });
                sd.Advertise(sp);
                sd.Advertise(sp1);
            }));
            this.advertiseThread.Start();

            mdns.Start();
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        protected void SendMessage(string message, string multicastIp, int port)
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

        private void ParseRecords(List<ResourceRecord> records)
        {
            foreach (ResourceRecord record in records)
            {
                if (record is SRVRecord)
                {
                    if (!((SRVRecord)record).Name.ToString().Contains("_" + this.tag))
                    {
                        continue;
                    }
                    this.srvRec = (SRVRecord)record;
                    this.ForMe = true;
                }
            }
        }

        public void Receive()
        {
            while (this.is_active)
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                if (this.udpClient.Available == 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
                    continue;
                }
                byte[] bytes = this.udpClient.Receive(ref remoteEP);
                string xml = Encoding.Default.GetString(bytes);
                Console.WriteLine("FeedbackType.GNSS: " + xml);
                SendMessage(xml, "192.168.88.239", this.srvRec.Port);
            }
        }

        public void Cancel()
        {
            this.is_active = false;
            this.mdns.Stop();
            Console.WriteLine("FeedbackType.Client client stopped");
            this.udpClient = null;
        }

        public string tag
        {
            get
            {
                return this.servicename;
            }
        }
    }

    public class CustomServiceProfile : ServiceProfile
    {
        public CustomServiceProfile(DomainName instanceName, DomainName serviceName, ushort port, IEnumerable<IPAddress> addresses = null) : base()
        {
            InstanceName = instanceName;
            ServiceName = serviceName;
            var fqn = instanceName;

            var simpleServiceName = new DomainName(ServiceName.ToString()
                .Replace("._tcp", "")
                .Replace("._udp", "")
                .Trim('_')
                .Replace("_", "-"));
            HostName = serviceName;
            Resources.Add(new SRVRecord
            {
                Name = fqn,
                Port = port,
                Class = DnsClass.IN
            });
            foreach (var address in addresses ?? MulticastService.GetLinkLocalAddresses())
            {
                Resources.Add(AddressRecord.Create(HostName, address));
            }
        }
    }
}
