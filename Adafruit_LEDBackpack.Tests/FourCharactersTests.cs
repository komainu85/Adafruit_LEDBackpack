using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Adafruit_LEDBackpack.Tests
{
    [TestFixture]
    public class FourCharactersTests
    {
        [Test]
        public void BytesForClearDisplayAddedToDisplatBuffer()
        {
            var alphaNumericFourCharacters = new AlphaNumericFourCharacters();

            alphaNumericFourCharacters.ClearDisplay();

            Assert.Contains(new byte[] { Convert.ToByte(0), 0000000 }, alphaNumericFourCharacters.Displaybuffer);
        }
    }
}
