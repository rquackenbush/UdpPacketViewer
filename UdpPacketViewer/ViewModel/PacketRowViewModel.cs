using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using UdpPacketViewer.Plugin.Interfaces;

namespace UdpPacketViewer.ViewModel
{
    public class PacketRowViewModel
    {
        private readonly IPacket _packet;

        public PacketRowViewModel(IPacket packet)
        {
            _packet = packet;
        }

        public IPacket Packet
        {
            get { return _packet; }
        }

        public DateTime Timestamp
        {
            get { return _packet.Timestamp; }
        }

        public string Source
        {
            get { return string.Format("{0}:{1}", _packet.SourceAddress, _packet.SourcePort); }
        }

        public int Length
        {
            get
            {
                return _packet.Data == null 
                    ? 0 
                    : _packet.Data.Length; 
            }
        }

        public string Content
        {
            get
            {
                return _packet.Data == null  
                    ? "" 
                    : Encoding.ASCII.GetString(_packet.Data).Trim();
            }
            
        }
    }
}
