﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;
using SharpPcap;

namespace Interface
{
    class UDPParser : IParser
    {
        public List<string> ParsePacket(Packet packet, CaptureEventArgs e)
        {
            List<string> row = new List<string>();
            DateTime time = e.Packet.Timeval.Date;
            int len = e.Packet.Data.Length;

            if (packet is PacketDotNet.EthernetPacket)
            {
                var eth = ((PacketDotNet.EthernetPacket)packet);

                var ip = (PacketDotNet.IpPacket)packet.Extract(typeof(PacketDotNet.IpPacket));
                if (ip != null)
                {
                    var udp = (PacketDotNet.UdpPacket)packet.Extract(typeof(PacketDotNet.UdpPacket));
                    if (udp != null)
                    {
                        row.Add("UDP");
                        row.Add(time.ToString("s.ffff"));
                        row.Add(ip.SourceAddress.ToString());
                        row.Add(ip.DestinationAddress.ToString());
                        row.Add(len.ToString());
                        row.Add(udp.SourcePort + "->" + udp.DestinationPort);
                    }
                }
            }

            return row;
        }
    }
}
