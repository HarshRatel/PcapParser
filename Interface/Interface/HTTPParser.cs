using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using SharpPcap;

namespace Interface
{
    class HTTPParser
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

            return row;;
        }
    }
}
