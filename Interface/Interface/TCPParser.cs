using System.Collections.Generic;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Interface
{
    class TCPParser : IParser
    {
        public List<string> ParsePacket(PcapDotNet.Packets.Packet packet)
        {
            List<string> row = new List<string>();
            IpV4Datagram ip = packet.Ethernet.IpV4;
            TcpDatagram tcp = ip.Tcp;

            if (tcp == null)
                return row;

            if (tcp.IsValid)
            {
                    row.Add("TCP");
                    row.Add(packet.Timestamp.ToString("s.ffff"));
                    row.Add(ip.Source.ToString());
                    row.Add(ip.Destination.ToString());
                    row.Add(packet.Length.ToString());
                    row.Add(tcp.SourcePort + " -> " + tcp.DestinationPort + " Seq: " + tcp.SequenceNumber + " Win " + tcp.Window);
            }

            return row;
        }
    }
}
