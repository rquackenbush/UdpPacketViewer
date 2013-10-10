using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace UdpPacketViewer.ViewModel
{
    public class PacketViewModel
    {
        public PacketViewModel(DateTime timestamp, string source, int length, string content)
        {
            this.Timestamp = timestamp;
            this.Source = source;
            this.Length = length;
            this.Content = content;
        }

        public DateTime Timestamp { get; private set; }

        public string Source { get; private set; }

        public int Length { get; private set; }

        public string Content { get; private set; }
    }
}
