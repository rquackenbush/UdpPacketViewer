using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdpPacketViewer.Plugin.Interfaces;

namespace UpdPacketViewer.Plugin.AsciiViewer
{
    public class AsciiPacketViewerFactory : IUdpPacketViewerFactory
    {
        public string Name
        {
            get { return "ASCII"; }
        }

        public string Description
        {
            get { return "Display the plain text version of the received data."; }
        }

        public IUdpPacketViewer Create()
        {
            return new AsciiPacketViewer();
        }
    }
}
