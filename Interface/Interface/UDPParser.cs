using System.Collections.Generic;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Interface
{
    class UDPParser : IParser
    {
        /// <summary>
        /// Parse UDP packet
        /// </summary>
        /// <param name="packet">packet for analysing</param>
        /// <returns>information about packet</returns>
        public List<string> ParsePacket(PcapDotNet.Packets.Packet packet)
        {
            List<string> row = new List<string>();
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;

            if (udp == null)
                return row;

            if (udp.IsValid)
            {   
                    row.Add("UDP");
                    row.Add(packet.Timestamp.ToString("s.ffff"));
                    row.Add(ip.Source.ToString());
                    row.Add(ip.Destination.ToString());
                    row.Add(packet.Length.ToString());
                    row.Add(udp.SourcePort + "->" + udp.DestinationPort);
            }

            return row;
        }
    }
}
