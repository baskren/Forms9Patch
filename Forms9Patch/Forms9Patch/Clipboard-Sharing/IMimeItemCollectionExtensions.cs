using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Forms9Patch
{
    /// <summary>
    /// Interface for MimeItemCollections
    /// </summary>
    public static class IMimeItemCollectionExtensions
    {
        /// <summary>
        /// MIME types in the collection
        /// </summary>
        /// <returns>The MIME types.</returns>
        /// <param name="mimeItemCollection">MIME item collection.</param>
        public static List<string> MimeTypes(this IMimeItemCollection mimeItemCollection)
        {
            return mimeItemCollection.Items.Select((mimeItem) => mimeItem.MimeType).ToList();
        }

        /// <summary>
        /// Gets the first MimeItem of a given MIME type.
        /// </summary>
        /// <returns>The first MIME item.</returns>
        /// <param name="mimeItemCollection">MimeItemCollection.</param>
        /// <param name="mimeType">MIME type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static MimeItem<T> GetFirstMimeItem<T>(this IMimeItemCollection mimeItemCollection, string mimeType)
        {
            //MimeItem<T> result = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            mimeType = mimeType.ToLower();
            foreach (var item in mimeItemCollection.Items)
                if (item.MimeType == mimeType)
                {
                    stopwatch.Stop();
                    //System.Diagnostics.Debug.WriteLine("\t\t GetFirstMimeItem<T> elapsed: " + stopwatch.ElapsedMilliseconds);
                    return MimeItem<T>.Create(item);
                }
            //var untypedMimeItem = clipboardEntry.Items.FirstOrDefault((mi) => mi.MimeType == mimeType);
            return null;
        }

        /// <summary>
        /// Gets the all MimeItems of a given MIME type
        /// </summary>
        /// <returns>The MimeItems.</returns>
        /// <param name="mimeItemCollection">MIME item collection.</param>
        /// <param name="mimeType">MIME type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<MimeItem<T>> GetMimeItems<T>(this IMimeItemCollection mimeItemCollection, string mimeType)
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
            foreach (var item in mimeItemCollection.Items)
                if (item.MimeType == mimeType)
                    mimeItems.Add(MimeItem<T>.Create(item));
            return mimeItems;
        }

        /// <summary>
        /// Adds the content from file (as byte[]) to collection.  Alternative to adding file as FileInfo as the Value of a MimeItem.
        /// </summary>
        /// <returns>The bytes from file.</returns>
        /// <param name="mimeItemCollection">MIME item collection.</param>
        /// <param name="mimeType">MIME type.</param>
        /// <param name="path">File Path.</param>
        /// <param name="failAction">What to do if the method fails</param>
        public static byte[] AddBytesFromFile(this Forms9Patch.MimeItemCollection mimeItemCollection, string mimeType, string path, FailAction failAction = FailAction.ShowAlert)
        {
            if (File.ReadAllBytes(path) is byte[] byteArray && byteArray.Length > 0)
            {
                mimeItemCollection.Items.Add(new MimeItem<byte[]>(mimeType, byteArray));
                return byteArray;
            }
            if (failAction == FailAction.ShowAlert)
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                {
                    using (var toast = Alert.Create(null, "Cannot access empty file [" + path + "]"))
                    {
                        await toast.WaitForPoppedAsync();
                    }
                });
            }
            else if (failAction == FailAction.ThrowException)
                throw new System.Exception("Cannot access empty file [" + path + "]");
            return null;
        }

        /// <summary>
        /// Does the MimeItemCollection contain at least one item of a given mimeType?
        /// </summary>
        /// <param name="mimeItemCollection"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static bool ContainsMimeType(this IMimeItemCollection mimeItemCollection, string mimeType)
        {
            mimeType = mimeType.ToLower();
            foreach (var item in mimeItemCollection.Items)
                if (item.MimeType == mimeType)
                    return true;
            return false;
        }

    }
}