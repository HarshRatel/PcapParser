using System;
using System.Collections.Generic;
using System.IO;
using Interface;
using LightInject;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace PcapParser
{
	public class PacketManipulation
	{
        LightInject.ServiceContainer container = new LightInject.ServiceContainer();

		public List<List<string>> pcapTable { get; set; }
	    public ILogger logger;

	    public PacketManipulation()
	    {
	        pcapTable = new List<List<string>>();

            container.Register<IParser, HTTPParser>("HTTPParser");
	        container.Register<IParser, TCPParser>("TCPParser");
	        container.Register<ILogger, Logger>("Logger");
            container.Register<IParser, UDPParser>("UDPParser");
            container.Register<IParser, ICMPParser>("ICMPParser");
	        logger = container.GetInstance<ILogger>("Logger");
	    }

		public void Parse(string devicePath)   
		{
			pcapTable.Clear();

			if (!File.Exists(devicePath))
			{
				logger.CommonLog("[" + DateTime.Now.ToLongDateString() + "] | Wrong device path used : \"" + devicePath + "\".");
				throw new ArgumentException("Wrong device path");
			}

            OfflinePacketDevice selectedDevice = new OfflinePacketDevice(devicePath);

            using (PacketCommunicator communicator =
                selectedDevice.Open(65536, 
                                    PacketDeviceOpenAttributes.Promiscuous, 
                                    1000))                                  
            {
                communicator.ReceivePackets(0, DispatcherHandler);
            }
		}

	    private void DispatcherHandler(Packet packet)
	    {
            List<string> row = new List<string>();

            string[] parserList = { "HTTPParser", "TCPParser", "UDPParser", "ICMPParser" };

            if (!packet.IsValid)
                logger.CommonLog("[" + DateTime.Now.ToShortTimeString() + "] | Error in parsing " + packet.ToString() + " time " + packet.Timestamp.ToString("s.ffff") + " packet.");

            foreach (var parserName in parserList)
            {
                var parser = container.GetInstance<IParser>(parserName);
                var node = parser.ParsePacket(packet);

                if (node.Count != 0)
                {
                    pcapTable.Add(node);
                    return;
                }
            }
	    }
	}
}
