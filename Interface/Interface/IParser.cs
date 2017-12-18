using System.Collections.Generic;

namespace Interface
{
    public interface IParser
    {
        List<string> ParsePacket(PcapDotNet.Packets.Packet packet);
    }
}