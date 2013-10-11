using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UdpPacketViewer.Plugin.Interfaces;

namespace UdpPacketViewer.Plugin.BinaryViewer
{
    internal class BinaryPacketViewer : IUdpPacketViewer
    {
        private readonly BinaryView _control = new BinaryView();

        public UIElement Control
        {
            get { return _control; }
        }

        public void SetPacket(IPacket packet)
        {
            if (packet == null || packet.Data == null)
            {
                _control.BinaryTextBox.Text = "";
            }
            else
            {
                //We need to do the whole binary thing
                _control.BinaryTextBox.Text = HexFormatter.Format(packet.Data);
            }
        }
    }
}
