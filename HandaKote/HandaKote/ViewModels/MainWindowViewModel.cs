using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HandaKote.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        private BluetoothScanner _scanner = null;

        public MainWindowViewModel()
        {
            _scanner = new BluetoothScanner();
            _scanner.Received += async (sender, e) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.Rssi = ((HandaEventArgs)e).BluetoothAdvertisement.Rssi;
                });
            };
        }

        ~MainWindowViewModel()
        {
            _scanner.Stop();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        internal void Init()
        {
            _scanner.Init();
            _scanner.Start();
        }

        private int _rssi = 0;

        public int Rssi
        {
            get
            {
                return _rssi;
            }

            set
            {
                _rssi = value;
                NotifyPropertyChanged();
            }
        }
    }
}
