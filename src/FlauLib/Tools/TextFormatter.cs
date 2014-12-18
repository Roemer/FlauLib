using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlauLib.Tools
{
    public static class TextFormatter
    {
        /// <summary>
        /// Converts a List of string arrays to a string where each element in each line is correctly padded.
        /// Make sure that each array contains the same amount of elements!
        /// <example>
        /// --- Without:
        /// Title Name Street
        /// Mr. Roman Sesamstreet
        /// Mrs. Claudia Abbey Road
        /// --- With:
        /// Title   Name      Street
        /// Mr.     Roman     Sesamstreet
        /// Mrs.    Claudia   Abbey Road
        /// </example>
        /// <param name="lines">List lines, where each line is an array of elements for that line.</param>
        /// <param name="padding">Additional padding between each element (default = 1)</param>
        /// </summary>
        public static string PadElementsInLines(List<string[]> lines, int padding = 1)
        {
            // Calculate maximum numbers for each element accross all lines
            var numElements = lines[0].Length;
            var maxValues = new int[numElements];
            for (int i = 0; i < numElements; i++)
            {
                maxValues[i] = lines.Max(x => (x[i] ?? String.Empty).Length) + padding;
            }

            var sb = new StringBuilder();
            // Build the output
            bool isFirst = true;
            foreach (var line in lines)
            {
                if (!isFirst)
                {
                    sb.AppendLine();
                }
                isFirst = false;

                for (int i = 0; i < line.Length; i++)
                {
                    var value = line[i] ?? String.Empty;
                    // Append the value with padding of the maximum length of any value for this element
                    sb.Append(value.PadRight(maxValues[i]));
                }
            }
            return sb.ToString();
        }

        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        /// <summary>
        /// Converts a given number of bytes to a string with the appropriate suffix (eg. KB)
        /// </summary>
        /// <example>
        /// Simply convert some bytes. This will return "2KB"
        /// <code>
        /// SizeSuffix(2048);
        /// </code>
        /// </example>
        /// <param name="numBytes">The number of bytes</param>
        /// <param name="decimalPlaces">The number of decimal places</param>
        /// <returns>A string with the appropriate suffix</returns>
        public static string SizeSuffix(long numBytes, int decimalPlaces = 0)
        {
            if (numBytes < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "numBytes");
            }
            var mag = (int)Math.Max(0, Math.Log(numBytes, 1024));
            var adjustedSize = Math.Round(numBytes / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
