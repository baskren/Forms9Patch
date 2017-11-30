using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
{
    class MetaFont : Forms9Patch.MetaFont
    {
        // Item1: Name
        // Item2: Href
        public MetaFontAction Action { get; set; }
        public Xamarin.Forms.Color TextColor { get; set; }
        public Xamarin.Forms.Color BackgroundColor { get; set; }

        public bool Underline { get; set; }
        public bool Strikethrough { get; set; }

        public MetaFont(string family, double size, bool bold = false, bool italic = false, string id = null, string href = null, Xamarin.Forms.Color textColor = default(Xamarin.Forms.Color), Xamarin.Forms.Color backgroundColor = default(Xamarin.Forms.Color), bool underline = false, bool strikethrough=false) : base(family, size, bold, italic)
        {
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(href))
                Action = new MetaFontAction(id, href);
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            Underline = underline;
            Strikethrough = strikethrough;
        }

        public MetaFont(MetaFont f) : base (f)
        {
            if (f == null)
                return;
            Action = f.Action?.Copy();
            BackgroundColor = f.BackgroundColor;
            TextColor = f.TextColor;
            Underline = f.Underline;
            Strikethrough = f.Strikethrough;
        }

        public static bool operator ==(MetaFont f1, MetaFont f2) => Equals(f1, f2);

        public static bool operator !=(MetaFont f1, MetaFont f2) => !Equals(f1, f2);

        public override bool Equals(object obj)
        {
            if (obj is MetaFont other)
            {
                if (base.Equals(other))
                {
                    if (Action != other.Action)
                        return false;
                    if (TextColor != other.TextColor)
                        return false;
                    if (BackgroundColor != other.BackgroundColor)
                        return false;
                    if (Underline != other.Underline)
                        return false;
                    if (Strikethrough != other.Strikethrough)
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hash = base.GetHashCode();
            if (Action!=null)
                hash = hash * 23 + Action.GetHashCode();
            hash = hash * 23 + TextColor.GetHashCode();
            hash = hash * 23 + BackgroundColor.GetHashCode();
            hash = hash * 23 + Underline.GetHashCode();
            hash = hash * 23 + Strikethrough.GetHashCode();
            return hash;
        }

        public bool IsActionEmpty()
        {
            return Action == null || Action.IsEmpty();
        }
    }

    class MetaFontAction
    {
        public string Id { get; set; }

        public string Href { get; set; }

        public MetaFontAction(string id, string href)
        {
            Id = id;
            Href = href;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Id) && string.IsNullOrWhiteSpace(Href);
        }

        public MetaFontAction Copy()
        {
            if (IsEmpty())
                return null;
            return new MetaFontAction(Id, Href);
        }

        public static bool operator ==(MetaFontAction a1, MetaFontAction a2) => Equals(a1, a2);

        public static bool operator !=(MetaFontAction a1, MetaFontAction a2) => !Equals(a1, a2);

        public static bool Equals(MetaFontAction a1, MetaFontAction a2)
        {
            if (a1?.Id != a2?.Id)
                return false;
            if (a1?.Href != a2?.Href)
                return false;
            
            return true;
        }

        public override bool Equals(object obj) => Equals(this, obj);
        

        public override int GetHashCode()
        {
            int hash = 7;
            hash = hash * 17 + Id.GetHashCode();
            hash = hash * 17 + Href.GetHashCode();
            return hash;
        }

        public MetaFontAction(ActionSpan actionSpan)
        {
            Id = actionSpan.Id;
            Href = actionSpan.Href;
        }
    }
}
