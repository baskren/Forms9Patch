using System;

using Xamarin.Forms;
using P42.Utils;
using System.Linq;
using System.Collections.Generic;
//using Forms9Patch;
using System.Reflection;
using System.Diagnostics;
using System.IO;
//using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ClipboardTest : Xamarin.Forms.ContentPage
    {
        #region Fields
        const int seed = 3452;
        readonly static Random _rand = new Random(seed);

        const string _textPlain = "This is text/plain: ⅓ ⅔ ⓪ ① ② ③ ④ ⑤ ⑥ ⑦ ⑧ ⑨ ⑩ ⑪ ⑫ ⑬ ⑭ ⑮ ⑯ ⑰ ⑱ ⑲ ⑳  🅰 🅱 🅲 🅳 🅴 🅵 🅶 🅷 🅸 🅹 🅺 🅻 🅼 🅽 🅾 🅿 🆀 🆁 🆂 🆃 🆄 🆅 🆆 🆇 🆈 🆉 🙃 😐 😑 🤔 🙄 😮 😔 😖 😕";
        const string _htmlText = "<h1>HTML Ipsum Presents</h1> <p><strong>Pellentesque habitant morbi tristique</strong> senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. <em>Aenean ultricies mi vitae est.</em> Mauris placerat eleifend leo. Quisque sit amet est et sapien ullamcorper pharetra. Vestibulum erat wisi, condimentum sed, <code>commodo vitae</code>, ornare sit amet, wisi. Aenean fermentum, elit eget tincidunt condimentum, eros ipsum rutrum orci, sagittis tempus lacus enim ac dui. <a href=\"#\">Donec non enim</a> in turpis pulvinar facilisis. Ut felis.</p> <h2>Header Level 2</h2> <ol> <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li> <li>Aliquam tincidunt mauris eu risus.</li> </ol> <blockquote><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus magna. Cras in mi at felis aliquet congue. Ut a est eget ligula molestie gravida. Curabitur massa. Donec eleifend, libero at sagittis mollis, tellus est malesuada tellus, at luctus turpis elit sit amet quam. Vivamus pretium ornare est.</p></blockquote> <h3>Header Level 3</h3> <ul> <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li> <li>Aliquam tincidunt mauris eu risus.</li> </ul> <pre><code> #header h1 a { display: block; width: 300px; height: 80px; } </code></pre>";
        const string _htmlBlockQuote = "<blockquote> Gregor then turned to look out the window at the dull weather.Drops of rain could be heard hitting the pane, which made him feel quite sad. \"How about if I sleep a little bit longer and forget all this nonsense\", he thought, but that was something he was unable to do because he was used to sleeping on his right, and in his present state couldn't get into that position. However hard he threw himself onto his right, he always rolled back to where he was. </blockquote>";
        const string _htmlForm = "<form action=\"#\" method=\"post\"> <fieldset> <label for=\"name\">Name:</label> <input type=\"text\" id=\"name\" placeholder=\"Enter your full name\" /> <label for=\"email\">Email:</label> <input type=\"email\" id=\"email\" placeholder=\"Enter your email address\" /> <label for=\"message\">Message:</label> <textarea id=\"message\" placeholder=\"What's on your mind?\"></textarea> <input type=\"submit\" value=\"Send message\" /> </fieldset> </form> ";
        const string _htmlTable = "<table class=\"data\"> <tr> <th>Entry Header 1</th> <th>Entry Header 2</th> <th>Entry Header 3</th> <th>Entry Header 4</th> </tr> <tr> <td>Entry First Line 1</td> <td>Entry First Line 2</td> <td>Entry First Line 3</td> <td>Entry First Line 4</td> </tr> <tr> <td>Entry Line 1</td> <td>Entry Line 2</td> <td>Entry Line 3</td> <td>Entry Line 4</td> </tr> <tr> <td>Entry Last Line 1</td> <td>Entry Last Line 2</td> <td>Entry Last Line 3</td> <td>Entry Last Line 4</td> </tr> </table> ";
        const string _htmlList = "<ul> <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.</li> <li>Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.</li> <li>Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.</li> <li>In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt.</li> </ul> ";
        const string _htmlSmallEmbeddedImage = "<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAM0AAADNCAMAAAAsYgRbAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAABJQTFRF3NSmzMewPxIG//ncJEJsldTou1jHgAAAARBJREFUeNrs2EEKgCAQBVDLuv+V20dENbMY831wKz4Y/VHb/5RGQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0PzMWtyaGhoaGhoaGhoaGhoaGhoxtb0QGhoaGhoaGhoaGhoaGhoaMbRLEvv50VTQ9OTQ5OpyZ01GpM2g0bfmDQaL7S+ofFC6xv3ZpxJiywakzbvd9r3RWPS9I2+MWk0+kbf0Hih9Y17U0nTHibrDDQ0NDQ0NDQ0NDQ0NDQ0NTXbRSL/AK72o6GhoaGhoRlL8951vwsNDQ0NDQ1NDc0WyHtDTEhDQ0NDQ0NTS5MdGhoaGhoaGhoaGhoaGhoaGhoaGhoaGposzSHAAErMwwQ2HwRQAAAAAElFTkSuQmCC\" alt=\"beastie.png\" scale=\"0\">";

        #endregion


        #region Tests
        readonly TestElement _byteTest = new TestElement("byte test ", (entry) =>
        {
            var testByteArray = new byte[200];
            _rand.NextBytes(testByteArray);
            entry.AddValue("application/x-forms9patchdemo-byte", testByteArray[0]);
            return testByteArray[0];
        }, (object obj) =>
        {
            var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-byte");
            if (resultMimeItem == null)
                return false;
            var resultByte = resultMimeItem.Value;
            return obj.Equals(resultByte);
        });
        readonly TestElement _byteArrayTest = new TestElement("byte[] test ", (entry) =>
        {
            var testByteArray = new byte[200];
            _rand.NextBytes(testByteArray);
            entry.AddValue("application/x-forms9patchdemo-bytebuffer", testByteArray);
            return testByteArray;
        }, (object obj) =>
        {
            var resultByteArray = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-bytebuffer")?.Value;
            if (obj is byte[] testByteArray)
            {
                if (resultByteArray != null && resultByteArray.Count() != testByteArray.Count())
                    throw new Exception("byte array counts don't match");
                for (int i = 0; i < resultByteArray.Count(); i++)
                    if (resultByteArray[i] != testByteArray[i])
                        return false;
                return true;
            }
            return false;
        });
        readonly TestElement _charTest = new TestElement("char test ", (entry) =>
        {
            var testChar = (char)_rand.Next(255);
            entry.AddValue("application/x-forms9patchdemo-char", testChar);
            return testChar;
        }, (object obj) =>
        {
            if (obj is char testChar)
            {
                var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<char>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-char");
                if (resultMimeItem == null)
                    return false;
                return testChar == resultMimeItem.Value;
            }
            return false;
        });
        readonly TestElement _shortTest = new TestElement("short test ", (entry) =>
        {
            var testShort = (short)_rand.Next(255);
            entry.AddValue("application/x-forms9patchdemo-short", testShort);
            return testShort;
        }, (object obj) =>
        {
            if (obj is short testShort)
            {
                var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<short>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-short");
                if (resultMimeItem == null)
                    return false;
                return testShort == resultMimeItem.Value;
            }
            return false;
        });
        readonly TestElement _intTest = new TestElement("int test ", (entry) =>
        {
            var testInt = _rand.Next();
            entry.AddValue("application/x-forms9patchdemo-int", testInt);
            return testInt;
        }, (object obj) =>
        {
            if (obj is int testInt)
            {
                var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<int>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-int");
                if (resultMimeItem?.Value == null)
                    return false;
                return testInt == resultMimeItem.Value;
            }
            return false;
        });
        readonly TestElement _longTest = new TestElement("long test ", (entry) =>
        {
            var testLong = (long)_rand.Next() + (long)int.MaxValue;
            entry.AddValue("application/x-forms9patchdemo-long", testLong);
            return testLong;
        }, (object obj) =>
        {
            if (obj is long testLong)
            {
                var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<long>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-long");
                if (resultMimeItem?.Value == null)
                    return false;
                return testLong == resultMimeItem.Value;
            }
            return false;
        });
        readonly TestElement _doubleTest = new TestElement("double test ", (entry) =>
        {
            var testDouble = _rand.NextDouble();
            entry.AddValue("application/x-forms9patchdemo-double", testDouble);
            return testDouble;
        }, (object obj) =>
        {
            if (obj is double testDouble)
            {
                var resultMimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<double>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-double");
                if (resultMimeItem?.Value == null)
                    return false;
                return Math.Abs(testDouble - resultMimeItem.Value) < 0.00001;
            }
            return false;
        });
        readonly TestElement _stringTest = new TestElement("string test ", (entry) =>
        {
            entry.AddValue("text/plain", _textPlain);
            return _textPlain;
        }, (object obj) =>
        {
            if (obj is string testString)
            {
                //var resultString = Forms9Patch.IClipboardEntryExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-string")?.Value as string;
                //if (testString != resultString)
                //    return false;
                var resultString = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/plain")?.Value as string;
                return testString == resultString;
            }
            return false;
        });
        readonly TestElement _intListTest = new TestElement("List<int> test ", (entry) =>
        {
            var testIntList = new List<int>();
            for (int i = 0; i < 20; i++)
                testIntList.Add(i);
            entry.AddValue("application/x-forms9patchdemo-int-list", testIntList);
            return testIntList;
        }, (object obj) =>
        {
            if (obj is List<int> testIntList)
            {
                if (!(Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<List<int>>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-int-list")?.Value is List<int> resultIntList) 
                    || resultIntList.Count != testIntList.Count)
                    return false;
                for (int i = 0; i < resultIntList.Count; i++)
                    if (resultIntList[i] != testIntList[i])
                        return false;
                return true;
            }
            return false;
        });
        readonly TestElement _doubleListTest = new TestElement("List<double> test ", (entry) =>
        {
            var testDoubleList = new List<double>();
            for (int i = 0; i < 20; i++)
                testDoubleList.Add(i + i / 10.0);
            entry.AddValue("application/x-forms9patchdemo-double-list", testDoubleList);
            return testDoubleList;
        }, (object obj) =>
        {
            if (obj is List<double> testDoubleList)
            {
                if (!(Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<List<double>>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-double-list")?.Value is List<double> resulDoubleList) || resulDoubleList.Count != testDoubleList.Count)
                    return false;
                for (int i = 0; i < resulDoubleList.Count; i++)
                    if (Math.Abs(resulDoubleList[i] - testDoubleList[i]) > 0.00001)
                        return false;
                return true;
            }
            return false;
        });
        readonly TestElement _stringListTest = new TestElement("List<string> test ", (entry) =>
        {
            var testStringList = new List<string>();
            for (int i = 0; i < 20; i++)
                testStringList.Add(RandomString(10));
            entry.AddValue("application/x-forms9patchdemo-string-list", testStringList);
            return testStringList;
        }, (object obj) =>
        {
            if (obj is List<string> testStringList)
            {
                if (!(Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<List<string>>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-string-list")?.Value is List<string> resultStringList) || resultStringList.Count != testStringList.Count)
                    return false;
                for (int i = 0; i < resultStringList.Count; i++)
                    if (resultStringList[i] != testStringList[i])
                        return false;
                return true;
            }
            return false;
        });
        readonly TestElement _dictionaryTest = new TestElement("Dictionary<string,double> test ", (entry) =>
        {
            var testDictionary = new Dictionary<string, double>();
            for (int i = 0; i < 20; i++)
                testDictionary.Add(RandomString(10), i + i / 10.0);
            entry.AddValue("application/x-forms9patchdemo-dictionary", testDictionary);

            return testDictionary;
        }, (object obj) =>
        {
            if (obj is Dictionary<string, double> testDictionary)
            {
                if (!(Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<Dictionary<string, double>>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-dictionary")?.Value is Dictionary<string, double> resultDictionary) || resultDictionary.Keys.Count != testDictionary.Keys.Count)
                    return false;
                foreach (var key in testDictionary.Keys)
                {
                    if (!resultDictionary.Keys.Contains(key))
                        return false;
                    if (Math.Abs(resultDictionary[key] - testDictionary[key]) > 0.0001)
                        return false;
                }
                return true;
            }
            return false;
        });
        readonly TestElement _dictionaryListTest = new TestElement("List<Dictionary<string,double>> test ", (entry) =>
        {
            var keys = new List<string>();
            for (int i = 0; i < 20; i++)
                keys.Add(RandomString(10));
            var testDictionaryList = new List<Dictionary<string, double>>();
            for (int i = 0; i < 20; i++)
            {
                var dictionary = new Dictionary<string, double>();
                foreach (var key in keys)
                    dictionary.Add(key, _rand.NextDouble());
                testDictionaryList.Add(dictionary);
            }
            entry.AddValue("application/x-forms9patchdemo-dictionaryList", testDictionaryList);
            return testDictionaryList;
        }, (object obj) =>
        {
            if (obj is List<Dictionary<string, double>> testDictionaryList)
            {
                if (!(Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<List<Dictionary<string, double>>>(Forms9Patch.Clipboard.Entry, "application/x-forms9patchdemo-dictionaryList")?.Value is List<Dictionary<string, double>> resultDictionaryList) || resultDictionaryList.Count != testDictionaryList.Count)
                    return false;
                for (int i = 0; i < testDictionaryList.Count; i++)
                {
                    var tDictionary = testDictionaryList[i];
                    var rDictionary = resultDictionaryList[i];
                    if (tDictionary.Count != rDictionary.Count)
                        return false;
                    var tKeys = tDictionary.Keys;
                    foreach (var key in tKeys)
                    {
                        if (!rDictionary.Keys.Contains(key))
                            return false;
                        if (rDictionary[key] != tDictionary[key])
                            return false;
                    }
                }
                return true;
            }
            return false;
        });
        readonly TestElement _dateTimeTest = new TestElement("DateTime as JSON", (entry) =>
        {
            // anything more complex than the ClipboardEntry.ValidItemType() types should be serialized (string, byte[], or uri) and stored that way. 
            var dateTime = DateTime.Now;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dateTime);
            entry.AddValue("application/json", json);
            return json;
        }, (obj) =>
        {
            if (obj is string testJson)
            {
                var dateTimeResultJson = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "application/json")?.Value as string;
                var resultDateTime = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(dateTimeResultJson);
                return testJson == dateTimeResultJson;
            }
            return false;
        });

        readonly TestElement _jpegByteArrayTest = new TestElement("jpeg from byte[] test", (entry) =>
        {
            // anything more complex than the ClipboardEntry.ValidItemType() types should be serialized (string, byte[], or uri) and stored that way. 
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons1.jpg");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "balloons1.jpg");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.balloons1.jpg", path);
            if (!File.Exists(path))
                throw new Exception("EmbeddedResource (balloons.jpg) was not extracted to file");
            var testByteArray = File.ReadAllBytes(path);
            _testImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(testByteArray));
            entry.AddValue("image/jpeg", testByteArray);
            return testByteArray;
        }, (object obj) =>
        {
            if (obj is byte[] testByteArray)
            {
                var mimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "image/jpeg");
                var mimeResult = mimeItem?.Value;
                _resultImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(mimeResult));
                if (testByteArray.Length == mimeResult.Length)
                    return NewMemCmp(testByteArray, mimeResult, testByteArray.Length);
                return false;
            }
            return false;
        });

        readonly TestElement _jpegFileInfoTest = new TestElement("jpeg from FileInfo test", (entry) =>
        {
            // anything more complex than the ClipboardEntry.ValidItemType() types should be serialized (string, byte[], or uri) and stored that way. 
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons2.jpg");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "balloons2.jpg");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.balloons2.jpg", path);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new Exception("EmbeddedResource (balloons.jpg) was not extracted to file");
            entry.AddValue("image/jpeg", fileInfo);
            _testImage.Source = Xamarin.Forms.ImageSource.FromFile(fileInfo.FullName);
            return path;
        }, (object obj) =>
        {
            if (obj is string testPath)
            {
                // FileInfo will be passed into the clipboard as FileInfo objects but will returned as byte[].
                var testByteArray = File.ReadAllBytes(testPath);

                var mimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "image/jpeg");
                var mimeResult = mimeItem?.Value;
                _resultImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(mimeResult));
                if (testByteArray.Length == mimeResult.Length)
                    return NewMemCmp(testByteArray, mimeResult, testByteArray.Length);
                return false;
            }
            return false;
        });


        readonly TestElement _pngByteArrayTest = new TestElement("png byte[] test", (entry) =>
        {
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "236-baby.png");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "236-baby.png");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.236-baby.png", path);
            if (!File.Exists(path))
                throw new Exception("EmbeddedResource (236-baby.png) was not extracted to file");
            var result = Forms9Patch.IMimeItemCollectionExtensions.AddBytesFromFile(entry, "image/png", path);
            _testImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(result));
            return result;
        }, (object obj) =>
        {
            if (obj is byte[] testByteArray)
            {
                var mimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "image/png");
                var mimeResult = mimeItem?.Value;
                _resultImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(mimeResult));
                if (testByteArray.Length == mimeResult.Length)
                    return NewMemCmp(testByteArray, mimeResult, testByteArray.Length);
                return false;
            }
            return false;
        });

        readonly TestElement _pdfByteArrayTest = new TestElement("pdf byte[] test", (entry) =>
        {
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ProjectProposal.pdf");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "ProjectProposal.pdf");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.ProjectProposal.pdf", path);
            if (!File.Exists(path))
                throw new Exception("EmbeddedResource (ProjectProposal.pdf) was not extracted to file");
            return Forms9Patch.IMimeItemCollectionExtensions.AddBytesFromFile(entry, "application/pdf", path);
        }, (object obj) =>
        {
            if (obj is byte[] testByteArray)
            {
                var mimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "application/pdf");
                var resultByteArray = mimeItem?.Value;
                if (testByteArray.Length == resultByteArray.Length)
                    return NewMemCmp(testByteArray, resultByteArray, testByteArray.Length);
                return false;
            }
            return false;
        });

        readonly TestElement _pdfFileInfoTest = new TestElement("pdf from FileInfo test", (entry) =>
        {
            // anything more complex than the ClipboardEntry.ValidItemType() types should be serialized (string, byte[], or uri) and stored that way. 
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons2.jpg");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "ProjectProposal.pdf");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.ProjectProposal.pdf", path);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new Exception("EmbeddedResource (ProjectProposal.pdf) was not extracted to file");
            entry.AddValue("application/pdf", fileInfo);
            return File.ReadAllBytes(fileInfo.FullName);
        }, (object obj) =>
        {
            if (obj is byte[] testByteArray)
            {
                var mimeItem = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<byte[]>(Forms9Patch.Clipboard.Entry, "application/pdf");
                var resultByteArray = mimeItem?.Value;

                if (resultByteArray != null && testByteArray.Length == resultByteArray.Length)
                    return NewMemCmp(testByteArray, resultByteArray, testByteArray.Length);
                return false;
            }
            return false;
        });


        readonly TestElement _jpegHttpUrlTest = new TestElement("jpeg http url test", (entry) =>
        {
            var uri = new Uri("https://i.redditmedia.com/npNromwDHMXlxFMa0CAZqw0MMSKFj-aHx5rvgQNPXyA.jpg?fit=crop&crop=faces%2Centropy&arh=2&w=640&s=04fe226f00868a3182265a9af861608e");
            entry.AddValue("image/jpeg", uri.AbsoluteUri);
            _testImage.Source = Xamarin.Forms.ImageSource.FromUri(uri);
            return uri.AbsoluteUri;
        }, (object obj) =>
        {
            if (obj is string testUri)
            {
                //var resultUri = Forms9Patch.Clipboard.Entry.Uri;
                var result = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "image/jpeg");
                if (result?.Value == null)
                    return false;
                var resultUri = new Uri(result.Value);
                _resultImage.Source = Xamarin.Forms.ImageSource.FromUri(resultUri);
                return testUri == result.Value;
            }
            return false;
        });

        readonly TestElement _pdfFileTest = new TestElement("pdf file test", (entry) =>
        {
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ProjectProposal.pdf");
            var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "ProjectProposal.pdf");
            ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.ProjectProposal.pdf", path);
            if (!File.Exists(path))
                throw new Exception("EmbeddedResource (ProjectProposal.pdf) was not extracted to file");
            var url = "file://" + path;
            entry.AddValue("image/jpeg", path);
            _testImage.Source = Xamarin.Forms.ImageSource.FromFile(path);
            return url;
        }, (object obj) =>
        {
            if (obj is string testUrlString)
            {
                /*
                var mimeItem = Forms9Patch.Clipboard.Entry.GetItem<byte[]>("application/pdf");
                var resultByteArray = mimeItem?.Value;
                if (testByteArray.Length == resultByteArray.Length)
                    return NewMemCmp(testByteArray, resultByteArray, testByteArray.Length);
*/
                return false;
            }
            return false;
        });


        readonly TestElement _multipleByteArrayImagesTest = new TestElement("multiple byte[] images test", (entry) =>
        {
            for (int i = 1; i <= 3; i++)
            {
                //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons" + i + ".jpg");
                var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "balloons" + i + ".jpg");
                ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.balloons" + i + ".jpg", path);
                var byteArray = File.ReadAllBytes(path);
                entry.AddValue("image/jpeg", byteArray);
            }
            return null;
        }, (obj) =>
        {
            Forms9Patch.Toast.Create("Copy complete", "Verify results by performing paste into Note or email");
            return false;
        });

        readonly TestElement _multipleFileInfoImagesTest = new TestElement("multiple FileInfo images test", (entry) =>
        {
            for (int i = 1; i <= 3; i++)
            {
                //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons" + i + ".jpg");
                var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "balloons" + i + ".jpg");
                ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.balloons" + i + ".jpg", path);
                //var byteArray = File.ReadAllBytes(path);
                var fileInfo = new FileInfo(path);
                entry.AddValue("image/jpeg", fileInfo);
            }
            return null;
        }, (obj) =>
        {
            Forms9Patch.Toast.Create("Copy complete", "Verify results by performing paste into Note or email");
            return false;
        });


        readonly TestElement _multipleTextTest = new TestElement("multiple text test", (entry) =>
        {
            entry.AddValue("text/plain", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In at tristique neque. Aliquam libero mauris, pulvinar non ligula quis, ultricies ullamcorper nisl. Ut varius fringilla ipsum ut accumsan. Curabitur luctus pulvinar quam eget pharetra. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Morbi tincidunt commodo quam. Phasellus et finibus nisi, quis elementum nisl. Vivamus aliquet lectus vel diam lacinia, vel condimentum eros ultricies. Nulla molestie risus at lectus scelerisque, quis efficitur tortor pulvinar. Aenean sollicitudin justo magna, vitae molestie sapien placerat at. Sed ut mauris auctor, interdum ante ac, porttitor lectus. Morbi iaculis mi a malesuada tincidunt.");
            entry.AddValue("text/plain", "Mauris eu sodales velit. Etiam efficitur gravida neque at mollis. Nunc pellentesque lacus id tortor fringilla consequat. Sed et leo vel urna pulvinar lacinia. Nullam a odio sed urna mollis consequat. Curabitur id tincidunt arcu. Aliquam pulvinar ipsum vehicula lacus faucibus, ut posuere neque porttitor. Etiam consectetur risus sed gravida lobortis. Quisque aliquet nisl lectus, vitae mattis diam mattis sit amet. Donec malesuada lectus leo, vitae viverra diam dictum sed. Cras velit lorem, porta in mattis in, pharetra non odio. Sed consectetur nec tortor a sagittis. Fusce dictum odio sed est commodo sagittis. Morbi accumsan, nisi quis hendrerit consectetur, dolor neque cursus ex, et egestas ipsum enim et purus. Nullam non posuere orci. Proin tincidunt iaculis elit, eu aliquam sapien scelerisque at.");
            entry.AddValue("text/plain", "Vivamus lobortis nibh justo, id porta orci suscipit nec. Donec a purus ut ligula condimentum rutrum. Vestibulum a nisl a augue fringilla venenatis. Duis dignissim, dolor elementum faucibus pellentesque, diam ante porta sapien, in bibendum enim nibh sed magna. Mauris sed mi ac libero facilisis fermentum. Integer feugiat eros vitae nulla euismod, vitae ultricies enim porta. Nullam convallis velit vitae ligula facilisis aliquam. Nunc sagittis, ligula ac interdum laoreet, ipsum dui pellentesque leo, eu pellentesque lectus quam sed lacus. Sed commodo dui eget ultrices commodo.");
            entry.AddValue("text/plain", "Phasellus lacus ligula, rutrum in eros cursus, tempor elementum erat. Nullam vel vehicula ante, ac porttitor ante. In non tempus ex. Maecenas eu ultrices erat, eget porta ipsum. Aenean dictum, elit sed mollis dapibus, nibh magna semper felis, vitae aliquam diam lectus tempus elit. Sed ornare velit non augue rhoncus pretium. Phasellus in justo tellus. Pellentesque molestie enim sed nibh volutpat, nec lacinia dolor luctus. Aenean ac dui enim. Vivamus maximus sem sit amet leo euismod, ac blandit mauris blandit. Donec iaculis dolor interdum orci tincidunt congue. Vivamus ac erat sit amet neque ullamcorper aliquet at vel magna. Mauris auctor blandit est, eu dapibus leo dapibus id.");
            entry.AddValue("text/plain", "Nullam aliquam neque diam, ut pharetra neque tincidunt sed. Praesent rutrum, orci in tincidunt posuere, turpis velit mollis purus, id ullamcorper ipsum diam in augue. Nunc porttitor tempor orci, eget rhoncus magna molestie pharetra. Nam maximus mollis imperdiet. In vitae ante sollicitudin, ultrices lectus ac, vehicula lacus. Proin eget sapien ut magna sollicitudin eleifend. Integer volutpat ipsum vel nibh malesuada euismod. Sed scelerisque vulputate interdum. Proin bibendum vel lectus et semper. Mauris tincidunt euismod ligula non dignissim. Nulla et quam nec nunc bibendum pulvinar quis nec erat. Suspendisse porta, nulla eu tincidunt maximus, leo ex hendrerit mi, sit amet mattis libero nisl sed arcu. Nunc ac congue ante. Fusce a mollis erat.");
            return null;
        }, (obj) =>
        {
            Forms9Patch.Toast.Create("Copy complete", "Verify results by performing paste into Note or email");
            return false;
        });

        readonly TestElement _multipleHtmlTest = new TestElement("multiple HTML fragments test", (entry) =>
        {
            entry.AddValue("text/html", _htmlForm);
            entry.AddValue("text/html", _htmlList);
            entry.AddValue("text/html", _htmlTable);
            entry.AddValue("text/html", _htmlBlockQuote);
            entry.AddValue("text/html", _htmlSmallEmbeddedImage);
            return null;
        }, (obj) =>
         {
             Forms9Patch.Toast.Create("Copy complete", "Verify results by performing paste into Note or email");
             return false;
         });

        readonly TestElement _htmlStringTest = new TestElement("html test", (entry) =>
        {
            // Note: unlike images and text, multiple html items does not work with iOS apps (Notes and mail).
            //entry.AddValue("text/html", "<p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. Mauris placerat eleifend leo.</p>");
            //entry.AddValue("text/html", "<dl> <dt>Definition list</dt> <dd>Consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</dd> <dt>Lorem ipsum dolor sit amet</dt> <dd>Consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</dd> </dl>");
            //entry.AddValue("text/html", "<ul> <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li> <li>Aliquam tincidunt mauris eu risus.</li> <li>Vestibulum auctor dapibus neque.</li> </ul>");
            //entry.AddValue("text/html", "<ul> <li>Morbi in sem quis dui placerat ornare. Pellentesque odio nisi, euismod in, pharetra a, ultricies in, diam. Sed arcu. Cras consequat.</li> <li>Praesent dapibus, neque id cursus faucibus, tortor neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, facilisis luctus, metus.</li> <li>Phasellus ultrices nulla quis nibh. Quisque a lectus. Donec consectetuer ligula vulputate sem tristique cursus. Nam nulla quam, gravida non, commodo a, sodales sit amet, nisi.</li> <li>Pellentesque fermentum dolor. Aliquam quam lectus, facilisis auctor, ultrices ut, elementum vulputate, nunc.</li> </ul> ");
            entry.AddValue("text/html", _htmlText);
            return _htmlText;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });

        readonly TestElement _htmlFormTest = new TestElement("html Form test", (entry) =>
        {
            entry.AddValue("text/html", _htmlForm);
            return _htmlForm;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });

        readonly TestElement _htmlListTest = new TestElement("html List test", (entry) =>
        {
            entry.AddValue("text/html", _htmlList);
            return _htmlList;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });

        readonly TestElement _htmlTableTest = new TestElement("html Table test", (entry) =>
        {
            entry.AddValue("text/html", _htmlTable);
            return _htmlTable;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });

        readonly TestElement _htmlBlockQuoteTest = new TestElement("html Block Quote test", (entry) =>
        {
            entry.AddValue("text/html", _htmlBlockQuote);
            return _htmlBlockQuote;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });

        readonly TestElement _htmlEmbedImageTest = new TestElement("html Embedded Image test", (entry) =>
        {
            entry.AddValue("text/html", _htmlSmallEmbeddedImage);
            return _htmlSmallEmbeddedImage;
        }, (obj) =>
        {
            if (obj is string testHtml)
            {
                var resultHtml = Forms9Patch.IMimeItemCollectionExtensions.GetFirstMimeItem<string>(Forms9Patch.Clipboard.Entry, "text/html")?.Value as string;
                return testHtml == resultHtml;
            }
            return false;
        });


        readonly TestElement _mixedContentTest = new TestElement("mixed content test", (entry) =>
        {
            entry.AddValue("text/plain", _textPlain);
            entry.AddValue("text/html", _htmlText);
            for (int i = 1; i <= 3; i++)
            {
                //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "balloons" + i + ".jpg");
                var path = Path.Combine(P42.Utils.Environment.ApplicationDataPath, "balloons" + i + ".jpg");
                ExtractEmbeddedResourceToPath(typeof(ClipboardTest).GetTypeInfo().Assembly, "Forms9PatchDemo.Resources.balloons" + i + ".jpg", path);
                //var byteArray = File.ReadAllBytes(path);
                var fileInfo = new FileInfo(path);
                entry.AddValue("image/jpeg", fileInfo);
            }
            return null;
        }, (obj) =>
        {
            Forms9Patch.Toast.Create("Copy complete", "Verify results by performing paste into Note or email");
            return false;
        });

        #endregion


        #region other visual elements
        readonly Xamarin.Forms.Label _availableMimeTypesLabel = new Xamarin.Forms.Label();

        readonly Xamarin.Forms.Label _elapsedTimeLabel = new Xamarin.Forms.Label();

        readonly Xamarin.Forms.StackLayout _layout = new Xamarin.Forms.StackLayout
        {
            Children = { new Forms9Patch.Label("<b>Copy / Paste tests:</b>") }
        };

        readonly Xamarin.Forms.Switch _entryCaching = new Xamarin.Forms.Switch { HorizontalOptions = LayoutOptions.End };
        //Switch _entryItemTypeCaching = new Switch { HorizontalOptions = LayoutOptions.End };
        readonly Xamarin.Forms.Button _execute = new Xamarin.Forms.Button
        {
            Text = "run test"
        };

        readonly Forms9Patch.Toast _clipboardChangedToast = new Forms9Patch.Toast { Title = "CLIPBOARD CHANGED", Text = "The clipboard has changed." };

        readonly static Xamarin.Forms.Image _testImage = new Xamarin.Forms.Image
        {
            Aspect = Aspect.AspectFit,
        };

        readonly static Xamarin.Forms.Image _resultImage = new Xamarin.Forms.Image
        {
            Aspect = Aspect.AspectFit,
        };

        readonly Xamarin.Forms.Grid _jpegComparisonGrid = new Xamarin.Forms.Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition{ Width = GridLength.Star },
                new ColumnDefinition{ Width = GridLength.Star },
            },
        };


        #endregion


        public ClipboardTest()
        {
            //Padding = new Thickness(20, Device.RuntimePlatform == Device.iOS ? 40 : 20, 20, 0);

            _entryCaching.IsToggled = Forms9Patch.Clipboard.EntryCaching;

            _entryCaching.Toggled += (sender, e) => Forms9Patch.Clipboard.EntryCaching = _entryCaching.IsToggled;

            _jpegComparisonGrid.Children.Add(_testImage);
            _jpegComparisonGrid.Children.Add(_resultImage, 1, 0);


            _layout.Children.Add(new Xamarin.Forms.Label { Text = "Available Mime Types: " });
            _layout.Children.Add(_availableMimeTypesLabel);
            _layout.Children.Add(new BoxView { Color = Color.Black, HeightRequest = 1 });
            _layout.Children.Add(_byteTest);
            _layout.Children.Add(_byteArrayTest);
            _layout.Children.Add(_shortTest);
            _layout.Children.Add(_intTest);
            _layout.Children.Add(_longTest);
            _layout.Children.Add(_doubleTest);
            _layout.Children.Add(_stringTest);
            _layout.Children.Add(_htmlStringTest);

            _layout.Children.Add(_htmlFormTest);
            _layout.Children.Add(_htmlListTest);
            _layout.Children.Add(_htmlTableTest);
            _layout.Children.Add(_htmlBlockQuoteTest);
            _layout.Children.Add(_htmlEmbedImageTest);

            _layout.Children.Add(_intListTest);
            _layout.Children.Add(_doubleListTest);
            _layout.Children.Add(_stringListTest);
            _layout.Children.Add(_dictionaryTest);
            _layout.Children.Add(_dictionaryListTest);
            _layout.Children.Add(_dateTimeTest);
            _layout.Children.Add(_pdfByteArrayTest);
            _layout.Children.Add(_pdfFileInfoTest);

            _layout.Children.Add(new BoxView { Color = Color.Blue, HeightRequest = 5 });
            _layout.Children.Add(new Label { Text = "Must visually check images because results will not be the same, byte for byte" });
            _layout.Children.Add(new BoxView { Color = Color.Blue, HeightRequest = 5 });

            _layout.Children.Add(_pngByteArrayTest);
            _layout.Children.Add(_jpegByteArrayTest);
            _layout.Children.Add(_jpegFileInfoTest);
            _layout.Children.Add(_jpegHttpUrlTest);
            _layout.Children.Add(_jpegComparisonGrid);
            _layout.Children.Add(new BoxView { Color = Color.Blue, HeightRequest = 5 });
            _layout.Children.Add(new Forms9Patch.Label("Entry caching:"));
            _layout.Children.Add(_entryCaching);
            _layout.Children.Add(_execute);
            _layout.Children.Add(new BoxView { Color = Color.Blue, HeightRequest = 5 });
            _layout.Children.Add(_multipleByteArrayImagesTest);
            _layout.Children.Add(_multipleFileInfoImagesTest);
            _layout.Children.Add(_multipleTextTest);
            _layout.Children.Add(_multipleHtmlTest);
            _layout.Children.Add(_mixedContentTest);
            _layout.Children.Add(_elapsedTimeLabel);


            Content = new ScrollView
            {
                Content = _layout
            };

            _execute.Clicked += (sender, e) => CopyPaste();

            System.Diagnostics.Debug.WriteLine("ClipboardTest Constructor 1");
            UpdateAvailableMimeTypesLabel();
            System.Diagnostics.Debug.WriteLine("ClipboardTest Constructor 2");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Keep in mind, event handlers can be zombies.  Think about proper disposal (or squelching) if you want to be sure it doesn't appear after you leave the page/view where you instantiated it.
            Forms9Patch.Clipboard.ContentChanged += Clipboard_ContentChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Forms9Patch.Clipboard.ContentChanged -= Clipboard_ContentChanged;
        }

        void Clipboard_ContentChanged(object sender, EventArgs e)
        {
            _clipboardChangedToast.IsVisible = true;
            UpdateAvailableMimeTypesLabel();
        }

        void UpdateAvailableMimeTypesLabel()
        {
            var items = Forms9Patch.Clipboard.Entry?.Items;
            if (items != null && items.Count > 0)
            {
                var mimeTypes = items.Select((mimeEntry) => mimeEntry.MimeType);
                _availableMimeTypesLabel.Text = string.Join(", ", mimeTypes);
            }
            else
                _availableMimeTypesLabel.Text = null;
        }


        bool CopyPaste()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var entry = new Forms9Patch.MimeItemCollection();
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 1 elapsed: " + stopwatch.ElapsedMilliseconds);


            var testByte = (byte)_byteTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.01 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testByteArray = (byte[])_byteArrayTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.02 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testChar = (char)_charTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.03 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testShort = (short)_shortTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.04 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testInt = (int)_intTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.05 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testLong = (long)_longTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.06 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testDouble = (double)_doubleTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.07 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testString = (string)_stringTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.08 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testHtml = (string)_htmlStringTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.09 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testIntList = (List<int>)_intListTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.10 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testDoubleList = (List<double>)_doubleListTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.11 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testStringList = (List<string>)_stringListTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.12 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testDictionary = (Dictionary<string, double>)_dictionaryTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.13 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testDictionaryList = (List<Dictionary<string, double>>)_dictionaryListTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.14 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testDateTimeJson = (string)_dateTimeTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.15 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testByteArrayPdf = (byte[])_pdfByteArrayTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.16 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testFileInfoPdf = (byte[])_pdfByteArrayTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.17 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testJpegByteArray = (byte[])_jpegByteArrayTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.18 elapsed: " + stopwatch.ElapsedMilliseconds);
            var testPngByteArray = (byte[])_pngByteArrayTest.CopyAction.Invoke(entry);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 2.19 elapsed: " + stopwatch.ElapsedMilliseconds);

            Forms9Patch.Clipboard.Entry = entry;
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 3 elapsed: " + stopwatch.ElapsedMilliseconds);

            _byteTest.Success = _byteTest.PasteFunction(testByte);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.01 elapsed: " + stopwatch.ElapsedMilliseconds);
            _byteArrayTest.Success = _byteArrayTest.PasteFunction(testByteArray);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.02 elapsed: " + stopwatch.ElapsedMilliseconds);
            _charTest.Success = _charTest.PasteFunction(testChar);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.03 elapsed: " + stopwatch.ElapsedMilliseconds);
            _shortTest.Success = _shortTest.PasteFunction(testShort);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.04 elapsed: " + stopwatch.ElapsedMilliseconds);
            _intTest.Success = _intTest.PasteFunction(testInt);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.05 elapsed: " + stopwatch.ElapsedMilliseconds);
            _longTest.Success = _longTest.PasteFunction(testLong);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.06 elapsed: " + stopwatch.ElapsedMilliseconds);
            _doubleTest.Success = _doubleTest.PasteFunction(testDouble);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.07 elapsed: " + stopwatch.ElapsedMilliseconds);
            _stringTest.Success = _stringTest.PasteFunction(testString);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.08 elapsed: " + stopwatch.ElapsedMilliseconds);
            _htmlStringTest.Success = _htmlStringTest.PasteFunction(testHtml);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.09 elapsed: " + stopwatch.ElapsedMilliseconds);
            _intListTest.Success = _intListTest.PasteFunction(testIntList);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.10 elapsed: " + stopwatch.ElapsedMilliseconds);
            _doubleListTest.Success = _doubleListTest.PasteFunction(testDoubleList);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.11 elapsed: " + stopwatch.ElapsedMilliseconds);
            _stringListTest.Success = _stringListTest.PasteFunction(testStringList);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.12 elapsed: " + stopwatch.ElapsedMilliseconds);
            _dictionaryTest.Success = _dictionaryTest.PasteFunction(testDictionary);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.13 elapsed: " + stopwatch.ElapsedMilliseconds);
            _dictionaryListTest.Success = _dictionaryListTest.PasteFunction(testDictionaryList);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.14 elapsed: " + stopwatch.ElapsedMilliseconds);
            _dateTimeTest.Success = _dateTimeTest.PasteFunction(testDateTimeJson);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.15 elapsed: " + stopwatch.ElapsedMilliseconds);
            _pdfByteArrayTest.Success = _pdfByteArrayTest.PasteFunction(testByteArrayPdf);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.16 elapsed: " + stopwatch.ElapsedMilliseconds);
            _pdfFileInfoTest.Success = _pdfFileInfoTest.PasteFunction(testFileInfoPdf);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.16 elapsed: " + stopwatch.ElapsedMilliseconds);
            _jpegByteArrayTest.Success = _jpegByteArrayTest.PasteFunction(testJpegByteArray);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.17 elapsed: " + stopwatch.ElapsedMilliseconds);
            _pngByteArrayTest.Success = _pngByteArrayTest.PasteFunction(testPngByteArray);
            System.Diagnostics.Debug.WriteLine("\t CopyPaste 4.18 elapsed: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Stop();
            _elapsedTimeLabel.Text = "Elapsed time: " + stopwatch.ElapsedMilliseconds + "ms";
            return true;
        }

        static string RandomString(int length)
        {
            const string chars = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«⅓⅔⓪①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_rand.Next(s.Length)]).ToArray());
        }

        static void ExtractEmbeddedResourceToPath(Assembly assembly, String embeddedResourceId, String path)
        {
            Stream resFilestream = assembly.GetManifestResourceStream(embeddedResourceId);
            if (resFilestream != null)
            {
                System.Diagnostics.Debug.WriteLine("embedded resource length: " + resFilestream.Length);
                using (BinaryReader br = new BinaryReader(resFilestream))
                {
                    if (br != null)
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            if (fs != null)
                            {
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {
                                    if (bw != null)
                                    {
                                        byte[] ba = new byte[resFilestream.Length];
                                        resFilestream.Read(ba, 0, ba.Length);
                                        bw.Write(ba);
                                        var fileInfo = new System.IO.FileInfo(path);
                                        System.Diagnostics.Debug.WriteLine("output length: " + fileInfo.Length);
                                        System.Diagnostics.Debug.WriteLine("output length: " + fs.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*
        static byte[] ByteArrayFromFile(String path)
        {
            using (FileStream filestream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (filestream != null)
                {
                    System.Diagnostics.Debug.WriteLine("embedded resource length: " + filestream.Length);
                    using (BinaryReader br = new BinaryReader(filestream))
                    {
                        if (br != null)
                        {
                            //using (FileStream fs = new FileStream(path, FileMode.Create))
                            using (MemoryStream ms = new MemoryStream((int)filestream.Length))
                            {
                                if (ms != null)
                                {
                                    using (BinaryWriter bw = new BinaryWriter(ms))
                                    {
                                        if (bw != null)
                                        {
                                            byte[] ba = new byte[filestream.Length];
                                            filestream.Read(ba, 0, ba.Length);
                                            bw.Write(ba);
                                            var fileInfo = new System.IO.FileInfo(path);
                                            System.Diagnostics.Debug.WriteLine("output length: " + fileInfo.Length);
                                            System.Diagnostics.Debug.WriteLine("output length: " + ms.Length);
                                            return ms.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        */

        public static unsafe bool NewMemCmp(byte* b0, byte* b1, int length)
        {
            byte* lastAddr = b0 + length;
            byte* lastAddrMinus32 = lastAddr - 32;
            while (b0 < lastAddrMinus32) // unroll the loop so that we are comparing 32 bytes at a time.
            {
                if (*(ulong*)b0 != *(ulong*)b1) return false;
                if (*(ulong*)(b0 + 8) != *(ulong*)(b1 + 8)) return false;
                if (*(ulong*)(b0 + 16) != *(ulong*)(b1 + 16)) return false;
                if (*(ulong*)(b0 + 24) != *(ulong*)(b1 + 24)) return false;
                b0 += 32;
                b1 += 32;
            }
            while (b0 < lastAddr)
            {
                if (*b0 != *b1) return false;
                b0++;
                b1++;
            }
            return true;
        }

        public static unsafe bool NewMemCmp(byte[] arr0, byte[] arr1, int length)
        {
            fixed (byte* b0 = arr0, b1 = arr1)
            {
                return b0 == b1 || NewMemCmp(b0, b1, length);
            }
        }

        class TestElement : Xamarin.Forms.StackLayout
        {
            public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(TestElement), default(string));
            public string Text
            {
                get { return (string)GetValue(TextProperty); }
                set { SetValue(TextProperty, value); }
            }

            public static readonly BindableProperty SuccessProperty = BindableProperty.Create("Success", typeof(bool), typeof(TestElement), default(bool));
            public bool Success
            {
                get { return (bool)GetValue(SuccessProperty); }
                set { SetValue(SuccessProperty, value); }
            }
            public static readonly BindableProperty TimeProperty = BindableProperty.Create("Time", typeof(long), typeof(TestElement), default(long));
            public long Time
            {
                get { return (long)GetValue(TimeProperty); }
                set { SetValue(TimeProperty, value); }
            }

            public Func<Forms9Patch.MimeItemCollection, object> CopyAction { get; private set; }

            public Func<object, bool> PasteFunction { get; private set; }

            readonly Forms9Patch.Button _shareButton = new Forms9Patch.Button
            {
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.shareIcon.png") { Fill = Forms9Patch.Fill.AspectFit, Margin = new Thickness(5, 0, 5, 0) },
                BackgroundColor = Color.White,
                //OutlineColor = Color.Blue,
                //OutlineWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 30,
                WidthRequest = 50,
            };
            readonly Xamarin.Forms.Button _testButton = new Xamarin.Forms.Button { Text = " Test ", BorderWidth = 1, BorderColor = Color.Blue, HorizontalOptions = LayoutOptions.Start };
            readonly Xamarin.Forms.Label _textLabel = new Xamarin.Forms.Label();
            //readonly Xamarin.Forms.Label _statusLabel = new Xamarin.Forms.Label { Text = "☐" };
            //readonly Forms9Patch.Image _statusLabel = new Forms9Patch.Image { WidthRequest = 20, HeightRequest = 20, Fill = Forms9Patch.Fill.Fill };
            readonly Xamarin.Forms.BoxView _statusLabel = new Xamarin.Forms.BoxView { WidthRequest = 20, HeightRequest = 20, Color = Color.Gray };
            readonly Xamarin.Forms.Label _timeLabel = new Xamarin.Forms.Label { HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };

            public TestElement(string text, Func<Forms9Patch.MimeItemCollection, object> copyAction = null, Func<object, bool> pasteFunction = null)
            {
                //_statusLabel.Source = Forms9Patch.ImageSource.FromResource("Forms9PatchDemo.Resources.ballot_box.svg", this.GetType().Assembly);
                _textLabel.Text = text;
                CopyAction = copyAction;
                PasteFunction = pasteFunction;
                Orientation = StackOrientation.Horizontal;
                if (CopyAction != null && PasteFunction != null)
                {
                    Children.Add(_shareButton);
                    _shareButton.Clicked += (s, e) =>
                    {
                        var entry = new Forms9Patch.MimeItemCollection();
                        var obj = CopyAction.Invoke(entry);
                        Forms9Patch.Sharing.Share(entry, _shareButton);
                    };
                    Children.Add(_testButton);
                    _testButton.Clicked += (s, e) =>
                    {
                        var stopWatch = new Stopwatch();
                        stopWatch.Start();
                        var entry = new Forms9Patch.MimeItemCollection();
                        System.Diagnostics.Debug.WriteLine("\t TestElement 1 elapsed: " + stopWatch.ElapsedMilliseconds);
                        var obj = CopyAction.Invoke(entry);
                        System.Diagnostics.Debug.WriteLine("\t TestElement 2 elapsed: " + stopWatch.ElapsedMilliseconds);
                        Forms9Patch.Clipboard.Entry = entry;
                        System.Diagnostics.Debug.WriteLine("\t TestElement 3 elapsed: " + stopWatch.ElapsedMilliseconds);
                        Success = PasteFunction.Invoke(obj);
                        System.Diagnostics.Debug.WriteLine("\t TestElement 4 elapsed: " + stopWatch.ElapsedMilliseconds);
                        stopWatch.Stop();
                        Time = stopWatch.ElapsedMilliseconds;
                    };
                }
                Children.Add(_statusLabel);
                Children.Add(_textLabel);
                Children.Add(_timeLabel);
            }

            protected override void OnPropertyChanged(string propertyName = null)
            {
                base.OnPropertyChanged(propertyName);
                if (propertyName == TextProperty.PropertyName)
                    _textLabel.Text = Text;
                else if (propertyName == SuccessProperty.PropertyName)
                {
                    //_statusLabel.Text = Success ? "☑" : "☒";
                    //_statusLabel.TextColor = Success ? Color.DarkGreen : Color.Red;
                    //var resourceId = Success ? "Forms9PatchDemo.Resources.ballot_box_with_check.svg" : "Forms9PatchDemo.Resources.ballot_box_with_x.svg";
                    //_statusLabel.Source = Forms9Patch.ImageSource.FromResource(resourceId, GetType().Assembly);
                    _statusLabel.Color = Success ? Color.Green : Color.Red;

                }
                else if (propertyName == TimeProperty.PropertyName)
                    _timeLabel.Text = Time.ToString() + "ms";
            }

        }
    }
}
