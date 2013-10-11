using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UdpPacketViewer.Model;
using UdpPacketViewer.ViewModel;

namespace UdpPacketViewer.View
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        public AppView()
        {
            InitializeComponent();
        }
        
        private void SetPacketViewer()
        {
            if (PacketViewerContainer == null)
                return;

            var packetViewer = PacketViewerComboBox.SelectedItem as PacketViewer;

            PacketViewerContainer.Children.Clear();

            if (packetViewer != null)
            {
                PacketViewerContainer.Children.Add(packetViewer.Viewer.Control);
            }
        }

        private void PacketViewerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPacketViewer();
        }

        private void AppView_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetPacketViewer();
        }
    }
}
