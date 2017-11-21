using System;
using System.Collections.Generic;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PcapParser
{
	public class PacketManipulation
	{
		public List<List<string>> pcapTable { get; set; }
	    public Logger logger = new Logger();
		public PacketManipulation() { pcapTable = new List<List<string>>();}

		public void Parse(string devicePath)
		{
			ICaptureDevice device = new CaptureFileReaderDevice(devicePath);
			
			device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

			device.Open();

			device.Capture();

			device.Close();
		}

		private void device_OnPacketArrival(object sender, CaptureEventArgs e)
		{
			List<string> row = new List<string>();
            DateTime time = e.Packet.Timeval.Date;
            int len = e.Packet.Data.Length;
            
			var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
			if (packet is PacketDotNet.EthernetPacket)
			{
				var eth = ((PacketDotNet.EthernetPacket)packet);

				var ip = (PacketDotNet.IpPacket)packet.Extract(typeof(PacketDotNet.IpPacket));
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
			            row.Add(tcp.SourcePort + " -> " + tcp.DestinationPort + "  Seq: " + tcp.SequenceNumber + " Win " + tcp.WindowSize);

			            pcapTable.Add(row);
			        }

			        var udp = (PacketDotNet.UdpPacket) packet.Extract(typeof (PacketDotNet.UdpPacket));
			        if (udp != null)
			        {
			            row.Add("UDP");
			            row.Add(time.ToString("s.ffff"));
			            row.Add(ip.SourceAddress.ToString());
			            row.Add(ip.DestinationAddress.ToString());
			            row.Add(len.ToString());
			            row.Add(udp.SourcePort + "->" + udp.DestinationPort);

			            pcapTable.Add(row);
			        }

			        var icmp = (PacketDotNet.ICMPv4Packet) packet.Extract((typeof (PacketDotNet.ICMPv4Packet)));
			        if (icmp != null)
			        {
			            row.Add("ICMP");
			            row.Add(time.ToString("s.ffff"));
			            row.Add(ip.SourceAddress.ToString());
			            row.Add(ip.DestinationAddress.ToString());
			            row.Add(len.ToString());
			            row.Add("id: " + icmp.ID);
			        }

			    }
			    else
			    {
			        logger.CommonLog("failed to read ip packet");
			    }
			}
		}
	}
}
