using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Clipboard class
    /// </summary>
    public static class Sharing
    {
        static Sharing()
        {
            Settings.ConfirmInitialization();
        }

        static ISharingService _service;
        static ISharingService Service
        {
            get
            {
                _service = _service ?? Xamarin.Forms.DependencyService.Get<ISharingService>();
                return _service;
            }
        }

        public static void Share(MimeItemCollection collection, VisualElement target) => Service?.Share(collection, target);

    }
}