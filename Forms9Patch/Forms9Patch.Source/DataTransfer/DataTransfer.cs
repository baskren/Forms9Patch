using System;

namespace Forms9Patch
{
    public static class DataTransfer
    {
        static IDataTransfer Service => Xamarin.Forms.DependencyService.Get<IDataTransfer>();

        public static void TransferEntry<T>(MimeItem<T> mimeItem) => Service?.Transfer(mimeItem);
    }
}