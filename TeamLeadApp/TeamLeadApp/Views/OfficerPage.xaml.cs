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
	public partial class OfficerPage : ContentPage
	{
		OfficerViewModel officerViewModel;
		public OfficerPage()
		{

			App.OfficerService.AddInitOfficers();
			App.DateService.AddInitDates();

			
			InitializeComponent();
			BindingContext = officerViewModel = new OfficerViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			officerViewModel.OnAppearing();
		}

		private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) 
		{
			var viewModel = (OfficerViewModel)BindingContext;
			if (viewModel.SearchCommand.CanExecute(e.NewTextValue))
			{
				viewModel.SearchCommand.Execute(e.NewTextValue);
			}

		}
	}
}