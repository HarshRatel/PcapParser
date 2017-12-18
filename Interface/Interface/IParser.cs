using System.Collections.Generic;

namespace Interface
{
    /// <summary>
    /// Interface for realistion LightInjection inversion pf controll
    /// </summary>
    public interface IParser
    {
        List<string> ParsePacket(PcapDotNet.Packets.Packet packet);
    }
}