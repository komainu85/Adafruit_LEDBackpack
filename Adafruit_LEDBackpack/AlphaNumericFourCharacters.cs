using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Adafruit_LEDBackpack
{
    public sealed class AlphaNumericFourCharacters
    {
        private readonly int _deviceAddress;
        private readonly List<byte[]> _displaybuffer = new List<byte[]>();
        private readonly Dictionary<char, int> _displayCharacters = new Dictionary<char, int>();

        public AlphaNumericFourCharacters() : this(0x70)
        {
        }

        public AlphaNumericFourCharacters(Int32 deviceAddress)
        {
            _displaybuffer.Add(new byte[] { 0x21 });
            _deviceAddress = deviceAddress;

            _displayCharacters.Add('0', 0x3F);
            _displayCharacters.Add('1', 0x06);
            _displayCharacters.Add('2', 0x5B);
            _displayCharacters.Add('3', 0x4F);
            _displayCharacters.Add('4', 0x66);
            _displayCharacters.Add('5', 0x6D);
            _displayCharacters.Add('6', 0x7D);
            _displayCharacters.Add('7', 0x07);
            _displayCharacters.Add('8', 0x7F);
            _displayCharacters.Add('9', 0x6F);
        }

        public void SetBrightness(int brightness)
        {
            if (brightness > 15)
            {
                brightness = 15;
            }

            _displaybuffer.Add(new byte[] { 0xE0, Convert.ToByte(brightness) });
        }

        public void SetBlinkRate(int blinkRate)
        {
            if (blinkRate > 3)
            {
                blinkRate = 0;
            }

            _displaybuffer.Add(new byte[] { (byte)(0x80 | 0x01 | blinkRate << 1) });
        }

        public void ClearDisplay()
        {
            _displaybuffer.Add(new byte[] { 0x00, 0000000 });
            _displaybuffer.Add(new byte[] { 0x01, 0000000 });
            _displaybuffer.Add(new byte[] { 0x02, 0000000 });
            _displaybuffer.Add(new byte[] { 0x03, 0000000 });
            _displaybuffer.Add(new byte[] { 0x04, 0000000 });
            _displaybuffer.Add(new byte[] { 0x05, 0000000 });
            _displaybuffer.Add(new byte[] { 0x06, 0000000 });
            _displaybuffer.Add(new byte[] { 0x07, 0000000 });
            _displaybuffer.Add(new byte[] { 0x08, 0000000 });
        }

        public void WriteCharacters(char firstChar, char secondChar, char thirdChar, char forthChar)
        {
            _displaybuffer.Add(new byte[] { 0x00, Convert.ToByte(_displayCharacters[firstChar]) });
            _displaybuffer.Add(new byte[] { 0x02, Convert.ToByte(_displayCharacters[secondChar]) });
            _displaybuffer.Add(new byte[] { 0x04, Convert.ToByte(_displayCharacters[thirdChar]) });
            _displaybuffer.Add(new byte[] { 0x06, Convert.ToByte(_displayCharacters[forthChar]) });
        }

        public async void WriteDisplay()
        {
            var deviceInfo = await GetDevice();
            var settings = new I2cConnectionSettings(_deviceAddress);

            using (I2cDevice device = await I2cDevice.FromIdAsync(deviceInfo[0].Id, settings))
            {
                foreach (var byteValue in _displaybuffer)
                {
                    device.Write(byteValue);
                }
            }
        }

        private async Task<DeviceInformationCollection> GetDevice()
        {
            string aqs = I2cDevice.GetDeviceSelector("I2C1");

            // Find the I2C bus controller with our selector string
            var dis = await DeviceInformation.FindAllAsync(aqs);

            return dis;
        }
    }
}
