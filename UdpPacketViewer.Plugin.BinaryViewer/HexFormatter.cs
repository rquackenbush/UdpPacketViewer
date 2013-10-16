using System;
using System.Linq;
using System.Text;

namespace UdpPacketViewer.Plugin.BinaryViewer
{
    internal static class HexFormatter
    {
        private const int HeaderWidth = 8;

        public static string Format(byte[] data)
        {
            const int Width = 16;

            var result = new StringBuilder();

            int currentIndex = 0;

            while(currentIndex < data.Length)
            {
                string line = FormatLine(currentIndex, data.Skip(currentIndex).Take(Width).ToArray(), Width);

                result.AppendLine(line);

                currentIndex += Width;
            }
        
            return result.ToString();
        }

        private static string FormatLine(int startingIndex, byte[] data, int width)
        {


            string result = Convert.ToString(startingIndex, 16).PadLeft(HeaderWidth, '0');

            result += "  ";

            for (int index = 0; index < data.Length; index++)
            {
                result += Convert.ToString(data[index], 16).PadLeft(2, '0') + " ";
            }

            result += " ";

            result = result.PadRight(HeaderWidth + (width * 3) + 3);

            result += GetAsciiChunk(data);

            return result;
        }

        private static string GetAsciiChunk(byte[] data)
        {
            string result = "";

            foreach (var value in data)
                result += GetDisplayChar(value);

            return result;
        }

        private static char GetDisplayChar(byte value)
        {
            if (value >= 32 && value <= 126)
                return (char) value;

            return '.';
        }
    }
}
