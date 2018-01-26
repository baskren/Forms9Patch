using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /*
    public class VersionString : IComparable
    {
        string[] _blocks;
        string _versionString;

        public string Major => Block(0);

        public string Minor => Block(1);

        public string Teriatary => Block(2);

        public string Quaternary => Block(3);

        public string Quinary => Block(4);

        public string Senary => Block(5);

        public string Septenary => Block(6);

        public string Octonary => Block(7);

        public VersionString(string versionString)
        {
            _versionString = versionString;
            _blocks = versionString.Split('.');
        }

        public override string ToString()
        {
            var result="";
            var lastIndex = _blocks.Length - 1;
            for (int i = 0; i < lastIndex; i++)
                result += _blocks[i] + ".";
            if (lastIndex>-1)
                result += _blocks[lastIndex];
            return result;
        }

        public string Block(int index)
        {
            if (index < _blocks.Length)
                return _blocks[index];
            return null;
        }

        public int CompareTo(object obj)
        {
            return _versionString.CompareVersionStrings(obj as string);
        }

        public static bool operator < (VersionString v1, VersionString v2) => v1.CompareTo(v2) < 0;

        public static bool operator >(VersionString v1, VersionString v2) => v1.CompareTo(v2) > 0;

        public static bool operator ==(Version)
    }
    */
}
