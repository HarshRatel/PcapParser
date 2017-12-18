using System;
using System.IO;
using Interface;
using PacketDotNet;

namespace PcapParser
{
    /// <summary>
    /// class for Logging exceptions and packets
    /// </summary>
    public class Logger : ILogger
    {
		private string writePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\\..\\..\\..\\logs\\logs.txt");

        /// <summary>
        /// Write message to log file
        /// </summary>
        /// <param name="cmnMsg">Message</param>
        public void CommonLog(string cmnMsg)
        {
            using (var cmnSw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                cmnSw.WriteLine("[" + DateTime.Now.ToShortTimeString() + "]:" + cmnMsg);
        }

        /// <summary>
        /// Write packet info to log file
        /// </summary>
        /// <param name="packet">packet</param>
        public void PacketLog(Packet packet)
        {
            using (var pctSw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                pctSw.WriteLine(packet.ToString());
            
        }
    }
}

