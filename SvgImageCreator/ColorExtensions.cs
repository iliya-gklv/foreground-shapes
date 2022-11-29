using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SvgImageCreator
{
    public static class ColorExtensions
    {
        public static string ToHexFormat(this Color color)
        {
            var hexArgb = $"{ColorTranslator.ToWin32(color):X8}";
            var hexRgb = hexArgb.Substring(2);
            var reversedBytes = ReverseBytesInColorStringRepresentation(hexRgb);
            return $"#{reversedBytes}";
        }

        private static string ReverseBytesInColorStringRepresentation(string hexColor)
        {
            var sb = new StringBuilder();
            for (int i = hexColor.Length-1; i > 0; i-=2)
            {
                sb.Append(hexColor[i-1]);
                sb.Append(hexColor[i]);
            }
            
            if (hexColor.Length % 2 != 0)
            {
                sb.Append(hexColor[0]);
                sb.Append(0);
            }

            return sb.ToString();
        }
    }
}