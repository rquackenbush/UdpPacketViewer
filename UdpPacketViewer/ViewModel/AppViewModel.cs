using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;
using UdpPacketViewer.Model;
using Xceed.Wpf.DataGrid.ValidationRules;

namespace UdpPacketViewer.ViewModel
{
    
    public class AppViewModel : ViewModelBase
    {
        private Task _listenerTask;
        private readonly ObservableCollection<PacketRowViewModel> _packets = new ObservableCollection<PacketRowViewModel>();
        private readonly ICollectionView _packetsViewSource;
        private StreamWriter _logWriter;
        private UdpListener _listener;
        private PacketViewerPluginManager _pluginManager = new PacketViewerPluginManager();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AppViewModel()
        {
            this.StartCommand = new RelayCommand(Start, CanStart);
            this.StopCommand = new RelayCommand(Stop, CanStop);
            this.ClearCommand = new RelayCommand(Clear, CanClear);

            //if (this.IsInDesignMode)
            //{
                
            //}

            _packetsViewSource = CollectionViewSource.GetDefaultView(_packets);

            _packetsViewSource.Filter = Filter;
            

            this.UdpListenPort = Properties.Settings.Default.UdpListenPort;

            this.Title = "UDP Packet Viewer";

            this.SelectedPacketViewer = this.PacketViewers.FirstOrDefault();
        }

        private bool Filter(object o)
        {
            var row = o as PacketRowViewModel;

            if (row == null)
                return true;

            //Content
            if (!string.IsNullOrWhiteSpace(this.ContentFilter) && CultureInfo.CurrentCulture.CompareInfo.IndexOf(row.Content, this.ContentFilter, CompareOptions.IgnoreCase) < 0 )
                return false;

            //Source
            if (!string.IsNullOrWhiteSpace(this.SourceFilter) && CultureInfo.CurrentCulture.CompareInfo.IndexOf(row.Source, this.SourceFilter, CompareOptions.IgnoreCase) < 0)
                return false;

            //Negative Content
            if (!string.IsNullOrWhiteSpace(this.ContentNegativeFilter) && CultureInfo.CurrentCulture.CompareInfo.IndexOf(row.Content, this.ContentNegativeFilter, CompareOptions.IgnoreCase) >= 0)
                return false;

            //Negative source
            if (!string.IsNullOrWhiteSpace(this.SourceNegativeFilter) && CultureInfo.CurrentCulture.CompareInfo.IndexOf(row.Source, this.SourceNegativeFilter, CompareOptions.IgnoreCase) >= 0)
                return false;

            return true;
        }

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        void PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var packet = new Packet(DateTime.Now, e.Address, e.Port, e.Contents);

            var packetViewModel = new PacketRowViewModel(packet);

            DispatcherHelper.CheckBeginInvokeOnUI(() => _packets.Add(packetViewModel));

            if (_logWriter != null)
            {
                _logWriter.WriteLine("{0},\"{1}\",{2},\"{3}\"", packetViewModel.Timestamp, packetViewModel.Source, packetViewModel.Content.Length, packetViewModel.Content.Replace("\"", "\"\""));
            }
        }

        private PacketRowViewModel _selectedPacket;
        public PacketRowViewModel SelectedPacket
        {
            get { return _selectedPacket; }
            set
            {
                _selectedPacket = value;
                RaisePropertyChanged(() => SelectedPacket);
                UpdatePacketViewer();
            }
        }

        private void UpdatePacketViewer()
        {
            if (this.SelectedPacketViewer == null)
                return;

            this.SelectedPacketViewer.Viewer.SetPacket(this.SelectedPacket == null 
                ? null 
                : this.SelectedPacket.Packet);
        }

        private PacketViewer _selectedPacketViewer;
        public PacketViewer SelectedPacketViewer 
        {
            get { return _selectedPacketViewer; }
            set
            {
                _selectedPacketViewer = value;
                RaisePropertyChanged(() => SelectedPacketViewer);

                UpdatePacketViewer();
            }
        }

        public PacketViewer[] PacketViewers
        {
            get
            {
                return _pluginManager.PacketViewers;
            }
        }

        private void Clear()
        {
            _packets.Clear();
        }

        private bool CanClear()
        {
            return true;
        }

        private void Start()
        {
            var settings = Properties.Settings.Default;

            settings.UdpListenPort = this.UdpListenPort;
            settings.Save();

            if (this.LogToFile)
            {
                var dialog = new SaveFileDialog()
                {
                    DefaultExt = ".csv",
                    AddExtension = true,
                    OverwritePrompt = false
                };

                if (dialog.ShowDialog() != true)
                    return;

                //Create the log writer
                _logWriter = new StreamWriter(File.Open(dialog.FileName, FileMode.Append));
            }

            _listenerTask = new Task(() =>
            {
                using (_listener = new UdpListener(this.UdpListenPort))
                {

                    //Attach the handler
                    _listener.PacketReceived += PacketReceived;

                    try
                    {
                        //Listen!
                        _listener.Listen();
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        //This is how we kill the task... The cancelllation token doesn't work well because another packet 
                        // has to come in for the task to die.
                        Console.WriteLine("Swallowed SocketException");
                    }

                    //Clean up the handler
                    _listener.PacketReceived -= PacketReceived;
                }

                //Ditch the listener
                _listener = null;
                _listenerTask = null;

                if (_logWriter != null)
                {
                    _logWriter.Dispose();
                    _logWriter = null;
                }

                RaisePropertyChanged(() => CanPlay);

            });

            RaisePropertyChanged(() => CanPlay);

            _listenerTask.Start();
        }

        private bool CanStart()
        {
            return _listenerTask == null;
        }

        private void Stop()
        {
            if (_listener != null)
            {
                _listener.Dispose();
            }
        }

        private bool CanStop()
        {
            return _listenerTask != null;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public bool CanPlay
        {
            get { return _listenerTask == null; }
        }

        private int _udpListenPort;
        public int UdpListenPort
        {
            get { return _udpListenPort; }
            set
            {
                _udpListenPort = value;
                RaisePropertyChanged(() => UdpListenPort);
            }
        }

        private bool _logToFile;
        public bool LogToFile
        {
            get { return _logToFile; }
            set
            {
                _logToFile = value;
                RaisePropertyChanged(() => LogToFile);
            }
        }

        private bool _isAutoScroll = true;
        public bool IsAutoScroll
        {
            get { return _isAutoScroll; }
            set
            {
                _isAutoScroll = value;
                RaisePropertyChanged(() => IsAutoScroll);
            }
        }

        public ICollectionView Packets
        {
            get { return _packetsViewSource; }
        }

        private string _contentFilter;
        public string ContentFilter
        {
            get { return _contentFilter; }
            set
            {
                _contentFilter = value;
                RaisePropertyChanged(() => ContentFilter);
                UpdateFilter();
            }
        }

        private string _contentNegativeFilter;
        public string ContentNegativeFilter
        {
            get { return _contentNegativeFilter; }
            set
            {
                _contentNegativeFilter = value;
                RaisePropertyChanged(() => ContentNegativeFilter);
                UpdateFilter();
            }
        }

        private string _sourceFilter;
        public string SourceFilter
        {
            get { return _sourceFilter; }
            set
            {
                _sourceFilter = value;
                RaisePropertyChanged(() => SourceFilter);
                UpdateFilter();
            }
        }

        private string _sourceNegativeFilter;
        public string SourceNegativeFilter
        {
            get { return _sourceNegativeFilter; }
            set
            {
                _sourceNegativeFilter = value;
                RaisePropertyChanged(() => SourceNegativeFilter);
                UpdateFilter();
            }
        }

        private void UpdateFilter()
        {
            _packetsViewSource.Refresh();
        }
    }

    
}