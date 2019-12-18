using System;
using System.IO;
using Xamarin.Forms;
using Android.Content;
using Android.Webkit;
using Android.Graphics;
using Android.Views;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using System.Collections.Generic;
using Android.Runtime;
using System.Linq;

namespace Forms9Patch.Droid.Permissions
{
    public class WriteExternalStorage
    {
        static readonly Random Random = new Random(4362);
        static readonly List<WriteExternalStorage> Instances = new List<WriteExternalStorage>();

        public static void Validate(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (permissions.Length == 1 && permissions[0] == Android.Manifest.Permission.WriteExternalStorage && Instances.FirstOrDefault(i => i.RequestCode == requestCode) is WriteExternalStorage instance)
            {
                if (grantResults.Any(g => g == Android.Content.PM.Permission.Denied))
                    instance.TaskCompletionSource.TrySetResult(false);
                else
                    instance.TaskCompletionSource.TrySetResult(true);
            }
        }

        int RequestCode;

        private WriteExternalStorage()
        {
            int max = 1 << 16;
            do
            {
                RequestCode = Random.Next();
            } while (RequestCode < 255 || RequestCode >= max);
            Instances.Add(this);
        }

        public static async Task<bool> ConfirmOrRequest()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M && Settings.Context.CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
            {
                if (!Settings.Context.PackageManager.GetPackageInfo(Settings.Context.PackageName, Android.Content.PM.PackageInfoFlags.Permissions)?.RequestedPermissions?.Any(r => r.Equals(Android.Manifest.Permission.WriteExternalStorage, StringComparison.OrdinalIgnoreCase)) ?? false)
                    throw new Exception($"You need to declare the permission: `{Android.Manifest.Permission.WriteExternalStorage}` in your AndroidManifest.xml");
                return await Request();
            }
            return true;
        }

        public TaskCompletionSource<bool> TaskCompletionSource = new TaskCompletionSource<bool>();
        public static Task<bool> Request()
        {
            var instance = new WriteExternalStorage();
            Android.Support.V4.App.ActivityCompat.RequestPermissions(Settings.Activity, new string[] { Android.Manifest.Permission.WriteExternalStorage }, instance.RequestCode);
            return instance.TaskCompletionSource.Task;
        }

    }
}
