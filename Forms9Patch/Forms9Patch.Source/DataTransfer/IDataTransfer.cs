using System;

namespace Forms9Patch
{
    interface IDataTransfer
    {
        void Transfer<T>(MimeItem<T> mimeItem);
    }
}