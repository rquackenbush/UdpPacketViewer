using System;

namespace UdpPacketViewer.Model
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public PacketReceivedEventArgs(string address, byte[] contents)
        {
            this.Address = address;
            this.Contents = contents;
        }

        public string Address { get; private set; }

        public byte[] Contents { get; private set; }
    }
}
