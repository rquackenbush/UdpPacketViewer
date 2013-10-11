using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UdpPacketViewer.Plugin.Interfaces
{
    /// <summary>
    /// Gets the packet viewer
    /// </summary>
    public interface IUdpPacketViewerFactory
    {
        /// <summary>
        /// Gets the name of the packet viewer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the packet viewer
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Creates the packet viewer
        /// </summary>
        /// <returns></returns>
        IUdpPacketViewer Create();
    }
}
