using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HandaKote.ViewModels
{
    public class BluetoothAdvertisement
    {
        /// <summary>
        /// Advertisement data
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get; private set; }

        /// <summary>
        /// Address of the BLE device which sends this advertisement.
        /// </summary>
        public ulong Address { get; private set; }

        /// <summary>
        /// RSSI value which indicates the signal strength from the BLE device.
        /// </summary>
        public int Rssi { get; private set; }

        public float Temperature { get; private set; }

        public BluetoothAdvertisement(BluetoothLEAdvertisement advertisement, ulong address, int rssi)
        {
            this.Advertisement = advertisement;
            this.Address = address;
            this.Rssi = rssi;
            var manufacturerData = this.Advertisement.GetManufacturerDataByCompanyId(0x00fe).SingleOrDefault();
            if (manufacturerData != null)
            {
                var buffer = new byte[4];
                manufacturerData.Data.CopyTo(buffer);
                this.Temperature = BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                this.Temperature = float.NaN;
            }
        }
    }
}
