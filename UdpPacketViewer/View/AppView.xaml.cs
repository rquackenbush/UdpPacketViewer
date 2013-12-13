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
using System.Windows.Threading;
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

        //private void AppView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var oldViewModel = e.OldValue as AppViewModel;

        //    if (oldViewModel != null)
        //    {
        //        oldViewModel.Packets.CollectionChanged -= Packets_CollectionChanged;
        //    }

        //    var newViewModel = e.NewValue as AppViewModel;

        //    if (newViewModel != null)
        //    {
        //        newViewModel.Packetses.CollectionChanged += Packets_CollectionChanged;
        //    }
        //}

        //void Packets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    var viewModel = this.DataContext as AppViewModel;

        //    if (viewModel == null)
        //        return;

        //    if (!viewModel.IsAutoScroll)
        //        return;

        //    var newItems = e.NewItems;

        //    if (e.NewItems != null && e.NewItems.Count > 0)
        //    {
        //        //this.Dispatcher.Invoke(() =>
        //        //{
        //            var item = e.NewItems[0];

        //        try
        //        {
        //            PacketsGrid.ScrollIntoView(item);
        //        }
        //        catch (Exception)
        //        {
                    
                    
        //        }

                    

        //            //var row = PacketsGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

        //            //if (row != null)
        //            //{
        //            //    row.Focus();
        //            //    //PacketsGrid.ScrollIntoView(row);
        //            //}

        //            //PacketsGrid.ScrollIntoView(e.NewItems[0]);
        //        //}, DispatcherPriority.Background);

        //    }
        //}
    }
}
