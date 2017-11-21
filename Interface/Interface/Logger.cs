using System.IO;
using PacketDotNet;

namespace PcapParser
{
    public class Logger
    {
        public string writePath = Path.Combine(Directory.GetCurrentDirectory(), @"../../../logs/logs.txt");

        public void CommonLog(string cmnMsg)
        {
            using (StreamWriter cmnSw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                cmnSw.WriteLine(cmnMsg);
            }
        }

        public void PacketLog(Packet packet)
        {
            using (StreamWriter pctSw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                pctSw.WriteLine(packet.ToString());
            }
        }
    }
}

