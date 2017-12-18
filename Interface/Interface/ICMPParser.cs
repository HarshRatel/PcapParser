using System.Collections.Generic;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;

namespace Interface
{
    class ICMPParser : IParser
    {
        public List<string> ParsePacket(PcapDotNet.Packets.Packet packet)
        {
            List<string> row = new List<string>();
            IpV4Datagram ip = packet.Ethernet.IpV4;
            IcmpDatagram icmp = ip.Icmp;

            if (icmp == null)
                return row;

            if (icmp.IsValid)
            {
                row.Add("TCP");
                row.Add(packet.Timestamp.ToString("s.ffff"));
                row.Add(ip.Source.ToString());
                row.Add(ip.Destination.ToString());
                row.Add(packet.Length.ToString());
                row.Add("id: " + icmp.Variable);
            }

            return row;
        }
    }
}