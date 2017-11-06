using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Forms9Patch.Droid
{
	static class ImageViewManagerExtension
	{
		static readonly Dictionary<string, byte[]>Cache = new Dictionary<string, byte[]>();
		static readonly Dictionary<string, List<ImageViewManager>>Clients = new Dictionary<string, List<ImageViewManager>>();

		static readonly object _constructorLock = new object();


#pragma warning disable 1998
		internal static async Task<byte[]> FetchStreamData(this ImageViewManager client, Xamarin.Forms.StreamImageSource streamSource)
#pragma warning restore 1998
		{
			var path =(string)streamSource?.GetValue (ImageSource.EmbeddedResourceIdProperty);
			if (path == null) 
				return null;

			lock (_constructorLock) {
				byte[] result=null;
				if (Cache.ContainsKey (path)) {
					Clients [path].Add (client);
					return Cache [path];
				}
				var assembly = (Assembly) streamSource.GetValue(ImageSource.AssemblyProperty);
				var stream = assembly.GetManifestResourceStream (path);
				if (stream == null) 
					return null;
				if (stream != null) {
					using (var outputStream = new MemoryStream())
					{
						stream.CopyTo(outputStream);
						result = outputStream.ToArray();
					}				
				}
				if (result == null) 
					return null;
				Cache [path] = result;
				if (!Clients.ContainsKey (path)) 
					Clients [path] = new List<ImageViewManager> ();
				Clients [path].Add (client);
				return result;
			}
		}



		internal static void ReleaseStreamBitmap(this ImageViewManager client, Xamarin.Forms.BindableObject streamSource) {
			var path = (string)streamSource?.GetValue (ImageSource.EmbeddedResourceIdProperty);
			if (path == null)
				return;
			lock (_constructorLock) {
				var clients = Clients [path];
				clients?.Remove (client);
				if (clients.Count > 0) 
					return;
				Clients.Remove (path);
				Cache.Remove (path);
			}
		}	
	}
}

