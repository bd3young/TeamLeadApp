using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		public Command ResetOfficersCommand { get; }
		public Command UpdateEhsCommand { get; }
		public Command SearchCommand { get; }
		public OfficerViewModel(INavigation _navigation) 
		{
			LoadOfficerCommand = new Command(async()=> await ExecuteLoadOfficerCommand());
			Officers = new ObservableCollection<Officer>();
			AddOfficerCommand = new Command(OnAddOfficer);
			EditOfficerCommand = new Command<Officer>(OnEditOfficer);
			DeleteOfficerCommand = new Command<Officer>(OnDeleteOfficer);
			ResetOfficersCommand = new Command(async()=> await App.OfficerService.ResetOfficers());
			UpdateEhsCommand = new Command<Officer>(OnUpdateEhs);
			SearchCommand = new Command(Search);
			Navigation = _navigation;
		}

		private async void Search(Object obj)
		{
			if (obj.ToString() == "")
			{
				await ExecuteLoadOfficerCommand();
			}
			else 
			{
				var offList = await App.OfficerService.GetProductsAsync();
				Officers.Clear();
				foreach (var officer in offList)
				{
					if (officer.LastName.ToUpper().StartsWith(obj.ToString().ToUpper()) || officer.FirstName.ToUpper().StartsWith(obj.ToString().ToUpper()))
					{
						
						Officers.Add(officer);
					}
				}
			}
		}

		private async void OnDeleteOfficer(Officer officer)
		{
			
			if (officer == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete - " + officer.FirstName + " " + officer.LastName, "Are you sure you would like to Delete " + officer.FirstName + " " + officer.LastName, "Yes", "No"))
			{
				await App.OfficerService.DeleteProductAsync(officer.Id);
				Officers.Remove(officer);
			}
			else 
			{
				return;
			}
			
		}
		private async void OnEditOfficer(Officer officer)
		{
			await Navigation.PushAsync(new AddOfficerPage(officer));
		}
		private async void OnAddOfficer(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddOfficerPage));
		}

		private async void OnUpdateEhs(Officer officer)
		{
			if (await App.Current.MainPage.DisplayAlert("Extra Hours - " + officer.FirstName + " " + officer.LastName, "Would you like to put " + officer.FirstName + " " + officer.LastName + " on extra hours", "Yes", "No")) 
			{
				officer.Lv = false;
				officer.Ehs = true;
				if (officer.Admin == false)
				{
					if (await App.Current.MainPage.DisplayAlert("Breaks - " + officer.FirstName + " " + officer.LastName, "How many breaks", "One", "Three"))
					{
						officer.FullTime = false;
					}
					else
					{
						officer.FullTime = true;
					}
				}


				await App.OfficerService.AddProductAsync(officer);
				await ExecuteLoadOfficerCommand();
			}
			
		}

		public void OnAppearing() 
		{
			IsBusy = true;
		}

		async Task ExecuteLoadOfficerCommand() 
		{
			IsBusy = true;
			var officerList = await App.OfficerService.GetRankOfficersAsync("OFFICER");
			var supOfficerList = await App.OfficerService.GetRankOfficersAsync("SUP");
			var leadOfficerList = await App.OfficerService.GetRankOfficersAsync("LEAD");
			var officers = await App.OfficerService.GetProductsAsync();

			try
			{
				Officers.Clear();

				foreach (var sup in supOfficerList)
				{
					Officers.Add(sup);
				}

				foreach (var lead in leadOfficerList)
				{
					Officers.Add(lead);
				}

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
