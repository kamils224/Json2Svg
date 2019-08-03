using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg.Tools
{
    public static class Zipper
    {
        public static MemoryStream Zip(List<ZipItem> zipItems)
        {
            var zipStream = new MemoryStream();

            using (var zip = new ZipArchive(zipStream,ZipArchiveMode.Create,true))
            {
                for (int i = 0; i < zipItems.Count; i++)
                {
                    var entry = zip.CreateEntry(zipItems[i].Name);
                    using (var entryStream = entry.Open())
                    {
                        zipItems[i].Content.CopyTo(entryStream);
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }
    }
}
