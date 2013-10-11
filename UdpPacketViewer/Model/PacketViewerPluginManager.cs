using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdpPacketViewer.Plugin.BinaryViewer;
using UdpPacketViewer.Plugin.Interfaces;
using UpdPacketViewer.Plugin.AsciiViewer;

namespace UdpPacketViewer.Model
{
    /// <summary>
    /// I hate things called manager.  But this isn't a factory.  Is it a repository?  Who can say.
    /// </summary>
    public class PacketViewerPluginManager
    {
        private PacketViewer[] _packetViewers; 

        public PacketViewerPluginManager()
        {
            var factories = new IUdpPacketViewerFactory[]
            {
                new AsciiPacketViewerFactory(),
                new BinaryPacketViewerFactory(), 
            };

            _packetViewers = factories.Select(f => new PacketViewer(f)).OrderBy(pv => pv.Name).ToArray();

        }

        public PacketViewer[] PacketViewers
        {
            get { return _packetViewers; }
        }
    }

    public class PacketViewer
    {
        private readonly IUdpPacketViewerFactory _factory;
        private readonly Lazy<IUdpPacketViewer> _viewer; 

        public PacketViewer(IUdpPacketViewerFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            _factory = factory;

            _viewer = new Lazy<IUdpPacketViewer>(factory.Create);
        }

        public string Name
        {
            get { return _factory.Name; }
        }

        public string Description
        {
            get { return _factory.Description; }
        }

        public IUdpPacketViewer Viewer
        {
            get { return _viewer.Value; }
        }
    }
}
