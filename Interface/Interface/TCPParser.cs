using System;
using System.Collections.Generic;
using PacketDotNet;
using SharpPcap;

namespace Interface
{
    class TCPParser : IParser
    {
        public List<string> ParsePacket(Packet packet, CaptureEventArgs e)
        {
            List<string> row = new List<string>();
            DateTime time = e.Packet.Timeval.Date;
            int len = e.Packet.Data.Length;

            if (packet is PacketDotNet.EthernetPacket)
            {
                var eth = ((PacketDotNet.EthernetPacket) packet);

                var ip = (PacketDotNet.IpPacket) packet.Extract(typeof (PacketDotNet.IpPacket));
                if (ip != null)
                {
                    var tcp = (PacketDotNet.TcpPacket) packet.Extract(typeof (PacketDotNet.TcpPacket));
                    if (tcp != null)
                    {
                        row.Add("TCP");
                        row.Add(time.ToString("s.ffff"));
                        row.Add(ip.SourceAddress.ToString());
                        row.Add(ip.DestinationAddress.ToString());
                        row.Add(len.ToString());
                        row.Add(tcp.SourcePort + " -> " + tcp.DestinationPort + "  Seq: " + tcp.SequenceNumber + " Win " +
                                tcp.WindowSize);
                    }
                }
            }

            return row;
        }
    }
}
