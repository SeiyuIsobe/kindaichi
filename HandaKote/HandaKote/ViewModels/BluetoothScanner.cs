using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace HandaKote.ViewModels
{
    public class BluetoothScanner
    {
        private readonly BluetoothLEAdvertisementWatcher _watcher = null;
        public bool IsStopped { get; private set; } = true;

        public event EventHandler Received;
        public event EventHandler Stopped;

        public BluetoothScanner()
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            #region
            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;
            var writer = new DataWriter();
            writer.WriteUInt16(0x1234);
            _watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
            #endregion
            _watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;
            _watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -75;
            _watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);

        }

        public void Init()
        {
            _watcher.Received += OnAdvertisementReceived;
            _watcher.Stopped += OnAdvertisementWatcherStopped;
        }

        private void OnAdvertisementWatcherStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            if(null != Stopped)
            {
                Stopped(this, null);
            }
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // The timestamp of the event
            DateTimeOffset timestamp = eventArgs.Timestamp;

            // The type of advertisement
            BluetoothLEAdvertisementType advertisementType = eventArgs.AdvertisementType;

            // The received signal strength indicator (RSSI)
            Int16 rssi = eventArgs.RawSignalStrengthInDBm;

            // The local name of the advertising device contained within the payload, if any
            string localName = eventArgs.Advertisement.LocalName;

            // データ
            BluetoothAdvertisement blueAd = new BluetoothAdvertisement(eventArgs.Advertisement, eventArgs.BluetoothAddress, rssi);

            System.Diagnostics.Debug.WriteLine($"-> BluetoothAddress = {eventArgs.BluetoothAddress}, rssi = {rssi}");

            if (null != Received)
            {
                Received(this, new HandaEventArgs { BluetoothAdvertisement = blueAd });
            }
        }

        public void Start()
        {
            _watcher.Start();
            this.IsStopped = false;
        }

        public void Stop()
        {
            _watcher.Stop();
            this.IsStopped = true;
        }
    }

    public class HandaEventArgs : EventArgs
    {
        public BluetoothAdvertisement BluetoothAdvertisement { get; set; }
    }
}
