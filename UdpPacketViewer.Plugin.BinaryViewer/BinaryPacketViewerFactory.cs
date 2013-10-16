using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdpPacketViewer.Plugin.Interfaces;

namespace UdpPacketViewer.Plugin.BinaryViewer
{
    public class BinaryPacketViewerFactory : IUdpPacketViewerFactory
    {
        public string Name
        {
            get { return "Binary"; }
        }

        public string Description
        {
            get { return "Description"; }
        }

        public IUdpPacketViewer Create()
        {
            return new BinaryPacketViewer();
        }
    }
}
