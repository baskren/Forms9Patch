using System;

namespace Forms9Patch
{
    public static class DataTransfer
    {
        static IDataTransfer Service => Xamarin.Forms.DependencyService.Get<IDataTransfer>();

        public static void TransferEntry(DataEntry entry) => Service?.Transfer(entry);
    }
}