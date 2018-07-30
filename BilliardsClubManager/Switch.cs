using FTD2XX_NET;

namespace BilliardsClubManager
{
    class Switch
    {
        static Switch _instance;
        static readonly object _syncLock;
        readonly FTDI _device;
        byte[] _sentBytes;
        uint _receivedBytes;
        readonly byte[][] _data;

        #region constructor

        static Switch()
        {
            _syncLock = new object();
        }

        private Switch()
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

        public static Switch Instance
        {
            get
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new Switch();

                    return _instance;
                }
            }
        }

        #endregion

        public bool Toggle(int index, bool isOn)
        {
            if (index < 0 || index > 3)
                return false;

            if (isOn)
                _sentBytes[0] = (byte)(_sentBytes[0] | _data[index][0]);
            else
                _sentBytes[0] = (byte)(_sentBytes[0] & _data[index][1]);

            return _device.Write(_sentBytes, 1, ref _receivedBytes) == FTDI.FT_STATUS.FT_OK;
        }

        public string Open()
        {
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
            return null;
        }

        public string Close()
        {
            if (_device.Close() != FTDI.FT_STATUS.FT_OK)
                return "Faild to close device.";

            return null;
        }
    }
}
