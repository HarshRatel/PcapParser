using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PcapParser
{
	public class PacketManipulation
	{
		public List<List<string>> pcapTable { get; set; }

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

			var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
			if (packet is PacketDotNet.EthernetPacket)
			{
				var eth = ((PacketDotNet.EthernetPacket)packet);
				Console.WriteLine("Eth: " + eth.ToString());

				var ip = (PacketDotNet.IpPacket)packet.Extract(typeof(PacketDotNet.IpPacket));
				if (ip != null)
				{
					Console.WriteLine("IP: " + ip.ToString());

					ip.SourceAddress = System.Net.IPAddress.Parse("1.2.3.4");
					ip.DestinationAddress = System.Net.IPAddress.Parse("44.33.22.11");
					ip.TimeToLive = 11;

					var tcp = (PacketDotNet.TcpPacket)packet.Extract(typeof(PacketDotNet.TcpPacket));
					if (tcp != null)
					{
						row.Add("TCP");
						row.Add(eth.SourceHwAddress.ToString());
						row.Add(eth.DestinationHwAddress.ToString());
						row.Add(tcp.SourcePort.ToString());
						row.Add(tcp.DestinationPort.ToString());
                        
						pcapTable.Add(row);
					}

					var udp = (PacketDotNet.UdpPacket)packet.Extract(typeof(PacketDotNet.UdpPacket));
					if (udp != null)
					{
                        row.Add("UDP");
                        row.Add(eth.SourceHwAddress.ToString());
                        row.Add(eth.DestinationHwAddress.ToString());
                        row.Add(udp.SourcePort.ToString());
                        row.Add(udp.DestinationPort.ToString());
                        pcapTable.Add(row);
					}
				}

				Console.WriteLine("");
			}
		}
	}
}
