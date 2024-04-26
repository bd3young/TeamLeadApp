using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using TeamLeadApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PositionsListPage : ContentPage
	{
		PositionViewModel positionViewModel;
		public PositionsListPage()
		{
			InitializeComponent();
			BindingContext = positionViewModel = new PositionViewModel(Navigation);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			positionViewModel.OnAppearing();
		}
	}
}