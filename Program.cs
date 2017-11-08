using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace System.IO
{
    public class Program
    {
        public string writePath = @"../dir/msg.txt";
        
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

