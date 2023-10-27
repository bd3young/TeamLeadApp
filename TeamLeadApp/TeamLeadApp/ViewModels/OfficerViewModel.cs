using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.Views;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
	public class OfficerViewModel : BaseOfficerViewModel
	{
		public Command LoadOfficerCommand { get; }
		public ObservableCollection<Officer> Officers { get; }
		public Command AddOfficerCommand { get; }
		public Command EditOfficerCommand { get; }
		public Command DeleteOfficerCommand { get; }
		public OfficerViewModel(INavigation _navigation) 
		{
			LoadOfficerCommand = new Command(async()=> await ExecuteLoadOfficerCommand());
			Officers = new ObservableCollection<Officer>();
			AddOfficerCommand = new Command(OnAddOfficer);
			EditOfficerCommand = new Command<Officer>(OnEditOfficer);
			DeleteOfficerCommand = new Command<Officer>(OnDeleteOfficer);
			Navigation = _navigation;
		}

		private async void OnDeleteOfficer(Officer officer)
		{
			if (officer == null)
			{
				return;
			}

			await App.OfficerService.DeleteProductAsync(officer.Id);
			await ExecuteLoadOfficerCommand();
		}
		private async void OnEditOfficer(Officer officer)
		{
			await Navigation.PushAsync(new AddOfficerPage(officer));
		}
		private async void OnAddOfficer(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddOfficerPage));
		}

		public void OnAppearing() 
		{
			IsBusy = true;
		}

		async Task ExecuteLoadOfficerCommand() 
		{
			IsBusy = true;

			try
			{
				Officers.Clear();
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var officer in officerList)
				{
					Officers.Add(officer);
				}
			}
			catch (Exception)
			{

				throw;
			}
			finally 
			{ 
				IsBusy = false; 
			}

			
		}
	}
}
