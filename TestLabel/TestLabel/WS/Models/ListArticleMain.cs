using System;
using System.Collections.Generic;

namespace TestLabel.ws.model
{
	public class ListArticleTopic
	{
		public string txtTopic { get; set; }
		public string txtTermin { get; set; }
		public string idTopic { get; set; }
		public object urlTopicImage { get; set; }
		public string txtDisclaimer { get; set; }
		public string txtPiwikTrackAppend { get; set;}
	}

	public class ListArticle
	{
		public string id { get; set; }
		public string txtArtikel { get; set; }
		public string txtAuslobung { get; set; }
		public string txtEinklinker { get; set; }
		public string txtInhalt { get; set; }
		public string txtInhaltLang { get; set; }
		public string txtBezogenAuf { get; set; }
		public string txtGrundpreis { get; set; }
		public string urlShop { get; set; }
		public string urlImageListe { get; set; }
		public string txtMarke { get; set; }
		public string txtTopic { get; set; }
		public string txtTermin { get; set; }
		public string txtErsparnis { get; set; }
		public string txtBilliger { get; set; }
		public string txtInfo { get; set; }
		public string txtVerkaufspreis { get; set; }
		public string txtAsterix { get; set; }
		public string txtDisclaimer { get; set; }
		public string urlDeeplink { get; set; }
    	public string txtPiwikTrackAppend { get; set; }
    	public string urlShortDeeplink { get; set; }
		public string urlPdf { get; set; }
		public string urlSharingShortDeeplink { get; set; }
		public String txtTerminErinnerung { get; set; }
    	public String cfgColor { get; set; }
    	public String cfgBgColor { get; set; }
    	public String cfgBorderColor { get; set; }
		public String cfgTextColor { get; set; }
	}

	public class Data
	{
		public ListArticleTopic topic { get; set; }
		public List<ListArticle> products { get; set; }
	}

	public class ListArticleMain
	{
		public bool success { get; set; }
		public Data data { get; set; }
	}
}
