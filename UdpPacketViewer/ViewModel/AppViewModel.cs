using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;
using UdpPacketViewer.Model;

namespace UdpPacketViewer.ViewModel
{
    
    public class AppViewModel : ViewModelBase
    {
        private Task _listenerTask;
        private readonly ObservableCollection<PacketViewModel> _packets = new ObservableCollection<PacketViewModel>();
        private StreamWriter _logWriter;

        private UdpListener _listener;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AppViewModel()
        {
            this.StartCommand = new RelayCommand(Start, CanStart);
            this.StopCommand = new RelayCommand(Stop, CanStop);

            if (this.IsInDesignMode)
            {
                this.Packets.Add(new PacketViewModel(DateTime.Now.Subtract(TimeSpan.FromMinutes(5)), "192.168.1.1:800", 6, "Hello!"));
            }

            this.UdpListenPort = Properties.Settings.Default.UdpListenPort;

            this.Title = "UDP Packet Viewer";
        }

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        void PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var contents =  Encoding.ASCII.GetString(e.Contents);

            var packetViewModel = new PacketViewModel(DateTime.Now, e.Address, e.Contents.Length, contents);

            DispatcherHelper.CheckBeginInvokeOnUI(() => this.Packets.Add(packetViewModel));

            if (_logWriter != null)
            {
                _logWriter.WriteLine("{0},\"{1}\",{2},\"{3}\"", packetViewModel.Timestamp, packetViewModel.Source, packetViewModel.Content.Length, packetViewModel.Content.Replace("\"", "\"\""));
            }
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

        public ObservableCollection<PacketViewModel> Packets
        {
            get { return _packets; }
        }
    }

    
}