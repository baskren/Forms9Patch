using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormsGestures.UWP
{
    public class Settings
    {
        public static void Init()
        {
            Xamarin.Forms.DependencyService.Register<DisplayService>();
            Xamarin.Forms.DependencyService.Register<GestureService>();

            P42.Utils.Environment.Init();
#if NETSTANDARD
            P42.Utils.Environment.PlatformPathLoader = PlatformPathLoader;
#endif
            System.Diagnostics.Debug.WriteLine("FormsGestures.Droid.Settings.Init()");
        }

#if NETSTANDARD
        static void PlatformPathLoader()
        {
            var envVars = System.Environment.GetEnvironmentVariables();

            try
            {
                var documentsFolderPath = Windows.Storage.KnownFolders.DocumentsLibrary?.Path;
                P42.Utils.Environment.DocumentsPath = documentsFolderPath;//Windows.Storage.ApplicationData.Current.LocalFolder.Path; 
            }
            catch (System.UnauthorizedAccessException e)
            {
                // we don't have access.  Oh well.
            }
            P42.Utils.Environment.ApplicationDataPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            P42.Utils.Environment.ApplicationCachePath = Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;
            P42.Utils.Environment.TemporaryStoragePath = Windows.Storage.ApplicationData.Current.TemporaryFolder.Path;
        }
#endif
    }
}
