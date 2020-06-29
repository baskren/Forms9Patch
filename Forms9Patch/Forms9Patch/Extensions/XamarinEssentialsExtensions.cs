using System;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Extension methods to make using Xamarin.Essentials just a tiny bit easier to use.
    /// </summary>
    public static class XamarinEssentialsExtensions
    {
        /// <summary>
        /// Combine CheckStatusAsync with RequestAsync 
        /// </summary>
        /// <typeparam name="TPermission">Xamarin.Essensials.Permissions</typeparam>
        /// <returns>true if permission granted, otherwise false</returns>
        public static async Task<bool> ConfirmOrRequest<TPermission>() where TPermission : Xamarin.Essentials.Permissions.BasePermission, new()
        {
            if (await Xamarin.Essentials.Permissions.CheckStatusAsync<TPermission>() != Xamarin.Essentials.PermissionStatus.Granted)
            {
                if (await Xamarin.Essentials.Permissions.RequestAsync<TPermission>() != Xamarin.Essentials.PermissionStatus.Granted)
                    return false;
            }
            return true;
        }
    }
}
