using System;
using System.Collections;
using System.Collections.Generic;

namespace Forms9Patch.Droid
{
	internal class ByteBuffer
	{
		public byte[] Array;

		ulong address=0;

		public enum Endian
		{
			Little,
			Big
		}

		public Endian ByteOrder;

		public ByteBuffer (int size)
		{
			Array = new byte[size];
			ByteOrder = BitConverter.IsLittleEndian ? Endian.Little : Endian.Big;
		}


		public void Add(byte[] array) {
			foreach (var item in array)
				Array [address++] = item;
		}

		public void Add(object obj) {
			if (obj is byte)
				Array [address++] = (byte)obj;
			else
				Add (obj, 0);
		}

		public void Add(object obj, int bytes)  {
			if (obj == null)
				return;
			var enumerator = obj as IEnumerable;
			if (enumerator != null) {
				foreach (var item in enumerator) 
					Add (item);
				return;
			}
			byte[] array;
			string type = obj.GetType ().ToString ();
			switch (type) {
				case "System.Boolean": 	array = BitConverter.GetBytes ((bool) obj); 	break;
				case "System.UInt32": 	array = BitConverter.GetBytes ((uint) obj); 	break;
				case "System.UInt16": 	array = BitConverter.GetBytes ((ushort) obj); 	break;
				case "System.Single": 	array = BitConverter.GetBytes ((float) obj); 	break;
				case "System.Int64": 	array = BitConverter.GetBytes ((long) obj); 	break;
				case "System.Int32": 	array = BitConverter.GetBytes ((int)  obj); 	break;
				case "System.Int16": 	array = BitConverter.GetBytes ((short) obj); 	break;
				case "System.Double": 	array = BitConverter.GetBytes ((double) obj); 	break;
				case "System.Char": 	array = BitConverter.GetBytes ((char) obj); 	break;
				case "System.UInt64": 	array = BitConverter.GetBytes ((ulong) obj); 	break;
				default: return;
			}
			if (bytes <= 0)
				bytes = array.Length;
			if (ByteOrder == Endian.Little && BitConverter.IsLittleEndian ||
				ByteOrder == Endian.Big && !BitConverter.IsLittleEndian) {
				for (int i = 0; i < Math.Min(array.Length,bytes); i++)
					Array [address++] = array [i];

			} else {
				for (int i = array.Length - 1; i >= Math.Max(0,array.Length-bytes); i--)
					Array [address++] = array [i];
			}
		}
	}
}

