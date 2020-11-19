using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class WordCroppedDemo : Xamarin.Forms.ContentPage
    {
        readonly ObservableCollection<string> strLst;
        const string longStr_Chi = "現在，解決生活的意義的問題，是非常非常重要的。所以， 那麼， 我們不得不面對一個非常尷尬的事實，X那就是， 現在，解決生活的意義的問題，是非常非常重要的。所以， 問題的關鍵究竟為何？從這個角度來看， 所謂生活的意義，關鍵是生活的意義需要如何寫。.";
        const string longStr_En = "現My bounce behaves. The pragmatic coal 現 floors the compromise next to a set 現 diagonal. 現 The warning dive strays next to an aged 現 lung. The libel yawns 現 opposite the sixteen load. A 現 silicon mills the brick. The sophisticated 現 market degenerates without 現 the mobile arithmetic.";

        public WordCroppedDemo()
        {
            P42.Utils.DebugExtensions.IsMessagesEnabled = true;
            InitializeComponent();

            ChiStrLabel.Text = longStr_Chi;
            EnStrLabel.Text = longStr_En;

            strLst = new ObservableCollection<string>
            {
                longStr_Chi,
                longStr_En
            };

            ChiStrLabel.SizeChanged += (s, e) => ChiStrSize.Text = ChiStrLabel.Bounds.Size.ToString() + " " + ChiStrLabel.SizeForWidthAndFontSize(Width, -1) + " " + ChiStrLabel.FittedFontSize;
            EnStrLabel.SizeChanged += (s, e) => EnStrSize.Text = EnStrLabel.Bounds.Size.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
