using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    interface ISharingService
    {
        void Share(MimeItemCollection mimeItemCollection, VisualElement target);
    }
}