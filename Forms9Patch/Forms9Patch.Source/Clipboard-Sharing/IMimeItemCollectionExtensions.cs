using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Forms9Patch
{
    public static class IMimeItemCollectionExtensions
    {
        public static List<string> MimeTypes(this IMimeItemCollection clipboardEntry)
        {
            return clipboardEntry.Items.Select((mimeItem) => mimeItem.MimeType).ToList();
        }

        public static MimeItem<T> GetFirstMimeItem<T>(this IMimeItemCollection clipboardEntry, string mimeType)
        {
            //MimeItem<T> result = null;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            mimeType = mimeType.ToLower();
            foreach (var item in clipboardEntry.Items)
                if (item.MimeType == mimeType)
                {
                    stopwatch.Stop();
                    System.Diagnostics.Debug.WriteLine("\t\t GetFirstMimeItem<T> elapsed: " + stopwatch.ElapsedMilliseconds);
                    return MimeItem<T>.Create(item);
                }
            //var untypedMimeItem = clipboardEntry.Items.FirstOrDefault((mi) => mi.MimeType == mimeType);
            return null;
        }

        public static List<MimeItem<T>> GetMimeItems<T>(this IMimeItemCollection clipboardEntry, string mimeType)
        {
            mimeType = mimeType.ToLower();
            var mimeItems = new List<MimeItem<T>>();
            /*
            var untypedMimeItems = clipboardEntry.Items.FindAll((mi) => mi.MimeType == mimeType);
            if (untypedMimeItems != null && untypedMimeItems.Count > 0)
                foreach (var untypedMimeItem in untypedMimeItems)
                    if (MimeItem<T>.Create(untypedMimeItem) is MimeItem<T> item)
                        mimeItems.Add(item);
                        */
            foreach (var item in clipboardEntry.Items)
                if (item.MimeType == mimeType)
                    mimeItems.Add(MimeItem<T>.Create(item));
            return mimeItems;
        }

        public static byte[] AddBytesFromFile(this Forms9Patch.MimeItemCollection clipboardEntry, string mimeType, string path)
        {
            if (File.ReadAllBytes(path) is byte[] byteArray)
            {
                clipboardEntry.Items.Add(new MimeItem<byte[]>(mimeType, byteArray));
                return byteArray;
            }
            return null;
        }

    }
}