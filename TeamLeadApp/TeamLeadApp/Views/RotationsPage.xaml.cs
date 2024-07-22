using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RotationsPage : ContentPage
	{
		RotationsViewModel rotationsViewModel;
		public RotationsPage()
		{

			InitializeComponent();
			BindingContext = rotationsViewModel = new RotationsViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			rotationsViewModel.OnAppearing();
		}

		private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewModel = (RotationsViewModel)BindingContext;
			if (viewModel.SearchCommand.CanExecute(e.NewTextValue))
			{
				viewModel.SearchCommand.Execute(e.NewTextValue);
			}

		}
	}
}