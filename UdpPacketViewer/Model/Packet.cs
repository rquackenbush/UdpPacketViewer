using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UdpPacketViewer.Plugin.Interfaces;

namespace UdpPacketViewer.Model
{
    public class Packet : IPacket
    {
        public Packet(DateTime timestamp, IPAddress sourceAddress, int sourcePort, byte[] data)
        {
            this.Timestamp = timestamp;
            this.SourceAddress = sourceAddress;
            this.SourcePort = sourcePort;
            this.Data = data;
        }

        public DateTime Timestamp { get; private set; }
        
        public IPAddress SourceAddress{ get; private set; }

        public int SourcePort{ get; private set; }

        public byte[] Data{ get; private set; }
        
    }
}
