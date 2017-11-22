using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public class CustomFontEffect : Xamarin.Forms.RoutingEffect, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Assembly _assembly=null;
        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                if (value != _assembly)
                {
                    _assembly = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Assembly"));
                }
            }
        }

        public CustomFontEffect(Assembly assembly = null) : base("Forms9Patch.CustomFontEffect")
        {
            _assembly = assembly ??  (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
        }
    }
}
