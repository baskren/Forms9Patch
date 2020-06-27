using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public class PageSize
    {
        #region Properties

        #region Width
        /// <summary>
        /// Page width in points (72 points per inch)
        /// </summary>
        public double Width { get; internal set; }
        #endregion

        #region Height
        /// <summary>
        /// Page height in points (72 points per inch)
        /// </summary>
        public double Height { get; internal set; }

        /// <summary>
        /// Name of Page Size
        /// </summary>
        public string Name { get; internal set; }
        #endregion

        #endregion

        internal PageSize()
        {
        }

        static PageSize CreateInInches(double inchWidth, double inchHeight, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
            => new PageSize
            {
                Width = inchWidth * 72,
                Height = inchHeight * 72,
                Name = callerName
            };

        static PageSize CreateInMillimeters(double mmWidth, double mmHeight, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
            => CreateInInches(mmWidth / 25.4, mmHeight / 25.4, callerName);


        public void AsLandscape()
        {
            if (Width < Height)
            {
                var newHeight = Width;
                Width = Height;
                Height = newHeight;
            }
        }

        public void AsPortrait()
        {
            if (Width > Height)
            {
                var newHeight = Width;
                Width = Height;
                Height = newHeight;
            }
        }


        #region Static Values (Constants)
        public static PageSize Default
        {
            get
            {
                var countryCode = System.Globalization.RegionInfo.CurrentRegion.ThreeLetterISORegionName;
                //var country = locale.GetCountryCodeDisplayName(countryCode);
                if (//Counties that use Letter
                    countryCode == "CA" || countryCode == "CAN" || // Canada
                    countryCode == "CL" || countryCode == "CHL" || // Chile
                    countryCode == "CO" || countryCode == "COL" || // Columbia
                    countryCode == "CR" || countryCode == "CRI" || // Costa Rica
                    countryCode == "MX" || countryCode == "MEX" || // Mexico
                    countryCode == "PA" || countryCode == "PAN" || // Panama
                    countryCode == "DO" || countryCode == "DOM" || // Domincan Republi
                    countryCode == "US" || countryCode == "USA" // United States
                    )
                    return NaLetter;
                return IsoA4;
            }
        }


        #region International Paper Sizes
        public static PageSize IsoA0 => CreateInMillimeters(841, 1189);
        public static PageSize IsoA1 => CreateInMillimeters(594, 841);
        public static PageSize IsoA2 => CreateInMillimeters(420, 594);
        public static PageSize IsoA3 => CreateInMillimeters(297, 420);
        public static PageSize IsoA4 => CreateInMillimeters(210, 297);
        public static PageSize IsoA5 => CreateInMillimeters(148, 210);
        public static PageSize IsoA6 => CreateInMillimeters(105, 148);
        public static PageSize IsoA7 => CreateInMillimeters(74, 105);
        public static PageSize IsoA8 => CreateInMillimeters(52, 74);
        public static PageSize IsoA9 => CreateInMillimeters(37, 52);
        public static PageSize IsoA10 => CreateInMillimeters(26, 37);

        public static PageSize IsoB0 => CreateInMillimeters(1000, 1414);
        public static PageSize IsoB1 => CreateInMillimeters(707, 1000);
        public static PageSize IsoB2 => CreateInMillimeters(500, 707);
        public static PageSize IsoB3 => CreateInMillimeters(353, 500);
        public static PageSize IsoB4 => CreateInMillimeters(250, 353);
        public static PageSize IsoB5 => CreateInMillimeters(176, 250);
        public static PageSize IsoB6 => CreateInMillimeters(125, 176);
        public static PageSize IsoB7 => CreateInMillimeters(88, 125);
        public static PageSize IsoB8 => CreateInMillimeters(62, 88);
        public static PageSize IsoB9 => CreateInMillimeters(44, 62);
        public static PageSize IsoB10 => CreateInMillimeters(31, 44);

        public static PageSize IsoC0 => CreateInMillimeters(917, 1297);
        public static PageSize IsoC1 => CreateInMillimeters(648, 917);
        public static PageSize IsoC2 => CreateInMillimeters(458, 648);
        public static PageSize IsoC3 => CreateInMillimeters(324, 458);
        public static PageSize IsoC4 => CreateInMillimeters(229, 324);
        public static PageSize IsoC5 => CreateInMillimeters(162, 229);
        public static PageSize IsoC6 => CreateInMillimeters(114, 162);
        public static PageSize IsoC7 => CreateInMillimeters(81, 114);
        public static PageSize IsoC8 => CreateInMillimeters(57, 81);
        public static PageSize IsoC9 => CreateInMillimeters(40, 57);
        public static PageSize IsoC10 => CreateInMillimeters(28, 40);
        #endregion


        #region Japan
        public static PageSize JisB0 => CreateInMillimeters(1030, 1456);
        public static PageSize JisB1 => CreateInMillimeters(728, 1030);
        public static PageSize JisB2 => CreateInMillimeters(515, 728);
        public static PageSize JisB3 => CreateInMillimeters(364, 515);
        public static PageSize JisB4 => CreateInMillimeters(257, 364);
        public static PageSize JisB5 => CreateInMillimeters(182, 257);
        public static PageSize JisB6 => CreateInMillimeters(128, 182);
        public static PageSize JisB7 => CreateInMillimeters(91, 128);
        public static PageSize JisB8 => CreateInMillimeters(64, 91);
        public static PageSize JisB9 => CreateInMillimeters(45, 64);
        public static PageSize JisB10 => CreateInMillimeters(32, 45);

        public static PageSize JisExec => CreateInMillimeters(216, 330);
        public static PageSize JpnChou2 => CreateInMillimeters(111.1, 146);
        public static PageSize JpnChou3 => CreateInMillimeters(120, 235);
        public static PageSize JpnChou4 => CreateInMillimeters(90, 205);
        public static PageSize JpnHagaki => CreateInMillimeters(100, 148);
        public static PageSize JpnKahu => CreateInMillimeters(240, 322.1);
        public static PageSize JpnKahu2 => CreateInMillimeters(240, 332);
        public static PageSize JpnOufuku => CreateInMillimeters(148, 200);
        public static PageSize JpnYou4 => CreateInMillimeters(105, 235);
        #endregion

        #region North American
        public static PageSize NaLedger => CreateInInches(17, 11);
        public static PageSize NaTabloid => CreateInInches(11, 17);
        public static PageSize NaLegal => CreateInInches(8.5, 14);
        public static PageSize NaLetter => CreateInInches(8.5, 11);
        public static PageSize NaGovernmentLetter => CreateInInches(8, 10.5);
        public static PageSize NaJuniorLegal => CreateInInches(8, 5);

        public static PageSize NaFoolscap => CreateInInches(8, 13);
        public static PageSize NaIndex3x5 => CreateInInches(3, 5);
        public static PageSize NaIndex4x6 => CreateInInches(4, 6);
        public static PageSize NaIndex5x8 => CreateInInches(5, 8);
        public static PageSize NaMonarch => CreateInInches(7.25, 10.5);
        public static PageSize NaQuarto => CreateInInches(8, 10);

        public static PageSize NaEnvelopeNo09 => CreateInMillimeters(226, 99);
        public static PageSize NaEnvelopeNo10 => CreateInMillimeters(241, 105);

        public static PageSize AnsiA => CreateInInches(11, 8.5);
        public static PageSize AnsiB => CreateInInches(17, 11);
        public static PageSize AnsiC => CreateInInches(22, 17);
        public static PageSize AnsiD => CreateInInches(34, 22);
        public static PageSize AnsiE => CreateInInches(44, 34);
        #endregion


        #region Chinese
        public static PageSize OmDaiPaKai => CreateInMillimeters(275, 395);
        public static PageSize OmJuuroKuKai => CreateInMillimeters(198, 275);

        public static PageSize Prc1 => CreateInMillimeters(102, 165);
        public static PageSize Prc2 => CreateInMillimeters(102, 176);
        public static PageSize Prc3 => CreateInMillimeters(125, 176);
        public static PageSize Prc4 => CreateInMillimeters(110, 208);
        public static PageSize Prc5 => CreateInMillimeters(110, 220);
        public static PageSize Prc6 => CreateInMillimeters(120, 320);
        public static PageSize Prc7 => CreateInMillimeters(160, 230);
        public static PageSize Prc8 => CreateInMillimeters(120, 309);
        public static PageSize Prc9 => CreateInMillimeters(229, 324);
        public static PageSize Prc10 => CreateInMillimeters(324, 458);
        public static PageSize Prc16K => CreateInMillimeters(146, 215);

        public static PageSize Roc16K => CreateInMillimeters(195, 270);
        public static PageSize Roc8K => CreateInMillimeters(270, 390);
        #endregion

        #endregion


    }
}
