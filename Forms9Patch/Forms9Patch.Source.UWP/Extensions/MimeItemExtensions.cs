using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace Forms9Patch.UWP
{
    static class MimeItemExtensions
    {
        public static StorageFile ToStorageFile(this IMimeItem mimeItem)
        {
            StorageFile result = null;
            if (MimeSharp.Current.Extension(mimeItem.MimeType) is List<string> extensions && extensions.Count>1 && extensions[0] is string ext)
            {
                var value = mimeItem.Value;
                if (value is string text)
                {
                    var fileName = Guid.NewGuid().ToString() + "." + ext;
                    var path = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
                    File.WriteAllText(path, text);
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
                else if (value is byte[] byteArray)
                {
                    var fileName = Guid.NewGuid().ToString() + "." + ext;
                    var path = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
                    File.WriteAllBytes(path, byteArray);
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
                else if (value is FileInfo fileInfo)
                {
                    var path = fileInfo.FullName;
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
            }
            return result;
        }


        public static string AsString(this IMimeItem mimeItem)
        {
            var value = mimeItem.Value;
            if (value is FileInfo fileInfo && fileInfo.Exists && fileInfo.Length > 0)
                return File.ReadAllText(fileInfo.FullName);
            if (value is string text)
                return text;
            return null;
        }


    }
}