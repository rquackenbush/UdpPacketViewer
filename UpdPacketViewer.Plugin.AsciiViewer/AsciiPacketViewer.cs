using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UdpPacketViewer.Plugin.Interfaces;

namespace UpdPacketViewer.Plugin.AsciiViewer
{
    internal class AsciiPacketViewer : IUdpPacketViewer
    {
        private readonly AsciiView _control = new AsciiView();

        public UIElement Control
        {
            get { return _control; }
        }

        public void SetPacket(IPacket packet)
        {
            _control.AsciiTextBox.Text = packet == null || packet.Data == null
                ? ""
                : Encoding.ASCII.GetString(packet.Data);
        }
    }
}
