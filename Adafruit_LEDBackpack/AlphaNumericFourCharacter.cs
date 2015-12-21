using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Adafruit_LEDBackpack
{
    public class AlphaNumericFourCharacter
    {
        private readonly Int32 _deviceAddress;
        private readonly List<int> _displaybuffer = new List<int>(8);

        public AlphaNumericFourCharacter(Int32 deviceAddress = 0x70)
        {
            _deviceAddress = deviceAddress;
        }

        private readonly int[] _numbertable = new[] {
            0x3F, /* 0 */
	        0x06, /* 1 */
	        0x5B, /* 2 */
	        0x4F, /* 3 */
	        0x66, /* 4 */
	        0x6D, /* 5 */
	        0x7D, /* 6 */
	        0x07, /* 7 */
	        0x7F, /* 8 */
	        0x6F, /* 9 */
	        0x77, /* a */
	        0x7C, /* b */
	        0x39, /* C */
	        0x5E, /* d */
	        0x79, /* E */
	        0x71, /* F */
        };

        public void WriteDigitNumber(int displayPosition, int number, bool showDot)
        {
            if (displayPosition > 4) return;


    int bitmask = showDot? _numbertable[number] | _numbertable[number] << 7 : _numbertable[number];

            WriteDigitRaw(displayPosition, _numbertable[number] | (showDot << 7));
        }

        public void WriteDigitRaw(int displayPosition, byte bitmask)
        {
            _displaybuffer[displayPosition] = bitmask;
        }

        public async void WriteDisplay()
        {
            string aqs = I2cDevice.GetDeviceSelector("I2C1");

            // Find the I2C bus controller with our selector string
            var dis = await DeviceInformation.FindAllAsync(aqs);
            if (dis.Count == 0)
                return; // bus not found

            var settings = new I2cConnectionSettings(_deviceAddress);

            using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
            {
                List<int> bytes = new List<int> { 0x00 };

                for (int i = 0; i < 8; i++)
                {
                    bytes.Add(_displaybuffer[i] & 0xFF);
                    bytes.Add(_displaybuffer[i] >> 8);
                }

                device.Write(bytes.Select(Convert.ToByte).ToArray());
            }
        }
    }
}
