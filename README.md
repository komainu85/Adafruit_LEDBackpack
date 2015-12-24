# Adafruit_LEDBackpack
Windows 10 IoT Library for AdaFruit Four Character Display
```C#
var alpha = new AlphaNumericFourCharacters(0x70);
alpha.ClearDisplay();
alpha.SetBlinkRate(0);
alpha.SetBrightness(1);
alpha.WriteCharacters(5,4,4,3);
alpha.WriteDisplay();
```
