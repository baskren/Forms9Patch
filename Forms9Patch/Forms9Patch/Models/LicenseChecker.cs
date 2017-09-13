using System;
using System.Text;
using System.Security.Cryptography;


#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif

{

#if DEBUG
public class LicenseChecker
#else
internal class LicenseChecker
#endif
	{
	static readonly byte[][] Prefixes = new byte[][] {																																																																	
	// 32 bytes	1		2		3		4		5		6		7		8		9		10		11	12	12		13		14		15		16		17		18		19		20		21		22		23		24		25		26		27		28		29		30		31		32	
		new byte[] {	63	,	3	,	90	,	153	,	77	,	133	,	25	,	87	,	108	,	27	,	136	,	176	,	73	,	21	,	34	,	72	,	159	,	183	,	132	,	165	,	113	,	11	,	8	,	6	,	159	,	51	,	217	,	40	,	8	,	141	,	147	,	86	},
		new byte[] {	167	,	241	,	184	,	216	,	124	,	196	,	23	,	49	,	76	,	75	,	3	,	40	,	163	,	129	,	145	,	170	,	119	,	86	,	189	,	203	,	96	,	177	,	53	,	193	,	212	,	165	,	35	,	250	,	108	,	168	,	170	,	82	},
		new byte[] {	116	,	243	,	95	,	156	,	38	,	226	,	205	,	39	,	33	,	213	,	144	,	87	,	121	,	193	,	246	,	225	,	241	,	240	,	176	,	71	,	135	,	6	,	175	,	144	,	55	,	26	,	116	,	122	,	22	,	84	,	76	,	232	},
		new byte[] {	177	,	253	,	88	,	190	,	234	,	220	,	126	,	41	,	177	,	106	,	191	,	252	,	83	,	118	,	244	,	113	,	19	,	195	,	98	,	8	,	201	,	67	,	150	,	247	,	67	,	72	,	127	,	118	,	70	,	28	,	89	,	76	},
		new byte[] {	59	,	219	,	146	,	198	,	210	,	253	,	102	,	181	,	168	,	98	,	158	,	32	,	248	,	29	,	196	,	54	,	99	,	142	,	34	,	16	,	241	,	69	,	122	,	135	,	217	,	13	,	183	,	199	,	112	,	151	,	134	,	172	},
		new byte[] {	77	,	79	,	28	,	183	,	163	,	64	,	226	,	96	,	91	,	225	,	99	,	46	,	117	,	97	,	202	,	20	,	138	,	18	,	150	,	223	,	54	,	116	,	119	,	52	,	39	,	200	,	97	,	185	,	107	,	177	,	203	,	78	},
		new byte[] {	83	,	94	,	22	,	136	,	98	,	61	,	28	,	179	,	216	,	244	,	180	,	162	,	123	,	238	,	73	,	24	,	111	,	52	,	52	,	210	,	6	,	201	,	210	,	16	,	14	,	52	,	192	,	34	,	118	,	95	,	207	,	98	},
		new byte[] {	71	,	47	,	101	,	238	,	91	,	2	,	48	,	19	,	128	,	56	,	72	,	70	,	36	,	21	,	227	,	117	,	82	,	25	,	212	,	45	,	67	,	54	,	122	,	204	,	206	,	68	,	159	,	225	,	125	,	132	,	130	,	13	},
		new byte[] {	194	,	41	,	214	,	213	,	114	,	59	,	22	,	2	,	255	,	103	,	41	,	210	,	21	,	185	,	24	,	203	,	148	,	90	,	3	,	207	,	163	,	44	,	145	,	167	,	240	,	210	,	168	,	123	,	39	,	159	,	246	,	54	},
		new byte[] {	231	,	119	,	172	,	140	,	40	,	223	,	150	,	76	,	160	,	158	,	191	,	153	,	232	,	178	,	211	,	51	,	142	,	241	,	83	,	59	,	20	,	144	,	139	,	186	,	8	,	160	,	196	,	186	,	176	,	237	,	51	,	208	},
		new byte[] {	18	,	248	,	89	,	140	,	190	,	162	,	122	,	167	,	83	,	142	,	255	,	32	,	217	,	204	,	254	,	242	,	31	,	212	,	196	,	121	,	91	,	226	,	30	,	74	,	18	,	81	,	2	,	64	,	114	,	49	,	230	,	144	},
		new byte[] {	181	,	196	,	187	,	253	,	75	,	60	,	129	,	216	,	81	,	67	,	59	,	178	,	44	,	230	,	39	,	181	,	113	,	167	,	87	,	202	,	122	,	223	,	31	,	116	,	65	,	211	,	105	,	156	,	161	,	103	,	244	,	91	},
		new byte[] {	200	,	73	,	218	,	74	,	49	,	139	,	221	,	108	,	162	,	195	,	16	,	87	,	216	,	200	,	83	,	196	,	157	,	233	,	210	,	113	,	243	,	171	,	55	,	105	,	105	,	61	,	109	,	226	,	157	,	186	,	15	,	87	},
		new byte[] {	162	,	23	,	121	,	100	,	74	,	24	,	21	,	23	,	175	,	255	,	57	,	22	,	225	,	10	,	137	,	18	,	44	,	248	,	193	,	35	,	159	,	123	,	75	,	239	,	54	,	150	,	21	,	245	,	187	,	212	,	188	,	62	},
		new byte[] {	48	,	74	,	103	,	187	,	108	,	37	,	150	,	189	,	129	,	254	,	183	,	226	,	200	,	71	,	115	,	248	,	64	,	28	,	91	,	136	,	52	,	246	,	211	,	10	,	124	,	43	,	210	,	217	,	113	,	92	,	47	,	50	},
		new byte[] {	146	,	180	,	61	,	2	,	180	,	149	,	159	,	222	,	3	,	66	,	161	,	50	,	152	,	199	,	109	,	54	,	78	,	239	,	217	,	148	,	61	,	82	,	55	,	233	,	127	,	165	,	183	,	139	,	48	,	34	,	172	,	135	},
	};																																																																	

	static readonly byte[][] Postfixes = new byte[][] {																																																																	
	// 24 bytes	1		2		3		4		5		6		7		8		9		10		11	12	12		13		14		15		16		17		18		19		20		21		22		23		24																	
		new byte[] {	217	,	161	,	212	,	176	,	188	,	70	,	36	,	120	,	64	,	110	,	145	,	82	,	235	,	48	,	192	,	98	,	64	,	224	,	215	,	3	,	246	,	21	,	232	,	38	},																
		new byte[] {	97	,	53	,	6	,	203	,	0	,	224	,	23	,	7	,	23	,	120	,	48	,	184	,	68	,	150	,	171	,	50	,	248	,	154	,	207	,	10	,	227	,	87	,	116	,	150	},																
		new byte[] {	107	,	143	,	48	,	211	,	75	,	197	,	197	,	187	,	187	,	243	,	132	,	161	,	212	,	219	,	175	,	71	,	247	,	113	,	179	,	56	,	49	,	201	,	227	,	206	},																
		new byte[] {	114	,	29	,	123	,	38	,	40	,	233	,	42	,	91	,	102	,	230	,	123	,	162	,	230	,	241	,	216	,	190	,	17	,	61	,	127	,	202	,	100	,	46	,	119	,	9	},																
		new byte[] {	206	,	249	,	171	,	45	,	227	,	171	,	31	,	103	,	223	,	209	,	44	,	129	,	198	,	28	,	190	,	169	,	185	,	95	,	88	,	230	,	189	,	208	,	55	,	209	},																
		new byte[] {	128	,	75	,	216	,	55	,	63	,	48	,	241	,	118	,	31	,	63	,	197	,	211	,	188	,	241	,	181	,	160	,	55	,	163	,	147	,	83	,	220	,	15	,	218	,	201	},																
		new byte[] {	137	,	85	,	81	,	198	,	196	,	103	,	252	,	40	,	177	,	102	,	135	,	164	,	34	,	72	,	196	,	17	,	112	,	236	,	47	,	224	,	137	,	84	,	35	,	80	},																
		new byte[] {	143	,	224	,	175	,	202	,	221	,	66	,	187	,	6	,	2	,	119	,	21	,	149	,	108	,	240	,	175	,	203	,	168	,	12	,	104	,	71	,	108	,	226	,	127	,	73	},																
		new byte[] {	144	,	253	,	225	,	91	,	5	,	231	,	189	,	165	,	125	,	165	,	167	,	50	,	85	,	93	,	141	,	152	,	13	,	1	,	44	,	38	,	13	,	100	,	40	,	186	},																
		new byte[] {	28	,	188	,	129	,	250	,	142	,	97	,	129	,	214	,	33	,	221	,	172	,	244	,	148	,	93	,	42	,	5	,	213	,	68	,	139	,	79	,	3	,	86	,	253	,	91	},																
		new byte[] {	179	,	236	,	183	,	38	,	191	,	54	,	220	,	139	,	248	,	239	,	106	,	248	,	88	,	251	,	129	,	86	,	56	,	110	,	185	,	83	,	243	,	245	,	97	,	191	},																
		new byte[] {	43	,	52	,	21	,	246	,	81	,	253	,	87	,	237	,	8	,	2	,	189	,	239	,	218	,	102	,	245	,	249	,	84	,	226	,	223	,	4	,	187	,	14	,	223	,	228	},																
		new byte[] {	255	,	208	,	170	,	21	,	242	,	166	,	169	,	148	,	204	,	211	,	241	,	95	,	132	,	180	,	10	,	131	,	251	,	123	,	157	,	137	,	164	,	94	,	70	,	6	},																
		new byte[] {	74	,	105	,	199	,	103	,	105	,	21	,	28	,	248	,	204	,	117	,	169	,	253	,	12	,	253	,	221	,	206	,	118	,	114	,	209	,	71	,	69	,	13	,	19	,	94	},																
		new byte[] {	238	,	252	,	155	,	142	,	221	,	200	,	70	,	92	,	117	,	69	,	89	,	65	,	147	,	23	,	187	,	238	,	105	,	192	,	125	,	122	,	132	,	16	,	60	,	111	},																
		new byte[] {	123	,	57	,	138	,	202	,	197	,	171	,	218	,	101	,	47	,	62	,	226	,	19	,	11	,	252	,	223	,	158	,	37	,	122	,	82	,	240	,	106	,	217	,	4	,	214	},																
	};																																																																	

	static readonly byte[][] IVs = new byte[][] {																																																																	
	// 8 bytes	1		2		3		4		5		6		7		8																																																	
		new byte[] {	225	,	93	,	196	,	167	,	5	,	29	,	78	,	187	},																																																
		new byte[] {	75	,	152	,	255	,	169	,	74	,	210	,	78	,	19	},																																																
		new byte[] {	122	,	13	,	2	,	160	,	215	,	179	,	183	,	167	},																																																
		new byte[] {	31	,	220	,	75	,	110	,	13	,	137	,	4	,	198	},																																																
		new byte[] {	137	,	63	,	0	,	176	,	11	,	209	,	60	,	226	},																																																
		new byte[] {	16	,	89	,	152	,	175	,	195	,	123	,	222	,	45	},																																																
		new byte[] {	242	,	162	,	104	,	0	,	52	,	205	,	55	,	94	},																																																
		new byte[] {	23	,	128	,	217	,	134	,	105	,	68	,	173	,	219	},																																																
		new byte[] {	147	,	139	,	37	,	190	,	229	,	231	,	0	,	180	},																																																
		new byte[] {	197	,	184	,	78	,	239	,	162	,	106	,	97	,	230	},																																																
		new byte[] {	92	,	55	,	178	,	84	,	231	,	65	,	65	,	246	},																																																
		new byte[] {	27	,	247	,	19	,	53	,	44	,	145	,	208	,	147	},																																																
		new byte[] {	45	,	222	,	236	,	121	,	106	,	120	,	157	,	209	},																																																
		new byte[] {	175	,	125	,	3	,	157	,	233	,	80	,	249	,	214	},																																																
		new byte[] {	245	,	60	,	217	,	211	,	87	,	244	,	248	,	96	},																																																
		new byte[] {	8	,	187	,	163	,	49	,	115	,	81	,	40	,	217	},																																																
	};																																																																	

	static readonly byte[][] Keys = new byte[][] {																																																																	
	// 24 bytes	1		2		3		4		5		6		7		8		9		10		11	12	12		13		14		15		16		17		18		19		20		21		22		23		24																	
		new byte[] {	28	,	16	,	217	,	97	,	104	,	68	,	230	,	208	,	225	,	18	,	204	,	36	,	112	,	12	,	223	,	221	,	135	,	119	,	53	,	248	,	190	,	28	,	165	,	18	},																
		new byte[] {	33	,	62	,	205	,	199	,	206	,	242	,	166	,	47	,	18	,	172	,	236	,	250	,	89	,	84	,	211	,	220	,	252	,	69	,	195	,	158	,	109	,	115	,	158	,	100	},																
		new byte[] {	204	,	110	,	21	,	12	,	12	,	76	,	230	,	64	,	246	,	147	,	66	,	49	,	233	,	93	,	255	,	219	,	248	,	154	,	71	,	184	,	180	,	213	,	64	,	45	},																
		new byte[] {	122	,	101	,	85	,	76	,	242	,	33	,	112	,	243	,	245	,	32	,	50	,	128	,	198	,	229	,	39	,	23	,	217	,	25	,	107	,	83	,	45	,	245	,	61	,	62	},																
		new byte[] {	64	,	159	,	244	,	255	,	122	,	136	,	119	,	103	,	97	,	129	,	81	,	141	,	160	,	232	,	30	,	193	,	244	,	213	,	139	,	23	,	6	,	60	,	253	,	17	},																
		new byte[] {	243	,	121	,	10	,	46	,	234	,	219	,	90	,	45	,	15	,	25	,	138	,	14	,	152	,	218	,	217	,	2	,	111	,	236	,	207	,	6	,	163	,	148	,	179	,	85	},																
		new byte[] {	216	,	134	,	27	,	8	,	239	,	6	,	119	,	223	,	77	,	240	,	128	,	171	,	152	,	204	,	95	,	2	,	10	,	100	,	168	,	128	,	78	,	159	,	168	,	31	},																
		new byte[] {	231	,	194	,	25	,	107	,	107	,	174	,	169	,	181	,	34	,	206	,	198	,	117	,	51	,	191	,	208	,	50	,	89	,	185	,	18	,	80	,	191	,	249	,	36	,	77	},																
		new byte[] {	252	,	248	,	185	,	67	,	44	,	252	,	61	,	4	,	205	,	110	,	187	,	88	,	221	,	255	,	19	,	157	,	249	,	4	,	60	,	20	,	179	,	215	,	208	,	6	},																
		new byte[] {	245	,	146	,	109	,	254	,	100	,	145	,	171	,	81	,	208	,	200	,	187	,	121	,	235	,	193	,	164	,	8	,	79	,	110	,	166	,	39	,	132	,	167	,	227	,	158	},																
		new byte[] {	216	,	70	,	146	,	25	,	185	,	128	,	87	,	190	,	3	,	197	,	88	,	164	,	207	,	128	,	143	,	183	,	172	,	229	,	231	,	159	,	174	,	155	,	43	,	1	},																
		new byte[] {	157	,	89	,	96	,	179	,	199	,	172	,	4	,	15	,	85	,	94	,	163	,	19	,	120	,	148	,	17	,	217	,	87	,	121	,	145	,	47	,	196	,	216	,	113	,	53	},																
		new byte[] {	156	,	138	,	139	,	83	,	217	,	69	,	206	,	12	,	17	,	250	,	38	,	215	,	187	,	10	,	50	,	86	,	11	,	192	,	201	,	70	,	34	,	77	,	142	,	15	},																
		new byte[] {	209	,	42	,	158	,	198	,	14	,	105	,	114	,	243	,	115	,	127	,	59	,	164	,	126	,	232	,	128	,	94	,	234	,	37	,	35	,	172	,	222	,	17	,	242	,	76	},																
		new byte[] {	165	,	201	,	19	,	204	,	150	,	62	,	143	,	212	,	210	,	83	,	221	,	140	,	25	,	174	,	37	,	253	,	38	,	71	,	66	,	60	,	52	,	193	,	245	,	56	},																
		new byte[] {	71	,	148	,	217	,	54	,	163	,	179	,	122	,	65	,	94	,	219	,	151	,	6	,	188	,	240	,	52	,	199	,	119	,	216	,	231	,	23	,	138	,	21	,	55	,	234	},																
	};																																																																	

	static readonly string Base32Chars = "QMXGD3VCUN57KBZPTARJE8SWFH429YL6";																																																																																																																																																																																																																																																																		


#if DEBUG
		public bool CheckLicenseKey(string licenseKey, string appName)
#else
		internal bool CheckLicenseKey(string licenseKey, string appName)
#endif
		{
			bool result;
			try
			{
				if (licenseKey == null || licenseKey.Length != 64)
					result = false;
				else if (appName == null || appName.Length < 1)
					result = false;
				else
				{
					byte[] base32LicenseKey = LicenseChecker.Base32StringToBytes(licenseKey);
					int num = LicenseChecker.GetNumber(base32LicenseKey);
					num = (num + 9 & 15);
					byte[] key = LicenseChecker.Keys[num];
					num = (num + 11 & 15);
					byte[] iv = LicenseChecker.IVs[num];
					num = (num + 4 & 15);
					byte[] postfix = LicenseChecker.Postfixes[num];
					num = (num + 13 & 15);
					byte[] prefix = LicenseChecker.Prefixes[num];
					byte[] byteAppName = Encoding.Unicode.GetBytes(appName);
					var composite = new byte[prefix.Length + byteAppName.Length + postfix.Length];
					prefix.CopyTo(composite, 0);
					byteAppName.CopyTo(composite, prefix.Length);
					postfix.CopyTo(composite, prefix.Length + byteAppName.Length);
					byte[] data = Encrypt(composite, iv, key);
					byte[] hash = ComputeHash(data);
					if (base32LicenseKey.Length - 1 != hash.Length)
						result = false;
					else
					{
						for (int i = 0; i < hash.Length; i++)
							if (base32LicenseKey[i] != hash[i])
								return false;
						result = true;
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		static byte[] Encrypt(byte[] cleartext, byte[] iv, byte[] key)
		{
			TripleDES tripleDES = TripleDES.Create();
			tripleDES.IV = iv;
			tripleDES.Key = key;
			tripleDES.Mode = System.Security.Cryptography.CipherMode.CBC;
			tripleDES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
			return tripleDES.CreateEncryptor().TransformFinalBlock(cleartext, 0, cleartext.Length);
		}

		static byte[] ComputeHash(byte[] data)
		{
			return SHA256.Create().ComputeHash(data);
		}

		static int GetNumber(byte[] bytes) {
			int num = 0;
			int i = 6;
			if ((bytes[i] & 2) > 0)
				num += 4;
			int u, m, l;
			u = (b2i (bytes [i]    ) & 252);
			m = (b2i (bytes [i]    ) &   1) << 1;
			l = (b2i (bytes [i + 1]) & 128) >> 7;
			bytes[i] = (byte)((u + m + l)&0xFF);
			for (i++; i < 13; i++) {
				u = b2i (bytes [i]    ) << 1;
				l = b2i (bytes [i + 1]) >> 7;
				bytes [i] = (byte)((u + l) & 0xFF);
			}
			if ((bytes[i] & 8) > 0)
				num++;
			u = (b2i (bytes [i]    ) & 112) << 1;
			m = (b2i (bytes [i]    ) & 7  ) << 2;
			l = (b2i (bytes [i + 1]) & 192) >> 6;
			bytes [i] = (byte)((u + m + l) & 0xff);
			for (i++; i < 25; i++) {
				u = b2i (bytes [i]    ) << 2;
				l = b2i (bytes [i + 1]) >> 6;
				bytes [i] = (byte)((u + l) & 0xFF);
			}
			if ((bytes[i] & 128) > 0)
				num += 8;
			u = (b2i (bytes [i - 1]) & 252);
			l = (b2i (bytes [i]    ) & 96 ) >> 5;
			bytes[i - 1] = (byte)((u+ l)&0xFF);
			u = (b2i (bytes [i]    ) & 31 ) << 3;
			l = (b2i (bytes [i + 1]) & 224) >> 5;
			bytes[i] = (byte)((u + l)&0xFF);
			for (i++; i < 29; i++) {
				u = b2i (bytes [i]) << 3;
				l = b2i (bytes [i + 1]) >> 5;
				bytes [i] = (byte)((u + l) & 0xFF);
			}
			if ((bytes[i] & 16) > 0)
				num += 2;
			while (i < bytes.Length - 1) {
				u = b2i (bytes [i]) << 4;
				l = b2i (bytes [i + 1]) >> 4;
				bytes[i] = (byte)((u + l) & 0xFF);
				i++;
			}
			return num;
		}

		static byte[] Base32StringToBytes(string base32String) {
			base32String = base32String.Replace("-", "");
			var array = new byte[base32String.Length * 5 + 7 >> 3];
			int bitOffset = 0;
			string text = base32String;
			for (int i = 0; i < text.Length; i++) {
				char value = text[i];
				int first3BitsOfOffset = bitOffset & 7;
				int restOfBitsOfOffset = bitOffset >> 3;
				int index = LicenseChecker.Base32Chars.IndexOf(value);
				if (index == -1)
					throw new ArgumentException("Input string is no base32 string.");
				if (first3BitsOfOffset < 4)
					array[restOfBitsOfOffset]    += (byte)(index << 3 - first3BitsOfOffset);
				else {
					array[restOfBitsOfOffset]    += (byte)(index >> first3BitsOfOffset - 3);
					array[restOfBitsOfOffset + 1] = (byte)(index << 11 - first3BitsOfOffset & 0xFF);
				}
				bitOffset += 5;
			}
			return array;
		}

		public static void dumpBytes(string label, byte[] byteArray) {
			/*
			#if DEBUG
			System.Diagnostics.Debug.Write (label+": ");
			for (int i = 0; i < byteArray.Length; i++)
				System.Diagnostics.Debug.Write (" " + byteArray[i]);
			System.Diagnostics.Debug.Write ("\n");
			#endif
			*/
		}

	static int b2i(byte b) {
		return ((int)b & 0xFF);
	}


		#if DEBUG
		public static string GenLicenseKey(string appName, int num) {
			if (appName == null || appName.Length < 1)
				return null;
			else {
				byte[] prefix = LicenseChecker.Prefixes[num];
				dumpBytes ("prefix", prefix);
				num = (num + 3 & 15);
				byte[] postfix = LicenseChecker.Postfixes[num];
				dumpBytes ("postfix", postfix);
				num = (num + 12 & 15);
				byte[] iv = LicenseChecker.IVs[num];
				dumpBytes ("iv", iv);
				num = (num + 5 & 15);
				byte[] key = LicenseChecker.Keys[num];
				dumpBytes ("key", key);
				num = (num + 7 & 15);
				byte[] byteAppName = Encoding.Unicode.GetBytes (appName);
		
				dumpBytes ("byteAppName", byteAppName);
				System.Diagnostics.Debug.WriteLine(Encoding.Unicode.GetString(byteAppName));

				var composite = new byte[prefix.Length + byteAppName.Length + postfix.Length];
				prefix.CopyTo(composite, 0);
				byteAppName.CopyTo(composite, prefix.Length);
				postfix.CopyTo(composite, prefix.Length + byteAppName.Length);
				dumpBytes ("composite", composite);

				byte[] data = Encrypt(composite, iv, key);
				dumpBytes ("data", data);

				byte[] base32LicenseKey = ComputeHash(data);
				dumpBytes ("hash", base32LicenseKey);
				base32LicenseKey = LicenseChecker.SetNumber (base32LicenseKey, num);
dumpBytes ("base32Key",base32LicenseKey);
				string licenseKey = LicenseChecker.BytesToBase32String (base32LicenseKey);
				return licenseKey;
			}
		}


		static byte[] SetNumber(byte[] bytes, int num) {
			byte[] result = new byte[bytes.Length + 1];
			int i = bytes.Length;
			int u, m, l;
			u = b2i (bytes [i - 1]) << 4;
			result [i] = (byte)(u & 0xFF);
			for (i--; i > 29; i--) {
				u = b2i (bytes [i - 1]) << 4;
				l = b2i (bytes [i]    ) >> 4;
				result [i] = (byte)((u + l) & 0xFF);
				//dumpBytes ("result", result);
			}
			u = b2i (bytes [i - 1]) << 5;
			m = ((num & 2) << 3);
			l = b2i (bytes [i]    ) >> 4;
			result [i] = (byte)((u + m + l) & 0xFF);
			//dumpBytes ("result", result);
			for (i--; i > 25; i--) {
				u = b2i (bytes [i - 1]) << 5;
				l = b2i (bytes [i]    ) >> 3;
				result [i] = (byte)((u + l) & 0xFF);
			}
			//dumpBytes ("result", result);
			u = ((num & 8) << 4);
			m = (b2i (bytes [i - 1]) << 5) & 96;
			l = (b2i (bytes [i]    ) >> 3);
			result [i] = (byte)((u + m + l) & 0xFF); 
			//dumpBytes ("result", result);
			for (i--; i > 13; i--) {
				u = b2i (bytes [i - 1]) << 6;
				l = b2i (bytes [i]    ) >> 2;
				result [i] = (byte)((u + l) & 0xFF);
			}
			//dumpBytes ("result", result);
			u = (b2i (bytes [i - 1]) << 7);
			m = (b2i (bytes [i]    ) >> 1) & 112;
			l = (b2i (bytes [i]    ) >> 2) & 7;
			result [i] = (byte)((u + m + ((num & 1) << 3) + l) & 0xFF);
			//dumpBytes ("result", result);
			for (i--; i > 6; i--) {
				u = b2i (bytes [i - 1]) << 7;
				l = b2i (bytes [i]    ) >> 1;
				result [i] = (byte)((u + l) & 0xFF);
			}
			//dumpBytes ("result", result);
			u = (b2i (bytes [i])     ) & 252;
			m = ((num & 4) >> 1);
			l = (b2i (bytes [i]) >> 1) & 1;
			result [i] = (byte)((u + m + l) & 0xFF);
			//dumpBytes ("result", result);
			for (i--; i >= 0; i--)
				result [i] = bytes [i];
			//dumpBytes ("result", result);
			return result;
		}


		static string BytesToBase32String(byte[] bytes) {
			int len = 64;
			var text = new char[len];
			int bitOffset = 0;
			int index;
			for (int i = 0; i < len; i++) {
				if ((i + 1) % 5 == 0) {
					text [i] = '-';
				} else {
					int first3BitsOfOffset = bitOffset & 7;
					int restOfBitsOfOffset = bitOffset >> 3;
					if (first3BitsOfOffset < 4) 
						index = (b2i(bytes [restOfBitsOfOffset]) >> 3 - first3BitsOfOffset) & 31;
					else 
						index = ((b2i(bytes [restOfBitsOfOffset]) << first3BitsOfOffset - 3) + (b2i(bytes [restOfBitsOfOffset + 1]) >> 11 - first3BitsOfOffset)) & 31;
					var value = LicenseChecker.Base32Chars [index];
					text [i] = value;
					bitOffset += 5;
				}
			}
			var result = new string (text);
			return result;
		}
		#endif

	}
}
