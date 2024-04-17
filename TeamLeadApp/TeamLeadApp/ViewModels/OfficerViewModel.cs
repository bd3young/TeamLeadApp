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
			ResetOfficersCommand = new Command(async()=> await ResetOfficers());
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
					if (officer.LastName.ToUpper().Contains(obj.ToString().ToUpper()) || officer.FirstName.ToUpper().Contains(obj.ToString().ToUpper()))
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

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Officer", "Yes", "No"))
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
			officer.BreakOne = false;
			officer.BreakTwo = false;
			officer.Lunch = false;
			officer.Notes = "";
			officer.Lv = false;
			officer.Position = "";
			officer.BreakNumber = "";
			if (officer.Admin == false) 
			{
				if (await App.Current.MainPage.DisplayAlert("Extra Hours", "Which shift?", "PM", "AM"))
				{
					officer.Shift = "PM";
	
				}
				else 
				{
					officer.Shift = "AM";
				}
				if (await App.Current.MainPage.DisplayAlert("Breaks", "How many", "One", "Three"))
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

		async Task ResetOfficers()
		{

			//if (await App.Current.MainPage.DisplayAlert("New Day", "Are you sure you would like to start a New Day", "Yes", "No"))
			//{
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var officer in officerList)
				{
					officer.BreakOne = false;
					officer.BreakTwo = false;
					officer.Lunch = false;
					officer.Notes = "";
					officer.Lv = false;
					officer.Ehs = false;
					officer.Position = "";
					officer.BreakNumber = "";
					int shiftBegin = Convert.ToInt32(officer.ShiftBegin);
					int shiftEnd = Convert.ToInt32(officer.ShiftEnd);
					if (shiftEnd - shiftBegin == 830 || shiftEnd - shiftBegin == 870 || shiftEnd - shiftBegin == 1030 || shiftEnd - shiftBegin == 1070)
					{
						officer.FullTime = true;
					}
					else 
					{
						officer.FullTime = false;
					}
					if (shiftBegin >= 300 && shiftEnd <= 1400 && officer.Admin == false)
					{
						officer.Shift = "AM";
						
					}
					else if (shiftBegin >= 900 && shiftEnd <= 2000 && officer.Admin == false) 
					{
						officer.Shift = "PM";
					}

					await App.OfficerService.AddProductAsync(officer);
				}
				var choreList = await App.ChoreService.GetProductsAsync();
				foreach (var chore in choreList) 
				{
					chore.IsCompleted = false;
					chore.Time = new System.TimeSpan();

					await App.ChoreService.AddProductAsync(chore);
				}
				//await ExecuteLoadOfficerCommand();
			//}
			//else
			//{
			//	return;
			//}
		}

		public void OnAppearing() 
		{
			IsBusy = true;
		}

		async Task ExecuteLoadOfficerCommand() 
		{
			IsBusy = true;
			var officerList = await App.OfficerService.GetProductsAsync();
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();
			try
			{
				Officers.Clear();

				if (currentDay != day.Day) 
				{
					await ResetOfficers();
					day.Day = currentDay;

					await App.DateService.AddProductAsync(day);
				}

				//foreach (var officer in officerList)
				//{

				//	Officers.Add(officer);

				//}

				foreach (var sofficer in officerList)
				{
					if (sofficer.Rank.ToUpper().Trim() == "SUP")
					{
						Officers.Add(sofficer);
					}
					else
					{
						continue;
					}

				}

				foreach (var lofficer in officerList)
				{
					if (lofficer.Rank.ToUpper().Trim() == "LEAD")
					{
						Officers.Add(lofficer);
					}
					else
					{
						continue;
					}

				}

				foreach (var ftpmofficer in officerList)
				{
					if (ftpmofficer.Rank.ToUpper().Trim() == "OFFICER" && ftpmofficer.FullTime == true && ftpmofficer.Shift == "PM")
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
					if (ftamofficer.Rank.ToUpper().Trim() == "OFFICER" && ftamofficer.FullTime == true && ftamofficer.Shift == "AM")
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
					if (ptpmofficer.Rank.ToUpper().Trim() == "OFFICER" && ptpmofficer.FullTime == false && ptpmofficer.Shift == "PM")
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
					if (ptamofficer.Rank.ToUpper().Trim() == "OFFICER" && ptamofficer.FullTime == false && ptamofficer.Shift == "AM")
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
