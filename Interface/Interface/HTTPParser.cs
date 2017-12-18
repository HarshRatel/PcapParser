using System.Collections.Generic;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Interface
{
    class HTTPParser : IParser
    {
        public List<string> ParsePacket(PcapDotNet.Packets.Packet packet)
        {
            List<string> row = new List<string>();
            IpV4Datagram ip = packet.Ethernet.IpV4;
            TcpDatagram tcp = ip.Tcp;

            if (tcp == null)
                return row;

            HttpDatagram http = tcp.Http;

            if (http.IsValid && http.IsRequest)
            {
                if (http.Header != null)
                {
                    row.Add("HTTP");
                    row.Add(packet.Timestamp.ToString("s.ffff"));
                    row.Add(ip.Source.ToString());
                    row.Add(ip.Destination.ToString());
                    row.Add(packet.Length.ToString());
                    row.Add(http.Header.ToString());
                }
            }

            return row; ;
        }
    }
}