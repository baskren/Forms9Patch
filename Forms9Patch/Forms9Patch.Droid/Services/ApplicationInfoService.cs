// /*******************************************************************
//  *
//  * ApplicationInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Android.App;
using Xamarin.Forms;
using Android.Content.PM;
using Java.Security.Cert;
using System.IO;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
//using Android.Net;

[assembly: Dependency(typeof(Forms9Patch.Droid.ApplicationInfoService))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ApplicationInfoService : IApplicationInfoService
    {
        public int Build
        {
            get
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
                    return (int)Settings.Context.PackageManager.GetPackageInfo(Identifier, 0).LongVersionCode;
                else
#pragma warning disable CS0618 // Type or member is obsolete
                    return Settings.Context.PackageManager.GetPackageInfo(Identifier, 0).VersionCode;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        public string Identifier
        {
            get
            {
                return Settings.Context.PackageName;
            }
        }

        public string Name
        {
            get
            {
                return ((Activity)Settings.Context).Title;
            }
        }

        public string Version
        {
            get
            {
                return Settings.Context.PackageManager.GetPackageInfo(Identifier, 0).VersionName;
            }
        }

        public string Fingerprint
        {
            get
            {
                var pm = Settings.Context.PackageManager;
                var packageName = Identifier;
                var flags = PackageInfoFlags.Signatures;
                PackageInfo packageInfo = null;
                try
                {
                    packageInfo = pm.GetPackageInfo(packageName, flags);
                }
                catch (PackageManager.NameNotFoundException e)
                {
                    e.PrintStackTrace();
                }

                IList<string> signatures = null;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
                {
                    if (packageInfo?.SigningInfo is SigningInfo signingInfo)
                    {
                        if (signingInfo.HasMultipleSigners && signingInfo.GetApkContentsSigners() is Signature[] apkSignatures)
                            signatures = SignatureDigest(apkSignatures);
                        if ((signatures is null || signatures.Count < 1) && signingInfo.GetSigningCertificateHistory() is Signature[] historySignatures)
                            signatures = SignatureDigest(historySignatures);
                    }
                }
#pragma warning disable CS0618 // Type or member is obsolete
                else if ((signatures is null || signatures.Count < 1) && packageInfo?.Signatures is IList<Signature> prePiSignatures)
#pragma warning restore CS0618 // Type or member is obsolete
                    signatures = SignatureDigest(prePiSignatures.ToArray());

                if (signatures != null && signatures?.Count < 1)
                    return signatures[0];

                return null;
            }
        }

        private static string SignatureDigest(Signature sig)
        {
            byte[] cert = sig.ToByteArray();
            using (var input = new MemoryStream(cert))
            {
                CertificateFactory cf = null;
                try
                {
                    cf = CertificateFactory.GetInstance("X509");
                }
                catch (CertificateException e)
                {
                    e.PrintStackTrace();
                }
                X509Certificate c = null;
                try
                {
                    c = (X509Certificate)cf.GenerateCertificate(input);
                }
                catch (CertificateException e)
                {
                    e.PrintStackTrace();
                }
                string hexString = null;
                try
                {
                    var md = Java.Security.MessageDigest.GetInstance("SHA1");
                    byte[] publicKey = md.Digest(c.GetEncoded());
                    hexString = Byte2HexFormatted(publicKey);
                }
                catch (Java.Security.NoSuchAlgorithmException e1)
                {
                    e1.PrintStackTrace();
                }
                catch (CertificateEncodingException e)
                {
                    e.PrintStackTrace();
                }
                return hexString;
            }
        }
        private static List<string> SignatureDigest(Signature[] sigList)
        {
            var signaturesList = new List<string>();
            foreach (var signature in sigList)
            {
                if (signature != null)
                {
                    signaturesList.Add(SignatureDigest(signature));
                }
            }
            return signaturesList;
        }

        /*
        public NetworkConnectivity NetworkConnectivity
        {
            get
            {
                Android.Net.NetworkInfo networkInfo = Android.Net.ConnectivityManager.FromContext(Settings.Context).ActiveNetworkInfo;
                if (!networkInfo.IsConnected)
                    return NetworkConnectivity.None;
                if (networkInfo.Type == Android.Net.ConnectivityType.Wifi || networkInfo.Type == Android.Net.ConnectivityType.Ethernet || networkInfo.Type == Android.Net.ConnectivityType.Bluetooth)
                    return NetworkConnectivity.LAN;
                return NetworkConnectivity.Mobile;
            }
        }
        */

        public static string Byte2HexFormatted(byte[] arr)
        {
            using (var str = new StringBuilder(arr.Length * 2))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var h = Integer.ToHexString(arr[i]);
                    int l = h.Length;
                    if (l == 1) h = "0" + h;
                    if (l > 2) h = h.Substring(l - 2, l);
                    str.Append(h.ToUpper());
                    if (i < (arr.Length - 1)) str.Append(':');
                }
                return str.ToString();
            }
        }
    }
}
