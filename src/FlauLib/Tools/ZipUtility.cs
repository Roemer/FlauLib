using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FlauLib.Tools
{
    /// <summary>
    /// Utility functions to handle zip/gzips without 3rd party dependencies
    /// Last updated: 14.01.2015
    /// </summary>
    public static class ZipUtility
    {
        public static void ExtractZip(byte[] data, string destinationFolder)
        {
            var des = new DirectoryInfo(destinationFolder);
            des.Create();
            using (var ms = new MemoryStream(data))
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Read, false))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Name == "")
                        {
                            // It's a folder, create it (recursively)
                            var folder = des;
                            var pathParts = entry.FullName.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var dir in pathParts)
                            {
                                folder = folder.CreateSubdirectory(dir);
                            }
                        }
                        else
                        {
                            // It's a file, save it
                            using (var fileData = entry.Open())
                            {
                                using (var fileStream = File.Create(Path.Combine(des.FullName, entry.FullName)))
                                {
                                    fileData.CopyTo(fileStream);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static byte[] CompressZip(string file)
        {
            var f = new FileInfo(file);
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var fileContent = File.ReadAllBytes(f.FullName);
                    var zipEntry = archive.CreateEntry(f.Name);
                    using (var entryStream = zipEntry.Open())
                    {
                        entryStream.Write(fileContent, 0, fileContent.Length);
                    }
                }
                return ms.ToArray();
            }
        }

        public static string ExtractGZipToString(byte[] gzipData, Encoding encoding = null)
        {
            var decodedData = ExtractGZip(gzipData);
            return (encoding ?? Encoding.Default).GetString(decodedData);
        }

        public static byte[] ExtractGZip(byte[] gzipData)
        {
            using (var stream = new GZipStream(new MemoryStream(gzipData), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
