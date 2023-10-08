using System.ComponentModel;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;

namespace TeamLeadApp.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}