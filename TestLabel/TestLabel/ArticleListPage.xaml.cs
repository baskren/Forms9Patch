using System;
using System.Collections.Generic;
using TestLabel.ws.model;
using Xamarin.Forms;
namespace TestLabel
{
	public partial class ArticleListPage : ContentPage
	{

		public ArticleListPage()
		{
			InitializeComponent();
			setArticles();
		}

		public async void setArticles() {
			List<ListArticle> data = new List<ListArticle>();
			data.Add(new ListArticle()
			{
				txtArtikel = "Italienischer Hartkäse",
				txtMarke = "Google Plasma reactor",
				cfgTextColor = "#000"
			});
			data.Add(new ListArticle()
			{
				txtArtikel = "Original Serrano Schniken XXL",
				txtMarke = "Google Plasma reactor",
				cfgTextColor = "#000"
			});
			articleList.ItemsSource = data;
		}
	}
}
