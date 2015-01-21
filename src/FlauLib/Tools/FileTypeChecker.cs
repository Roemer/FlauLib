using System.IO;

namespace FlauLib.Tools
{
    public enum FileType
    {
        Unknown,
        JPG,
        PNG,
        PDF,
        TIFF,
        BMP,
        ICO,
        MOV,
        GIF,
        MIDI,
        POSTSCRIPT,
        DOSEXEC,
        ZIP,
    }

    /// <summary>
    /// Checks files or byte arrays for magic bytes to determine a filetype
    /// Last updated: 21.01.2015
    /// </summary>
    public static class FileTypeChecker
    {
        /// <summary>
        /// Analyzes a file to get the file type according to the header data
        /// </summary>
        public static FileType CheckFileType(string filePath)
        {
            return CheckFileType(new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
        }

        /// <summary>
        /// Analyzes binary array to get the file type according to the header data
        /// </summary>
        public static FileType CheckFileType(byte[] binaryData)
        {
            return CheckFileType(new BinaryReader(new MemoryStream(binaryData)));
        }

        /// <summary>
        /// Analyzes a binary reader to get the file type according to the header data
        /// </summary>
        public static FileType CheckFileType(BinaryReader binReader)
        {
            // Check for PDF
            if (CheckHeaderBytes(new byte[] { 0x25, 0x50, 0x44, 0x46 }, binReader))
            {
                return FileType.PDF;
            }
            // Check for PNG
            if (CheckHeaderBytes(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, binReader))
            {
                return FileType.PNG;
            }
            // Check for Gif
            if (CheckHeaderBytes(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, binReader) // "GIF89a"
                || CheckHeaderBytes(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, binReader)) // "GIF87a"
            {
                return FileType.GIF;
            }
            // Check for TIFF
            if (CheckHeaderBytes(new byte[] { 0x49, 0x49, 0x2A, 0x00 }, binReader) // Intel
                || CheckHeaderBytes(new byte[] { 0x4D, 0x4D, 0x00, 0x2A }, binReader)) // Motorola
            {
                return FileType.TIFF;
            }
            // Check for JPEG
            if (CheckHeaderBytes(new byte[] { 0xFF, 0xD8 }, binReader)
                && CheckLastBytes(new byte[] { 0xFF, 0xD9 }, binReader))
            {
                return FileType.JPG;
            }
            // Check for BMP
            if (CheckHeaderBytes(new byte[] { 0x42, 0x4D }, binReader))
            {
                return FileType.BMP;
            }
            // Check for MOV
            if (CheckHeaderBytes(new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 }, binReader))
            {
                return FileType.MOV;
            }
            // Check for ICO
            if (CheckHeaderBytes(new byte[] { 0x00, 0x00, 0x01, 0x00 }, binReader))
            {
                return FileType.ICO;
            }
            // Check for MIDI
            if (CheckHeaderBytes(new byte[] { 0x4D, 0x54, 0x68, 0x64 }, binReader))
            {
                return FileType.MIDI;
            }
            // Check for POSTSCRIPT
            if (CheckHeaderBytes(new byte[] { 0x25, 0x21 }, binReader))
            {
                return FileType.POSTSCRIPT;
            }
            // Check for DOSEXEC
            if (CheckHeaderBytes(new byte[] { 0x4D, 0x5A }, binReader) // Standard
               || CheckHeaderBytes(new byte[] { 0x5A, 0x4D }, binReader)) // Uncommon
            {
                return FileType.DOSEXEC;
            }
            // Check for ZIP
            if (CheckHeaderBytes(new byte[] { 0x50, 0x4B }, binReader))
            {
                return FileType.ZIP;
            }
            // Unknown type detected
            return FileType.Unknown;
        }

        /// <summary>
        /// Checks if the BinaryData starts with the Bytes given in the CheckBytes
        /// </summary>
        private static bool CheckHeaderBytes(byte[] checkBytes, BinaryReader binReader)
        {
            if (binReader.BaseStream.Length < checkBytes.Length)
            {
                return false;
            }

            binReader.BaseStream.Seek(0, SeekOrigin.Begin);
            var binaryData = binReader.ReadBytes(checkBytes.Length);
            return CheckBytes(checkBytes, binaryData);
        }

        /// <summary>
        /// Checks if the very last Bytes of BinaryData maches the Bytes in CheckBytes
        /// </summary>
        private static bool CheckLastBytes(byte[] checkBytes, BinaryReader binReader)
        {
            if (binReader.BaseStream.Length < checkBytes.Length)
            {
                return false;
            }
            binReader.BaseStream.Seek(-checkBytes.Length, SeekOrigin.End);
            var binaryData = binReader.ReadBytes(checkBytes.Length);
            return CheckBytes(checkBytes, binaryData);
        }

        private static bool CheckBytes(byte[] checkBytes, byte[] binaryData)
        {
            for (int byteIndex = 0; byteIndex < checkBytes.Length; byteIndex++)
            {
                if (binaryData[byteIndex] != checkBytes[byteIndex])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
