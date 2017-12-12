using System;
using System.IO;
using PacketDotNet;

namespace PcapParser
{
    public class Logger
    {
		private string writePath = Path.Combine(Directory.GetCurrentDirectory(), @"../../../logs/logs.txt");

        public void CommonLog(string cmnMsg)
        {
            using (var cmnSw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                cmnSw.WriteLine("[" + DateTime.Now.ToShortTimeString() + "]:" + cmnMsg);

			}
        }

        public void PacketLog(Packet packet)
        {
            using (var pctSw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                pctSw.WriteLine(packet.ToString());
            }
        }
    }
}

