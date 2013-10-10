using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpPacketViewer.Model
{
    public class UdpListener : IDisposable
    {
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        private readonly UdpClient _listener;
        private IPEndPoint _groupEndPoint;

        public UdpListener(int port)
        {
            _listener = new UdpClient(port);

            _groupEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        private void RaisePacketReceived(string address, byte[] contents)
        {
            var handler = PacketReceived;

            if (handler == null)
                return;

            handler(this, new PacketReceivedEventArgs(address, contents));
        }

        public void Listen()
        {
            while (true)
            {
                var buffer = _listener.Receive(ref _groupEndPoint);

                RaisePacketReceived(_groupEndPoint.ToString(), buffer);                
            }
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                ((IDisposable)_listener).Dispose();
            }
        }
    }
}
