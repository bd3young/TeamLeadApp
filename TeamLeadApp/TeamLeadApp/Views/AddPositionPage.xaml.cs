using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPositionPage : ContentPage
	{
		public Position Position { get; set; }
		public AddPositionPage()
		{
			InitializeComponent ();
			BindingContext = new AddPositionViewModel();
		}
		public AddPositionPage(Position position)
		{
			InitializeComponent();
			BindingContext = new AddPositionViewModel();

			if (position != null)
			{
				((AddPositionViewModel)BindingContext).Position = position;
			}
		}
	}
}