using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UdpPacketViewer.Plugin.Interfaces
{
    /// <summary>
    /// Represent a packet viewer.
    /// </summary>
    public interface IUdpPacketViewer
    {
        /// <summary>
        /// Gets the visual for this packet viewer
        /// </summary>
        UIElement Control { get; }

        /// <summary>
        /// Sets the packet for this viewer
        /// </summary>
        /// <param name="packet"></param>
        void SetPacket(IPacket packet);
    }
}
