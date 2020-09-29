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
            Console.WriteLine("Please enter clientIp:");
            string ip = Console.ReadLine();
            ip = string.IsNullOrEmpty(ip) ? "192.168.88.239" : ip;
            var client = new Client(ip);

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
        private string _clientIp = "";

        public Client(string clientIp)
        {
            _clientIp = clientIp;
            mdns = new MulticastService();
            var domainName = "_itxpt_multicast._tcp";
            servicename = "gnsslocation";
            mdns.NetworkInterfaceDiscovered += (s, e) => mdns.SendQuery(domainName);

            mdns.AnswerReceived += (s, e) =>
            {
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
                        udpClient = new UdpClient(this.srvRec.Port);
                        udpClient.JoinMulticastGroup(IPAddress.Parse(this.multicastIp), 50);
                        receiveThread = new Thread(new ThreadStart(this.Receive));
                        receiveThread.Start();
                    }
                }
            };

            advertiseThread = new Thread(new ThreadStart(() =>
            {
                sd = new ServiceDiscovery(mdns);
                var sp = new ServiceProfile(servicename, domainName, 14005);
                sp.AddProperty("host", "192.168.88.239");
                sp.AddProperty("port", "14005");
                sp.Resources.Add(new ARecord { Name = "_itxpt_multicast._tcp", Address = IPAddress.Parse("192.168.88.239"), Class = DnsClass.IN });
                sp.Resources.Add(new SRVRecord { Name = "_itxpt_multicast._tcp", Port = 14005, Priority = 0, Weight = 0, Class = DnsClass.IN });
                sd.Advertise(sp);
            }));

            advertiseThread.Start();

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
                SendMessage(xml, _clientIp, this.srvRec.Port); // "192.168.88.43"
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
}
