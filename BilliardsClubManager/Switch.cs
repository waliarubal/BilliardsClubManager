using FTD2XX_NET;

namespace BilliardsClubManager
{
    class Switch
    {
        readonly FTDI _device;
        byte[] _sentBytes;
        uint _receivedBytes;
        readonly byte[][] _data;

        #region constructor

        public Switch()
        {
            _device = new FTDI();
            _sentBytes = new byte[2];
            _data = new byte[][]
            {
                new byte[]{ 2, 253 },
                new byte[]{ 8, 247 },
                new byte[]{ 32, 223 },
                new byte[]{ 128, 127 },
            };
        }

        #endregion

        #region properties

        public bool IsOpen
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Toggles all the switches,
        /// </summary>
        /// <param name="isOn"></param>
        /// <returns></returns>
        public bool Toggle(bool isOn)
        {
            var result = true;
            for (var index = 0; index < _data.Length; index++)
                result &= Toggle(index, isOn);

            return result;
        }

        /// <summary>
        /// Toggle switch with a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        /// <returns></returns>
        public bool Toggle(int index, bool isOn)
        {
            if (!IsOpen || index < 0 || index > 3)
                return false;

            if (isOn)
                _sentBytes[0] = (byte)(_sentBytes[0] | _data[index][0]);
            else
                _sentBytes[0] = (byte)(_sentBytes[0] & _data[index][1]);

            return _device.Write(_sentBytes, 1, ref _receivedBytes) == FTDI.FT_STATUS.FT_OK;
        }

        public string Open()
        {
            if (IsOpen)
                return "Connection to the device is already open.";

            IsOpen = false;

            var status = _device.OpenByIndex(0);
            if (status != FTDI.FT_STATUS.FT_OK)
                return "Failed to open device.";

            status = _device.SetBaudRate(921600);
            if (status != FTDI.FT_STATUS.FT_OK)
                return "Failed to set baud rate to 921600.";

            status = _device.SetBitMode(255, 4);
            if (status != FTDI.FT_STATUS.FT_OK)
                return "Failed to set mask to 255 and bitmode to 4.";

            _sentBytes[0] = 0;
            IsOpen = true;
            return null;
        }

        public string Close()
        {
            if (!IsOpen)
                return "Connection to the device is not open.";

            if (_device.Close() != FTDI.FT_STATUS.FT_OK)
                return "Faild to close device.";

            IsOpen = false;
            return null;
        }
    }
}
