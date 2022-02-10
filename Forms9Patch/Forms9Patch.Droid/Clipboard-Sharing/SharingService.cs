using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Webkit;
using Android.Database;
using Android.Net;
using System.Collections.Generic;
using Java.Lang.Reflect;
using System.Runtime.InteropServices;
using Java.Nio.FileNio;
using System.Collections;
using System.IO;
using System.Linq;
using Android.OS;
using System.Reflection;
using Android.Content.Res;
using Android.Content.PM;
using Java.IO;
using P42.Utils;

[assembly: Dependency(typeof(Forms9Patch.Droid.SharingService))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SharingService : Forms9Patch.ISharingService
    {

        public void Share(Forms9Patch.MimeItemCollection mimeItemCollection, VisualElement target)
            => Share(mimeItemCollection, target, false);

        public void Share(Forms9Patch.MimeItemCollection mimeItemCollection, VisualElement target, bool retry)
        {
            var uris = mimeItemCollection.AsContentUris();
            if (uris.Count > 0)
            {
                Intent intent = new Intent();

                string html = null;
                string text = null;
                foreach (var item in ClipboardContentProvider.UriItems)
                {
                    if (html == null && item.Value.MimeType == "text/html")
                    {
                        html = (string)item.Value.Value;
                        break;
                    }
                    if (text == null && item.Value.MimeType == "text/plain")
                        text = (string)item.Value.Value;
                }
                if (!retry)
                {
                    if (html != null)
                    {
                        intent.PutExtra(Intent.ExtraText, text ?? html);
                        intent.PutExtra(Intent.ExtraHtmlText, html);
                    }
                    else if (text != null)
                        intent.PutExtra(Intent.ExtraText, text);
                }

                if (uris.Count == 1)
                {
                    intent.SetAction(Intent.ActionSend);
                    intent.PutExtra(Intent.ExtraStream, uris[0]);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetType(ClipboardContentProvider.UriItems[uris[0]].MimeType);
                }
                else
                {
                    intent.SetAction(Intent.ActionSendMultiple);
                    intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris.ToArray());
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetType(mimeItemCollection.LowestCommonMimeType());
                }


                try
                {
                    Forms9Patch.Droid.Settings.Activity.StartActivity(Intent.CreateChooser(intent, "Share ..."));
                }
                catch (Exception e)
                {
                    if (!retry)
                        Share(mimeItemCollection, target, true);
                    else
                    {
                        Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            using (var toast = Forms9Patch.Toast.Create("Sharing Failure", e.Message))
                            {
                                await toast.WaitForPoppedAsync();
                            }
                        });
                    }
                }
            }
        }
    }
}