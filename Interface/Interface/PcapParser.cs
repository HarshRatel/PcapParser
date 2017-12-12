using System;
using System.Collections.Generic;
using System.IO;
using Interface;
using LightInject;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PcapParser
{
	public class PacketManipulation
	{
        LightInject.ServiceContainer container = new LightInject.ServiceContainer();

		public List<List<string>> pcapTable { get; set; }
	    public Logger logger = new Logger();

	    public PacketManipulation()
	    {
	        pcapTable = new List<List<string>>();

	        container.Register<IParser, TCPParser>("TCPParser");
            container.Register<IParser, UDPParser>("UDPParser");
            container.Register<IParser, ICMPParser>("ICMPParser");
	    }

		public void Parse(string devicePath)   
		{
			pcapTable.Clear();

			if (!File.Exists(devicePath))
			{
				logger.CommonLog("[" + DateTime.Now.ToLongDateString() + "] | Wrong device path used : \"" + devicePath + "\".");
				throw new ArgumentException("Wrong device path");
			}

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
            string[] parserList = { "TCPParser", "UDPParser", "ICMPParser" };
            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

			if (packet is PacketDotNet.NullPacket)
				logger.CommonLog("[" + DateTime.Now.ToShortTimeString() + "] | Error in parsing " + packet.ToString() + "packet.");

		    foreach (var parserName in parserList)
		    {
		        var parser = container.GetInstance<IParser>(parserName);
		        var node = parser.ParsePacket(packet, e);

		        if (node.Count != 0)
		        {
		            pcapTable.Add(node);
                    return;
		        }
		    }
		}
	}
}
