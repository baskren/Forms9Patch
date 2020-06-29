using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Helper class used to specify media size used in the generation of PDF documents
    /// </summary>
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

        /// <summary>
        /// Creates a new PaperSize from length and height dimensions in inches 
        /// </summary>
        /// <param name="inchWidth"></param>
        /// <param name="inchHeight"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        static PageSize CreateInInches(double inchWidth, double inchHeight, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
            => new PageSize
            {
                Width = inchWidth * 72,
                Height = inchHeight * 72,
                Name = callerName
            };

        /// <summary>
        /// Creates a new PaperSize from length and height dimensions in millimeters
        /// </summary>
        /// <param name="mmWidth"></param>
        /// <param name="mmHeight"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        static PageSize CreateInMillimeters(double mmWidth, double mmHeight, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
            => CreateInInches(mmWidth / 25.4, mmHeight / 25.4, callerName);


        /// <summary>
        /// Sets the current paper size to landscape orientation
        /// </summary>
        public void AsLandscape()
        {
            if (Width < Height)
            {
                var newHeight = Width;
                Width = Height;
                Height = newHeight;
            }
        }

        /// <summary>
        /// Sets the current PaperSize to portrait orientation
        /// </summary>
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

        /// <summary>
        /// Default paper size for current region (either Letter or IsoA4);
        /// </summary>
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

        /// <summary>
        /// International Standards Organization A0 (841 mm x 1189 mm)
        /// </summary>
        public static PageSize IsoA0 => CreateInMillimeters(841, 1189);

        /// <summary>
        /// International Standards Organization A1 (594 mm x 841 mm)
        /// </summary>
        public static PageSize IsoA1 => CreateInMillimeters(594, 841);

        /// <summary>
        /// International Standards Organization A2 (420 mm x 594 mm)
        /// </summary>
        public static PageSize IsoA2 => CreateInMillimeters(420, 594);

        /// <summary>
        /// International Standards Organization A3 (297 mm x 420 mm)
        /// </summary>
        public static PageSize IsoA3 => CreateInMillimeters(297, 420);

        /// <summary>
        /// International Standards Organization A4 (210 mm x 297 mm)
        /// </summary>
        public static PageSize IsoA4 => CreateInMillimeters(210, 297);

        /// <summary>
        /// International Standards Organization A5 (148 mm x 210 mm)
        /// </summary>
        public static PageSize IsoA5 => CreateInMillimeters(148, 210);

        /// <summary>
        /// International Standards Organization A6 (105 mm x 148 mm)
        /// </summary>
        public static PageSize IsoA6 => CreateInMillimeters(105, 148);

        /// <summary>
        /// International Standards Organization A7 (74 mm x 105 mm)
        /// </summary>
        public static PageSize IsoA7 => CreateInMillimeters(74, 105);

        /// <summary>
        /// International Standards Organization A8 (52 mm x 74 mm)
        /// </summary>
        public static PageSize IsoA8 => CreateInMillimeters(52, 74);

        /// <summary>
        /// International Standards Organization A9 (37 mm x 52 mm)
        /// </summary>
        public static PageSize IsoA9 => CreateInMillimeters(37, 52);

        /// <summary>
        /// International Standards Organization A10 (26 mm x 37 mm)
        /// </summary>
        public static PageSize IsoA10 => CreateInMillimeters(26, 37);


        /// <summary>
        /// International Standards Organization B0 (1000 mm x 1414 mm)
        /// </summary>
        public static PageSize IsoB0 => CreateInMillimeters(1000, 1414);

        /// <summary>
        /// International Standards Organization B1 (707 mm x 1000 mm)
        /// </summary>
        public static PageSize IsoB1 => CreateInMillimeters(707, 1000);

        /// <summary>
        /// International Standards Organization B2 (500 mm x 707 mm)
        /// </summary>
        public static PageSize IsoB2 => CreateInMillimeters(500, 707);

        /// <summary>
        /// International Standards Organization B3 (353 mm x 500 mm)
        /// </summary>
        public static PageSize IsoB3 => CreateInMillimeters(353, 500);

        /// <summary>
        /// International Standards Organization B4 (250 mm x 353 mm)
        /// </summary>
        public static PageSize IsoB4 => CreateInMillimeters(250, 353);

        /// <summary>
        /// International Standards Organization B5 (176 mm x 250 mm)
        /// </summary>
        public static PageSize IsoB5 => CreateInMillimeters(176, 250);

        /// <summary>
        /// International Standards Organization B6 (125 mm x 176 mm)
        /// </summary>
        public static PageSize IsoB6 => CreateInMillimeters(125, 176);

        /// <summary>
        /// International Standards Organization B7 (88 mm x 125 mm)
        /// </summary>
        public static PageSize IsoB7 => CreateInMillimeters(88, 125);

        /// <summary>
        /// International Standards Organization B8 (62 mm x 88 mm)
        /// </summary>
        public static PageSize IsoB8 => CreateInMillimeters(62, 88);

        /// <summary>
        /// International Standards Organization B9 (44 mm x 62 mm)
        /// </summary>
        public static PageSize IsoB9 => CreateInMillimeters(44, 62);

        /// <summary>
        /// International Standards Organization B10 (31 mm x 44 mm)
        /// </summary>
        public static PageSize IsoB10 => CreateInMillimeters(31, 44);


        /// <summary>
        /// International Standards Organization C0 (917 mm x 1297 mm)
        /// </summary>
        public static PageSize IsoC0 => CreateInMillimeters(917, 1297);

        /// <summary>
        /// International Standards Organization C1 (648 mm x 917 mm)
        /// </summary>
        public static PageSize IsoC1 => CreateInMillimeters(648, 917);

        /// <summary>
        /// International Standards Organization C2 (458 mm x 648 mm)
        /// </summary>
        public static PageSize IsoC2 => CreateInMillimeters(458, 648);

        /// <summary>
        /// International Standards Organization C3 (324 mm x 458 mm)
        /// </summary>
        public static PageSize IsoC3 => CreateInMillimeters(324, 458);

        /// <summary>
        /// International Standards Organization C4 (229 mm x 324 mm)
        /// </summary>
        public static PageSize IsoC4 => CreateInMillimeters(229, 324);

        /// <summary>
        /// International Standards Organization C5 (162 mm x 229 mm)
        /// </summary>
        public static PageSize IsoC5 => CreateInMillimeters(162, 229);

        /// <summary>
        /// International Standards Organization C6 (114 mm x 162 mm)
        /// </summary>
        public static PageSize IsoC6 => CreateInMillimeters(114, 162);

        /// <summary>
        /// International Standards Organization C7 (81 mm x 114 mm)
        /// </summary>
        public static PageSize IsoC7 => CreateInMillimeters(81, 114);

        /// <summary>
        /// International Standards Organization C8 (57 mm x 81 mm)
        /// </summary>
        public static PageSize IsoC8 => CreateInMillimeters(57, 81);

        /// <summary>
        /// International Standards Organization C9 (40 mm x 57 mm)
        /// </summary>
        public static PageSize IsoC9 => CreateInMillimeters(40, 57);

        /// <summary>
        /// International Standards Organization C10 (28 mm x 40 mm)
        /// </summary>
        public static PageSize IsoC10 => CreateInMillimeters(28, 40);
        #endregion


        #region Japan

        /// <summary>
        /// Japanese Industrial Standards B0 (1030 mm x 1456 mm)
        /// </summary>
        public static PageSize JisB0 => CreateInMillimeters(1030, 1456);

        /// <summary>
        /// Japanese Industrial Standards B1 (728 mm x 1030 mm)
        /// </summary>
        public static PageSize JisB1 => CreateInMillimeters(728, 1030);

        /// <summary>
        /// Japanese Industrial Standards B2 (515 mm x 728 mm)
        /// </summary>
        public static PageSize JisB2 => CreateInMillimeters(515, 728);

        /// <summary>
        /// Japanese Industrial Standards B3 (364 mm x 515 mm)
        /// </summary>
        public static PageSize JisB3 => CreateInMillimeters(364, 515);

        /// <summary>
        /// Japanese Industrial Standards B4 (257 mm x 364 mm)
        /// </summary>
        public static PageSize JisB4 => CreateInMillimeters(257, 364);

        /// <summary>
        /// Japanese Industrial Standards B5 (182 mm x 257 mm)
        /// </summary>
        public static PageSize JisB5 => CreateInMillimeters(182, 257);

        /// <summary>
        /// Japanese Industrial Standards B6 (128 mm x 182 mm)
        /// </summary>
        public static PageSize JisB6 => CreateInMillimeters(128, 182);

        /// <summary>
        /// Japanese Industrial Standards B7 (91 mm x 128 mm)
        /// </summary>
        public static PageSize JisB7 => CreateInMillimeters(91, 128);

        /// <summary>
        /// Japanese Industrial Standards B8 (64 mm x 91 mm)
        /// </summary>
        public static PageSize JisB8 => CreateInMillimeters(64, 91);

        /// <summary>
        /// Japanese Industrial Standards B9 (45 mm x 64 mm)
        /// </summary>
        public static PageSize JisB9 => CreateInMillimeters(45, 64);

        /// <summary>
        /// Japanese Industrial Standards B10 (32 mm x 45 mm)
        /// </summary>
        public static PageSize JisB10 => CreateInMillimeters(32, 45);


        /// <summary>
        /// Japanese Industrial Standards Exec (216 mm x 330 mm)
        /// </summary>
        public static PageSize JisExec => CreateInMillimeters(216, 330);

        /// <summary>
        /// Japan Chou2 (111.1 mm x 146 mm)
        /// </summary>
        public static PageSize JpnChou2 => CreateInMillimeters(111.1, 146);

        /// <summary>
        /// Japan Chou3 (120 mm x 235 mm)
        /// </summary>
        public static PageSize JpnChou3 => CreateInMillimeters(120, 235);

        /// <summary>
        /// Japan Chou4 (90 mm x 205 mm)
        /// </summary>
        public static PageSize JpnChou4 => CreateInMillimeters(90, 205);

        /// <summary>
        /// Japan Hagaki Postcard (100 mm x 148 mm)
        /// </summary>
        public static PageSize JpnHagaki => CreateInMillimeters(100, 148);

        /// <summary>
        /// Japan Kahu (240 mm x 322.1 mm)
        /// </summary>
        public static PageSize JpnKahu => CreateInMillimeters(240, 322.1);

        /// <summary>
        /// Japan Kahu2 (240 mm x 332 mm)
        /// </summary>
        public static PageSize JpnKahu2 => CreateInMillimeters(240, 332);

        /// <summary>
        /// Japan Oufuku (148 mm x 200 mm)
        /// </summary>
        public static PageSize JpnOufuku => CreateInMillimeters(148, 200);

        /// <summary>
        /// Japan You 4 (105 mm x 235 mm)
        /// </summary>
        public static PageSize JpnYou4 => CreateInMillimeters(105, 235);
        #endregion


        #region North American
        /// <summary>
        /// North America Ledger media (17" x 11")
        /// </summary>
        public static PageSize NaLedger => CreateInInches(17, 11);

        /// <summary>
        /// North America Tabloid media (11" x 17")
        /// </summary>
        public static PageSize NaTabloid => CreateInInches(11, 17);

        /// <summary>
        /// North America Legal media (8.5" x 14")
        /// </summary>
        public static PageSize NaLegal => CreateInInches(8.5, 14);

        /// <summary>
        /// North America Letter media (8.5" x 11")
        /// </summary>
        public static PageSize NaLetter => CreateInInches(8.5, 11);

        /// <summary>
        /// North America Government-Letter media (8.0" x 10.5")
        /// </summary>
        public static PageSize NaGovernmentLetter => CreateInInches(8, 10.5);

        /// <summary>
        /// North America Junior Legal media (8.0" x 5.0")
        /// </summary>
        public static PageSize NaJuniorLegal => CreateInInches(8, 5);

        /// <summary>
        /// North America Foolscap media (8" x 13")
        /// </summary>
        public static PageSize NaFoolscap => CreateInInches(8, 13);

        /// <summary>
        /// North America Index Card 3x5 (3" x 5")
        /// </summary>
        public static PageSize NaIndex3x5 => CreateInInches(3, 5);

        /// <summary>
        /// North America Index Card 4x6 (4" x 6")
        /// </summary>
        public static PageSize NaIndex4x6 => CreateInInches(4, 6);

        /// <summary>
        /// North America Index Card 5x8 (5" x 8")
        /// </summary>
        public static PageSize NaIndex5x8 => CreateInInches(5, 8);

        /// <summary>
        /// North America Monarch (7.25" x 10.5")
        /// </summary>
        public static PageSize NaMonarch => CreateInInches(7.25, 10.5);

        /// <summary>
        /// North America Quarto (8" x 10")
        /// </summary>
        public static PageSize NaQuarto => CreateInInches(8, 10);

        /// <summary>
        /// North Amerian #9 Envelope
        /// </summary>
        public static PageSize NaEnvelopeNo09 => CreateInMillimeters(226, 99);

        /// <summary>
        /// North America #10 Envelope
        /// </summary>
        public static PageSize NaEnvelopeNo10 => CreateInMillimeters(241, 105);

        /// <summary>
        /// American National Standards Institute: A (11" x 9.5");
        /// </summary>
        public static PageSize AnsiA => CreateInInches(11, 8.5);

        /// <summary>
        /// American National Standards Institute: B (17" x 11");
        /// </summary>
        public static PageSize AnsiB => CreateInInches(17, 11);

        /// <summary>
        /// American National Standards Institute: C (22" x 17");
        /// </summary>
        public static PageSize AnsiC => CreateInInches(22, 17);

        /// <summary>
        /// American National Standards Institute: D (34" x 22");
        /// </summary>
        public static PageSize AnsiD => CreateInInches(34, 22);

        /// <summary>
        /// American National Standards Institute: E (44" x 34");
        /// </summary>
        public static PageSize AnsiE => CreateInInches(44, 34);
        #endregion


        #region Chinese
        /// <summary>
        /// Chinese Dai Pa Kai media (275mm x 395mm)
        /// </summary>
        public static PageSize OmDaiPaKai => CreateInMillimeters(275, 395);
        /// <summary>
        /// Chinese Jurro Ku Kai media (198mm x 275mm)
        /// </summary>
        public static PageSize OmJuuroKuKai => CreateInMillimeters(198, 275);
        /// <summary>
        /// Chinese Pa Kai media (267mm x 389mm)
        /// </summary>
        public static PageSize OmPaKai => CreateInMillimeters(267, 389);


        /// <summary>
        /// Peoples Republic of China 1 (102mm x 165mm)
        /// </summary>
        public static PageSize Prc1 => CreateInMillimeters(102, 165);

        /// <summary>
        /// Peoples Republic of China 2 (102mm x 176mm)
        /// </summary>
        public static PageSize Prc2 => CreateInMillimeters(102, 176);

        /// <summary>
        /// Peoples Republic of China 3 (125mm x 176mm)
        /// </summary>
        public static PageSize Prc3 => CreateInMillimeters(125, 176);

        /// <summary>
        /// Peoples Republic of China 4 (110mm x 208mm)
        /// </summary>
        public static PageSize Prc4 => CreateInMillimeters(110, 208);

        /// <summary>
        /// Peoples Republic of China 5 (110mm x 220mm)
        /// </summary>
        public static PageSize Prc5 => CreateInMillimeters(110, 220);

        /// <summary>
        /// Peoples Republic of China 6 (120mm x 320mm)
        /// </summary>
        public static PageSize Prc6 => CreateInMillimeters(120, 320);

        /// <summary>
        /// Peoples Republic of China 7 (160mm x 230mm)
        /// </summary>
        public static PageSize Prc7 => CreateInMillimeters(160, 230);

        /// <summary>
        /// Peoples Republic of China 8 (120mm x 309mm)
        /// </summary>
        public static PageSize Prc8 => CreateInMillimeters(120, 309);
        
        /// <summary>
        /// Peoples Republic of China 9 (229mm x 324mm)
        /// </summary>
        public static PageSize Prc9 => CreateInMillimeters(229, 324);

        /// <summary>
        /// Peoples Republic of China 10 (324mm x 458mm)
        /// </summary>
        public static PageSize Prc10 => CreateInMillimeters(324, 458);

        /// <summary>
        /// Peoples Republic of China 16K (146mm x 215mm)
        /// </summary>
        public static PageSize Prc16K => CreateInMillimeters(146, 215);

        /// <summary>
        /// Republic of China 16K (195mm x 270mm)
        /// </summary>
        public static PageSize Roc16K => CreateInMillimeters(195, 270);
        /// <summary>
        /// Republic of China 8K (270mm x 390mm)
        /// </summary>
        public static PageSize Roc8K => CreateInMillimeters(270, 390);
        #endregion

        #endregion


    }
}
