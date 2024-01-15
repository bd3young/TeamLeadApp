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
		public Command ResetOfficersCommand { get; }
		public Command UpdateEhsCommand { get; }
		public OfficerViewModel(INavigation _navigation) 
		{
			LoadOfficerCommand = new Command(async()=> await ExecuteLoadOfficerCommand());
			Officers = new ObservableCollection<Officer>();
			AddOfficerCommand = new Command(OnAddOfficer);
			EditOfficerCommand = new Command<Officer>(OnEditOfficer);
			DeleteOfficerCommand = new Command<Officer>(OnDeleteOfficer);
			ResetOfficersCommand = new Command(ResetOfficers);
			UpdateEhsCommand = new Command<Officer>(OnUpdateEhs);
			Navigation = _navigation;
		}

		private async void OnDeleteOfficer(Officer officer)
		{
			
			if (officer == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Employee", "Yes", "No"))
			{
				await App.OfficerService.DeleteProductAsync(officer.Id);
				await ExecuteLoadOfficerCommand();
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
			officer.Lv = false;
			officer.Ehs = true;

			await App.OfficerService.AddProductAsync(officer);
		}

		async void ResetOfficers(object obj)
		{
			var officerList = await App.OfficerService.GetProductsAsync();
			foreach (var officer in officerList)
			{
				officer.BreakOne = false; 
				officer.BreakTwo = false;
				officer.Lunch = false;
				officer.Notes = "";
				officer.Lv = false;
				officer.Ehs = false;

				await App.OfficerService.AddProductAsync(officer);
			}
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
				foreach (var stsofficer in officerList)
				{
					if (stsofficer.Rank.ToUpper().Trim() == "STSO")
					{
						Officers.Add(stsofficer);
					}
					else 
					{
						continue;
					}
					
				}

				foreach (var ltsofficer in officerList)
				{
					if (ltsofficer.Rank.ToUpper().Trim() == "LTSO")
					{
						Officers.Add(ltsofficer);
					}
					else
					{
						continue;
					}

				}

				foreach (var ftpmofficer in officerList)
				{
					if (ftpmofficer.Rank.ToUpper().Trim() == "TSO" && ftpmofficer.FullTime == true && ftpmofficer.Shift == "PM" )
					{
						Officers.Add(ftpmofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ftamofficer in officerList)
				{
					if (ftamofficer.Rank.ToUpper().Trim() == "TSO" && ftamofficer.FullTime == true && ftamofficer.Shift == "AM")
					{
						Officers.Add(ftamofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ptpmofficer in officerList)
				{
					if (ptpmofficer.Rank.ToUpper().Trim() == "TSO" && ptpmofficer.FullTime == false && ptpmofficer.Shift == "PM")
					{
						Officers.Add(ptpmofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ptamofficer in officerList)
				{
					if (ptamofficer.Rank.ToUpper().Trim() == "TSO" && ptamofficer.FullTime == false && ptamofficer.Shift == "AM")
					{
						Officers.Add(ptamofficer);
					}
					else
					{
						continue;
					}

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
