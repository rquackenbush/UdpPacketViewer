using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UdpPacketViewer.Plugin.Interfaces
{
    /// <summary>
    /// Represents a packet
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// Gets the timestamp for this packet
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the source address for this packet
        /// </summary>
        IPAddress SourceAddress { get; }

        /// <summary>
        /// Gets the source port for this packet
        /// </summary>
        int SourcePort { get; }

        /// <summary>
        /// Gets the raw data for this packet
        /// </summary>
        byte[] Data { get; }
    }
}
