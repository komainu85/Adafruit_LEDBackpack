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
        public void BytesForClearDisplayAddedToDisplayBuffer()
        {
            var alphaNumericFourCharacters = new AlphaNumericFourCharacters();

            alphaNumericFourCharacters.ClearDisplay();

            for (int i = 0; i < 9; i++)
            {
                Assert.Contains(new byte[] { Convert.ToByte(i), 0000000 }, alphaNumericFourCharacters.Displaybuffer);
            }
        }
    }
}
