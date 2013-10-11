using System;
using System.Net;

namespace UdpPacketViewer.Model
{
    internal class PacketReceivedEventArgs : EventArgs
    {
        public PacketReceivedEventArgs(IPAddress address, int sourcePort, byte[] contents)
        {
            this.Address = address;
            this.Contents = contents;
            this.Port = sourcePort;
        }

        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        public byte[] Contents { get; private set; }
    }
}
