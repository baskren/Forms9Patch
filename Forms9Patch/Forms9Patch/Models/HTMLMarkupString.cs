using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch HTML markup string.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class HTMLMarkupString : F9PFormattedString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLMarkupString"/> class.
        /// </summary>
        public HTMLMarkupString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLMarkupString"/> class.
        /// </summary>
        /// <param name="s">S.</param>
        public HTMLMarkupString(string s) : base(s)
        {
        }

        class Attribute
        {
            public string Name;
            public string Value;
        }

        class Tag
        {
            public string Name;
            public int Start;
            public List<Attribute> Attributes = new List<Attribute> { Capacity = 10 };
        }

        readonly StringBuilder _unmarkedText = new StringBuilder();
        /// <summary>
        /// Gets the unmarked text.
        /// </summary>
        /// <value>The unmarked text.</value>
        public string UnmarkedText => _unmarkedText.ToString();

        bool inPreSpan;
        void ProcessHTML()
        {
            _unmarkedText.Clear();
            if (string.IsNullOrWhiteSpace(_string))
                return;
            // remove previously Translated spans
            _spans.Clear();

            var tags = new List<Tag>();
            var index = 0;
            for (int i = 0; i < _string.Length; i++)
            {
                if (_string[i] == '<')
                {
                    var j = i + 1;
                    var closing = false;
                    if (j < _string.Length && _string[j] == '/')
                    {
                        closing = true;
                        j++;
                    }
                    while (j < _string.Length && _string[j] != '>')
                    {
                        j++;
                    }
                    if (j == _string.Length)
                    {
                        //throw new FormatException ("Incomplete HTML Tag [" + _string.Substring (i) + "] before end of string");
                        i = j;
                        break;
                    }
                    // it was a closing tag, so ...
                    var tagString = _string.Substring(i + (closing ? 2 : 1), j - i - (closing ? 2 : 1));
                    if (!string.IsNullOrWhiteSpace(tagString))
                    {
                        if (tagString == "br/" || tagString == "br")
                        {
                            //var startIndex = index;
                            //if (_unmarkedText[_unmarkedText.Length - 1] == '\n')
                            // {
                            //     index++;
                            //     _unmarkedText.Append("|");
                            // }
                            index++;
                            _unmarkedText.Append("\n");
                            //var span = new FontColorSpan(startIndex, index, Xamarin.Forms.Color.FromRgba(0, 0, 0, 1));
                            //_spans.Add(span);
                        }
                        else if (closing)
                        {
                            //var tag = tags.Pop ();
                            var tag = tags.FindLast((Tag obj) => obj.Name == tagString.ToLower());
                            if (tag?.Name != tagString.ToLower())
                            {
                                //throw new FormatException ("Mismatched opening [" + tags.Peek ().Name + "] to closing [" + tagString + "] HTML tags");
                                i = j;
                                continue;
                            }
                            tags.Remove(tag);
                            // remove tag off of stack
                            ProcessTag(tag, index);
                        }
                        else
                        {
                            // openning
                            //var contents = tagString.Split(' ');

                            var pattern = @" ([^>=]*)=[""']([^""']*)[""']";

                            var tagName = tagString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
                            var tag = new Tag { Name = tagName, Start = index };
                            foreach (Match m in Regex.Matches(tagString, pattern))
                            {
                                if (m.Groups.Count != 3)
                                    throw new FormatException("Cannot parse attributes for tag [" + tagString + "]");
                                var attrName = m.Groups[1].Value;
                                var attrValue = m.Groups[2].Value;
                                var attribute = new Attribute { Name = attrName, Value = attrValue };
                                tag.Attributes.Add(attribute);
                            }
                            tags.Add(tag);
                            inPreSpan = inPreSpan || tagName == "pre";
                        }
                    }
                    i = j;
                }
                else if (_string[i] == '&')
                {
                    // escape character
                    var escapeCodeBuilder = new StringBuilder();
                    //var stringBuilder = new StringBuilder();
                    var j = i + 1;
                    while (j < _string.Length && _string[j] != ';')
                        escapeCodeBuilder.Append(_string[j++]);

                    if (escapeCodeBuilder.Length > 1)
                    {
                        var escapeCode = escapeCodeBuilder.ToString();
                        var unicodeInt = 0;
                        if (escapeCode[0] == '#')
                        {
                            var start = 1;
                            var numBase = 10;
                            if (escapeCode[1] == 'x' || escapeCode[1] == 'X')
                            {
                                start = 2;
                                numBase = 16;
                            }
                            unicodeInt = Convert.ToInt32(escapeCode.Substring(start), numBase);
                        }
                        else
                        {
                            switch (escapeCode)
                            {
                                case "quot": unicodeInt = 0x0022; break;
                                case "amp": unicodeInt = 0x0026; break;
                                case "apos": unicodeInt = 0x0027; break;
                                case "lt": unicodeInt = 0x003C; break;
                                case "gt": unicodeInt = 0x003E; break;
                                case "nbsp": unicodeInt = 0x00A0; break;
                                case "iexcl": unicodeInt = 0x00A1; break;
                                case "cent": unicodeInt = 0x00A2; break;
                                case "pound": unicodeInt = 0x00A3; break;
                                case "curren": unicodeInt = 0x00A4; break;
                                case "yen": unicodeInt = 0x00A5; break;
                                case "brvbar": unicodeInt = 0x00A6; break;
                                case "sect": unicodeInt = 0x00A7; break;
                                case "uml": unicodeInt = 0x00A8; break;
                                case "copy": unicodeInt = 0x00A9; break;
                                case "ordf": unicodeInt = 0x00AA; break;
                                case "laquo": unicodeInt = 0x00AB; break;
                                case "not": unicodeInt = 0x00AC; break;
                                case "shy": unicodeInt = 0x00AD; break;
                                case "reg": unicodeInt = 0x00AE; break;
                                case "macr": unicodeInt = 0x00AF; break;
                                case "deg": unicodeInt = 0x00B0; break;
                                case "plusmn": unicodeInt = 0x00B1; break;
                                case "sup2": unicodeInt = 0x00B2; break;
                                case "sup3": unicodeInt = 0x00B3; break;
                                case "acute": unicodeInt = 0x00B4; break;
                                case "micro": unicodeInt = 0x00B5; break;
                                case "para": unicodeInt = 0x00B6; break;
                                case "middot": unicodeInt = 0x00B7; break;
                                case "cedil": unicodeInt = 0x00B8; break;
                                case "sup1": unicodeInt = 0x00B9; break;
                                case "ordm": unicodeInt = 0x00BA; break;
                                case "raquo": unicodeInt = 0x00BB; break;
                                case "frac14": unicodeInt = 0x00BC; break;
                                case "frac12": unicodeInt = 0x00BD; break;
                                case "frac34": unicodeInt = 0x00BE; break;
                                case "iquest": unicodeInt = 0x00BF; break;
                                case "Agrave": unicodeInt = 0x00C0; break;
                                case "Aacute": unicodeInt = 0x00C1; break;
                                case "Acirc": unicodeInt = 0x00C2; break;
                                case "Atilde": unicodeInt = 0x00C3; break;
                                case "Auml": unicodeInt = 0x00C4; break;
                                case "Aring": unicodeInt = 0x00C5; break;
                                case "AElig": unicodeInt = 0x00C6; break;
                                case "Ccedil": unicodeInt = 0x00C7; break;
                                case "Egrave": unicodeInt = 0x00C8; break;
                                case "Eacute": unicodeInt = 0x00C9; break;
                                case "Ecirc": unicodeInt = 0x00CA; break;
                                case "Euml": unicodeInt = 0x00CB; break;
                                case "Igrave": unicodeInt = 0x00CC; break;
                                case "Iacute": unicodeInt = 0x00CD; break;
                                case "Icirc": unicodeInt = 0x00CE; break;
                                case "Iuml": unicodeInt = 0x00CF; break;
                                case "ETH": unicodeInt = 0x00D0; break;
                                case "Ntilde": unicodeInt = 0x00D1; break;
                                case "Ograve": unicodeInt = 0x00D2; break;
                                case "Oacute": unicodeInt = 0x00D3; break;
                                case "Ocirc": unicodeInt = 0x00D4; break;
                                case "Otilde": unicodeInt = 0x00D5; break;
                                case "Ouml": unicodeInt = 0x00D6; break;
                                case "times": unicodeInt = 0x00D7; break;
                                case "Oslash": unicodeInt = 0x00D8; break;
                                case "Ugrave": unicodeInt = 0x00D9; break;
                                case "Uacute": unicodeInt = 0x00DA; break;
                                case "Ucirc": unicodeInt = 0x00DB; break;
                                case "Uuml": unicodeInt = 0x00DC; break;
                                case "Yacute": unicodeInt = 0x00DD; break;
                                case "THORN": unicodeInt = 0x00DE; break;
                                case "szlig": unicodeInt = 0x00DF; break;
                                case "agrave": unicodeInt = 0x00E0; break;
                                case "aacute": unicodeInt = 0x00E1; break;
                                case "acirc": unicodeInt = 0x00E2; break;
                                case "atilde": unicodeInt = 0x00E3; break;
                                case "auml": unicodeInt = 0x00E4; break;
                                case "aring": unicodeInt = 0x00E5; break;
                                case "aelig": unicodeInt = 0x00E6; break;
                                case "ccedil": unicodeInt = 0x00E7; break;
                                case "egrave": unicodeInt = 0x00E8; break;
                                case "eacute": unicodeInt = 0x00E9; break;
                                case "ecirc": unicodeInt = 0x00EA; break;
                                case "euml": unicodeInt = 0x00EB; break;
                                case "igrave": unicodeInt = 0x00EC; break;
                                case "iacute": unicodeInt = 0x00ED; break;
                                case "icirc": unicodeInt = 0x00EE; break;
                                case "iuml": unicodeInt = 0x00EF; break;
                                case "eth": unicodeInt = 0x00F0; break;
                                case "ntilde": unicodeInt = 0x00F1; break;
                                case "ograve": unicodeInt = 0x00F2; break;
                                case "oacute": unicodeInt = 0x00F3; break;
                                case "ocirc": unicodeInt = 0x00F4; break;
                                case "otilde": unicodeInt = 0x00F5; break;
                                case "ouml": unicodeInt = 0x00F6; break;
                                case "divide": unicodeInt = 0x00F7; break;
                                case "oslash": unicodeInt = 0x00F8; break;
                                case "ugrave": unicodeInt = 0x00F9; break;
                                case "uacute": unicodeInt = 0x00FA; break;
                                case "ucirc": unicodeInt = 0x00FB; break;
                                case "uuml": unicodeInt = 0x00FC; break;
                                case "yacute": unicodeInt = 0x00FD; break;
                                case "thorn": unicodeInt = 0x00FE; break;
                                case "yuml": unicodeInt = 0x00FF; break;
                                case "OElig": unicodeInt = 0x0152; break;
                                case "oelig": unicodeInt = 0x0153; break;
                                case "Scaron": unicodeInt = 0x0160; break;
                                case "scaron": unicodeInt = 0x0161; break;
                                case "Yuml": unicodeInt = 0x0178; break;
                                case "fnof": unicodeInt = 0x0192; break;
                                case "circ": unicodeInt = 0x02C6; break;
                                case "tilde": unicodeInt = 0x02DC; break;
                                case "Alpha": unicodeInt = 0x0391; break;
                                case "Beta": unicodeInt = 0x0392; break;
                                case "Gamma": unicodeInt = 0x0393; break;
                                case "Delta": unicodeInt = 0x0394; break;
                                case "Epsilon": unicodeInt = 0x0395; break;
                                case "Zeta": unicodeInt = 0x0396; break;
                                case "Eta": unicodeInt = 0x0397; break;
                                case "Theta": unicodeInt = 0x0398; break;
                                case "Iota": unicodeInt = 0x0399; break;
                                case "Kappa": unicodeInt = 0x039A; break;
                                case "Lambda": unicodeInt = 0x039B; break;
                                case "Mu": unicodeInt = 0x039C; break;
                                case "Nu": unicodeInt = 0x039D; break;
                                case "Xi": unicodeInt = 0x039E; break;
                                case "Omicron": unicodeInt = 0x039F; break;
                                case "Pi": unicodeInt = 0x03A0; break;
                                case "Rho": unicodeInt = 0x03A1; break;
                                case "Sigma": unicodeInt = 0x03A3; break;
                                case "Tau": unicodeInt = 0x03A4; break;
                                case "Upsilon": unicodeInt = 0x03A5; break;
                                case "Phi": unicodeInt = 0x03A6; break;
                                case "Chi": unicodeInt = 0x03A7; break;
                                case "Psi": unicodeInt = 0x03A8; break;
                                case "Omega": unicodeInt = 0x03A9; break;
                                case "alpha": unicodeInt = 0x03B1; break;
                                case "beta": unicodeInt = 0x03B2; break;
                                case "gamma": unicodeInt = 0x03B3; break;
                                case "delta": unicodeInt = 0x03B4; break;
                                case "epsilon": unicodeInt = 0x03B5; break;
                                case "zeta": unicodeInt = 0x03B6; break;
                                case "eta": unicodeInt = 0x03B7; break;
                                case "theta": unicodeInt = 0x03B8; break;
                                case "iota": unicodeInt = 0x03B9; break;
                                case "kappa": unicodeInt = 0x03BA; break;
                                case "lambda": unicodeInt = 0x03BB; break;
                                case "mu": unicodeInt = 0x03BC; break;
                                case "nu": unicodeInt = 0x03BD; break;
                                case "xi": unicodeInt = 0x03BE; break;
                                case "omicron": unicodeInt = 0x03BF; break;
                                case "pi": unicodeInt = 0x03C0; break;
                                case "rho": unicodeInt = 0x03C1; break;
                                case "sigmaf": unicodeInt = 0x03C2; break;
                                case "sigma": unicodeInt = 0x03C3; break;
                                case "tau": unicodeInt = 0x03C4; break;
                                case "upsilon": unicodeInt = 0x03C5; break;
                                case "phi": unicodeInt = 0x03C6; break;
                                case "chi": unicodeInt = 0x03C7; break;
                                case "psi": unicodeInt = 0x03C8; break;
                                case "omega": unicodeInt = 0x03C9; break;
                                case "thetasym": unicodeInt = 0x03D1; break;
                                case "upsih": unicodeInt = 0x03D2; break;
                                case "piv": unicodeInt = 0x03D6; break;
                                case "ensp": unicodeInt = 0x2002; break;
                                case "emsp": unicodeInt = 0x2003; break;
                                case "thinsp": unicodeInt = 0x2009; break;
                                case "zwnj": unicodeInt = 0x200C; break;
                                case "zwj": unicodeInt = 0x200D; break;
                                case "lrm": unicodeInt = 0x200E; break;
                                case "rlm": unicodeInt = 0x200F; break;
                                case "ndash": unicodeInt = 0x2013; break;
                                case "mdash": unicodeInt = 0x2014; break;
                                case "lsquo": unicodeInt = 0x2018; break;
                                case "rsquo": unicodeInt = 0x2019; break;
                                case "sbquo": unicodeInt = 0x201A; break;
                                case "ldquo": unicodeInt = 0x201C; break;
                                case "rdquo": unicodeInt = 0x201D; break;
                                case "bdquo": unicodeInt = 0x201E; break;
                                case "dagger": unicodeInt = 0x2020; break;
                                case "Dagger": unicodeInt = 0x2021; break;
                                case "bull": unicodeInt = 0x2022; break;
                                case "hellip": unicodeInt = 0x2026; break;
                                case "permil": unicodeInt = 0x2030; break;
                                case "prime": unicodeInt = 0x2032; break;
                                case "Prime": unicodeInt = 0x2033; break;
                                case "lsaquo": unicodeInt = 0x2039; break;
                                case "rsaquo": unicodeInt = 0x203A; break;
                                case "oline": unicodeInt = 0x203E; break;
                                case "frasl": unicodeInt = 0x2044; break;
                                case "euro": unicodeInt = 0x20AC; break;
                                case "image": unicodeInt = 0x2111; break;
                                case "weierp": unicodeInt = 0x2118; break;
                                case "real": unicodeInt = 0x211C; break;
                                case "trade": unicodeInt = 0x2122; break;
                                case "alefsym": unicodeInt = 0x2135; break;
                                case "larr": unicodeInt = 0x2190; break;
                                case "uarr": unicodeInt = 0x2191; break;
                                case "rarr": unicodeInt = 0x2192; break;
                                case "darr": unicodeInt = 0x2193; break;
                                case "harr": unicodeInt = 0x2194; break;
                                case "crarr": unicodeInt = 0x21B5; break;
                                case "lArr": unicodeInt = 0x21D0; break;
                                case "uArr": unicodeInt = 0x21D1; break;
                                case "rArr": unicodeInt = 0x21D2; break;
                                case "dArr": unicodeInt = 0x21D3; break;
                                case "hArr": unicodeInt = 0x21D4; break;
                                case "forall": unicodeInt = 0x2200; break;
                                case "part": unicodeInt = 0x2202; break;
                                case "exist": unicodeInt = 0x2203; break;
                                case "empty": unicodeInt = 0x2205; break;
                                case "nabla": unicodeInt = 0x2207; break;
                                case "isin": unicodeInt = 0x2208; break;
                                case "notin": unicodeInt = 0x2209; break;
                                case "ni": unicodeInt = 0x220B; break;
                                case "prod": unicodeInt = 0x220F; break;
                                case "sum": unicodeInt = 0x2211; break;
                                case "minus": unicodeInt = 0x2212; break;
                                case "lowast": unicodeInt = 0x2217; break;
                                case "radic": unicodeInt = 0x221A; break;
                                case "prop": unicodeInt = 0x221D; break;
                                case "infin": unicodeInt = 0x221E; break;
                                case "ang": unicodeInt = 0x2220; break;
                                case "and": unicodeInt = 0x2227; break;
                                case "or": unicodeInt = 0x2228; break;
                                case "cap": unicodeInt = 0x2229; break;
                                case "cup": unicodeInt = 0x222A; break;
                                case "int": unicodeInt = 0x222B; break;
                                case "there4": unicodeInt = 0x2234; break;
                                case "sim": unicodeInt = 0x223C; break;
                                case "cong": unicodeInt = 0x2245; break;
                                case "asymp": unicodeInt = 0x2248; break;
                                case "ne": unicodeInt = 0x2260; break;
                                case "equiv": unicodeInt = 0x2261; break;
                                case "le": unicodeInt = 0x2264; break;
                                case "ge": unicodeInt = 0x2265; break;
                                case "sub": unicodeInt = 0x2282; break;
                                case "sup": unicodeInt = 0x2283; break;
                                case "nsub": unicodeInt = 0x2284; break;
                                case "sube": unicodeInt = 0x2286; break;
                                case "supe": unicodeInt = 0x2287; break;
                                case "oplus": unicodeInt = 0x2295; break;
                                case "otimes": unicodeInt = 0x2297; break;
                                case "perp": unicodeInt = 0x22A5; break;
                                case "sdot": unicodeInt = 0x22C5; break;
                                case "lceil": unicodeInt = 0x2308; break;
                                case "rceil": unicodeInt = 0x2309; break;
                                case "lfloor": unicodeInt = 0x230A; break;
                                case "rfloor": unicodeInt = 0x230B; break;
                                case "lang": unicodeInt = 0x2329; break;
                                case "rang": unicodeInt = 0x232A; break;
                                case "loz": unicodeInt = 0x25CA; break;
                                case "spades": unicodeInt = 0x2660; break;
                                case "clubs": unicodeInt = 0x2663; break;
                                case "hearts": unicodeInt = 0x2665; break;
                                case "diams": unicodeInt = 0x2666; break;
                            }
                        }
                        i = j;
                        if (unicodeInt != 0)
                        {
                            var unicodeString = char.ConvertFromUtf32(unicodeInt);
                            index += unicodeString.Length;
                            _unmarkedText.Append(unicodeString);
                        }
                    }
                }
                else
                {
                    if (inPreSpan || !(char.IsWhiteSpace(_string, i) && i > 0 && char.IsWhiteSpace(_string, i - 1)))
                    {
                        index++;
                        _unmarkedText.Append(_string[i]);
                    }
                }
            }
            if (tags.Count > 0)
                foreach (var tag in tags)
                    ProcessTag(tag, index);
        }

        void ProcessTag(Tag tag, int index)
        {
            if (tag.Start >= index)
                return;
            float size;
            inPreSpan &= tag.Name != "pre";
            Span span;
            switch (tag.Name)
            {
                case "strong":
                case "b":
                    span = new BoldSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "em":
                case "i":
                    span = new ItalicsSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "sub":
                    span = new SubscriptSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "sup":
                    span = new SuperscriptSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "num":
                    span = new NumeratorSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "den":
                    span = new DenominatorSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "u":
                case "ins":
                    span = new UnderlineSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "font":
                    foreach (var attr in tag.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "color":
                                span = new FontColorSpan(tag.Start, index - 1, attr.Value.ToColor());
                                _spans.Add(span);
                                break;
#pragma warning disable CC0021 // Use nameof
                            case "size":
#pragma warning restore CC0021 // Use nameof
                                size = (float)attr.Value.ToFontSize();
                                span = new FontSizeSpan(tag.Start, index - 1, size);
                                _spans.Add(span);
                                break;
                            case "face":
                                span = new FontFamilySpan(tag.Start, index - 1, attr.Value);
                                _spans.Add(span);
                                break;
                        }
                    }
                    break;
                case "pre": // preformatted text
                case "tt": // teletype (monospaced)
                    span = new FontFamilySpan(tag.Start, index - 1, "Monospace");
                    _spans.Add(span);
                    break;
                case "strike":
                case "s": // strikethrough 
                case "del":
                    span = new StrikethroughSpan(tag.Start, index - 1);
                    _spans.Add(span);
                    break;
                case "big":
                    size = (float)"x-large".ToFontSize();
                    span = new FontSizeSpan(tag.Start, index - 1, size);
                    _spans.Add(span);
                    break;
                case "small":
                    size = (float)"x-small".ToFontSize();
                    span = new FontSizeSpan(tag.Start, index - 1, size);
                    _spans.Add(span);
                    break;
                case "a":
                    string id = null;
                    string href = null;
                    foreach (var attr in tag.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case nameof(href):
                                href = attr.Value;
                                break;
                            case nameof(id):
                                id = attr.Value;
                                break;
                        }
                    }
                    span = new ActionSpan(tag.Start, index - 1, id, href);
                    _spans.Add(span);
                    break;
            }
            // process  attributes
            foreach (var attr in tag.Attributes)
            {
                if (attr.Name == "style")
                {
                    var styleAttrs = attr.Value.Split(';');
                    foreach (var styleAttr in styleAttrs)
                    {
                        var strs = styleAttr.Split(':');
                        if (strs.Length == 2)
                        {
                            switch (strs[0].ToLower())
                            {
                                case "background-color":
                                    span = new BackgroundColorSpan(tag.Start, index - 1, strs[1].ToColor());
                                    _spans.Add(span);
                                    break;
                                case "color":
                                    span = new FontColorSpan(tag.Start, index - 1, strs[1].ToColor());
                                    _spans.Add(span);
                                    break;
                                case "font-family":
                                    span = new FontFamilySpan(tag.Start, index - 1, strs[1]);
                                    _spans.Add(span);
                                    break;
                                case "font-size":
                                    size = (float)strs[1].ToFontSize();
                                    span = new FontSizeSpan(tag.Start, index - 1, size);
                                    _spans.Add(span);
                                    break;
                                case "font-weight":
                                    if (strs[1].ToLower() == "bold")
                                    {
                                        span = new BoldSpan(tag.Start, index - 1);
                                        _spans.Add(span);
                                    }
                                    else
                                    {
                                        throw new FormatException("style=\"font-wieght: " + strs[1] + ";\" not supported");
                                    }
                                    break;
                                case "font-style":
                                    if (strs[1].ToLower() == "italic")
                                    {
                                        span = new ItalicsSpan(tag.Start, index - 1);
                                        _spans.Add(span);
                                    }
                                    else
                                    {
                                        throw new FormatException("style=\"font-style: " + strs[1] + ";\" not supported");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == "String")
                ProcessHTML();
            base.OnPropertyChanged(propertyName);
        }
    }
}

