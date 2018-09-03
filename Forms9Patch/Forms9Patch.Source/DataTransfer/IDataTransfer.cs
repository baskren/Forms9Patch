using System;

namespace Forms9Patch
{
    interface IDataTransfer
    {
        void Transfer(MimeItemCollection clipboardEntry);
    }
}