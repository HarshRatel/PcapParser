using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Diagnostics;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PcapParser
{
    /// <summary>
    /// Example showing packet manipulation
    /// </summary>
    class PacketManipulation
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine();
            ICaptureDevice device = new CaptureFileReaderDevice("C:/Users/Landis/Documents/Visual Studio 2013/Projects/PcapParser/PcapParser/test.pcap");

            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            // Open the device for capturing
            device.Open();

            // Start capture 'INFINTE' number of packets
            device.Capture();

            // Close the pcap device
            // (Note: this line will never be called since
            //  we're capturing infinite number of packets
            device.Close();
        }

        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            if (packet is PacketDotNet.EthernetPacket)
            {
                var eth = ((PacketDotNet.EthernetPacket)packet);
                Console.WriteLine("Eth: " + eth.ToString());

                //Manipulate ethernet parameters
                //eth.SourceHwAddress = PhysicalAddress.Parse("00-11-22-33-44-55");
                //eth.DestinationHwAddress = PhysicalAddress.Parse("00-99-88-77-66-55");

                var ip = (PacketDotNet.IpPacket)packet.Extract(typeof(PacketDotNet.IpPacket));
                if (ip != null)
                {
                    Console.WriteLine("IP: " + ip.ToString());

                    //manipulate IP parameters
                    ip.SourceAddress = System.Net.IPAddress.Parse("1.2.3.4");
                    ip.DestinationAddress = System.Net.IPAddress.Parse("44.33.22.11");
                    ip.TimeToLive = 11;

                    var tcp = (PacketDotNet.TcpPacket)packet.Extract(typeof(PacketDotNet.TcpPacket));
                    if (tcp != null)
                    {
                        Console.WriteLine("TCP: " + tcp.ToString());
                    }

                    var udp = (PacketDotNet.UdpPacket)packet.Extract(typeof(PacketDotNet.UdpPacket));
                    if (udp != null)
                    {
                        Console.WriteLine("UDP: " + udp.ToString());
                    }
                }

                Console.WriteLine("");
            }
        }
    }
}