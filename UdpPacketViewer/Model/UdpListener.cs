using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpPacketViewer.Model
{
    internal class UdpListener : IDisposable
    {
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        private readonly UdpClient _client;

        public UdpListener(int port)
        {
            _client = new UdpClient(port);
        }

        private void RaisePacketReceived(IPAddress address, int port, byte[] contents)
        {
            var handler = PacketReceived;

            if (handler == null)
                return;

            handler(this, new PacketReceivedEventArgs(address, port, contents));
        }

        public void Listen()
        {
            while (true)
            {
                IPEndPoint groupEndPoint = null;

                var buffer = _client.Receive(ref groupEndPoint);

                RaisePacketReceived(groupEndPoint.Address, groupEndPoint.Port, buffer);               
            }
        }

        public void Dispose()
        {
            ((IDisposable)_client).Dispose();
        }
    }
}
