using System;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public static class XamarinEssentialsExtensions
    {
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
