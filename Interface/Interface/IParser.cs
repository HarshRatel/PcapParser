using System.Collections.Generic;
using SharpPcap;

namespace Interface
{
    public interface IParser
    {
        List<string> ParsePacket(PacketDotNet.Packet packet, CaptureEventArgs e);
    }
}