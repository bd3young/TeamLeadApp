using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.Services;
using TeamLeadApp.Views;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
	public class ShiftViewModel : BaseOfficerViewModel
	{
		public Command LoadShiftCommand { get; }
		public Command UpdateBreakOneCommand { get; }
		public Command UpdateBreakTwoCommand { get; }
		public Command UpdateLunchCommand { get; }
		public Command UpdateLvCommand { get; }
		public Command UpdateEhsCommand { get; }
		public Command UpdateNotesCommand { get; }
		public Command UpdateShiftCommand { get; }
		public ObservableCollection<Officer> Officers { get; }
		public string CurrentDate { get; set; }
		public string Shift { get; set; } = "LIVE";

		public ShiftViewModel(INavigation _navigation)
		{
			LoadShiftCommand = new Command(async () => await ExecuteLoadShiftCommand());
			Officers = new ObservableCollection<Officer>();
			CurrentDate = "";
			UpdateBreakOneCommand = new Command<Officer>(OnUpdateBreakOne);
			UpdateBreakTwoCommand = new Command<Officer>(OnUpdateBreakTwo);
			UpdateLunchCommand = new Command<Officer>(OnUpdateLunch);
			UpdateLvCommand = new Command<Officer>(OnUpdateLv);
			UpdateEhsCommand = new Command<Officer>(OnUpdateEhs);
			UpdateNotesCommand = new Command<Officer>(OnUpdateNotes);
			UpdateShiftCommand = new Command(OnUpdateShift);
			Navigation = _navigation;
		}

		private void OnUpdateShift(object obj)
		{

			IsBusy = true;	
			
		}

		private async void OnUpdateBreakOne(Officer officer)
		{
			if (officer != null)
			{

				var CurrentOfficer = await App.OfficerService.GetProductAsync(officer.Id);
				if (CurrentOfficer.BreakOne != officer.BreakOne)
				{
					await App.OfficerService.AddProductAsync(officer);
				}

			}
		}

		private async void OnUpdateBreakTwo(Officer officer)
		{
			if (officer != null)
			{

				var CurrentOfficer = await App.OfficerService.GetProductAsync(officer.Id);
				if (CurrentOfficer.BreakTwo != officer.BreakTwo)
				{
					await App.OfficerService.AddProductAsync(officer);
				}

			}
		}

		private async void OnUpdateLunch(Officer officer)
		{
			if (officer != null)
			{

				var CurrentOfficer = await App.OfficerService.GetProductAsync(officer.Id);
				if (CurrentOfficer.Lunch != officer.Lunch)
				{
					await App.OfficerService.AddProductAsync(officer);
				}

			}
		}

		private async void OnUpdateLv(Officer officer)
		{
			if (await App.Current.MainPage.DisplayAlert("Leave - " + officer.FirstName + " " + officer.LastName, "Are you sure you would like to put " + officer.FirstName + " " + officer.LastName + " on Leave.", "Yes", "No"))
			{
				officer.Lv = true;
				officer.Ehs = false;

				Officers.Remove(officer);

				await App.OfficerService.AddProductAsync(officer);
			}
		}

		private async void OnUpdateEhs(Officer officer)
		{
			if (await App.Current.MainPage.DisplayAlert("Extra Hours - " + officer.FirstName + " " + officer.LastName, "Are you sure you would like to put " + officer.FirstName + " " + officer.LastName + " on extra hours.", "Yes", "No"))
			{
				officer.Ehs = true;

				await App.OfficerService.AddProductAsync(officer);
			}
		}

		private async void OnUpdateNotes(Officer officer)
		{
			await App.OfficerService.AddProductAsync(officer);
		}
		private async void UpDateTime()
		{
			var date = await App.DateService.GetProductAsync(1);
			date.Day = DateTime.Today.ToString();
			await App.DateService.AddProductAsync(date);
			CurrentDate = date.Day;

		}
		public void OnAppearing()
		{
			UpDateTime();
			IsBusy = true;
		}

		private async Task ExecuteLoadShiftCommand()
		{
			Officers.Clear();
			IsBusy = true;

			var officers = await App.OfficerService.GetDayOfficersAsync(Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ehsOfficers = await App.OfficerService.GetEhsOfficersAsync();

			officers = officers.OrderBy(o => o.Shift);
			ehsOfficers = ehsOfficers.OrderBy(o => o.Shift);

			if (Shift == "AM")
			{
				try
				{
					foreach (var officer in officers)
					{
						if (officer.Shift == "AM")
						{
							Officers.Add(officer);
						}
					}
					foreach (var officer in ehsOfficers)
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

			if (Shift == "PM")
			{
				try
				{
					foreach (var officer in officers)
					{
						if (officer.Shift == "PM")
						{
							Officers.Add(officer);
						}
					}
					foreach (var officer in ehsOfficers)
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

			if (Shift == "MID") 
			{
				try
				{
					foreach (var officer in officers)
					{
						if (officer.Shift == "MID")
						{
							Officers.Add(officer);
						}
					}
					foreach (var officer in ehsOfficers)
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

			if (Shift == "LIVE") 
			{
				try
				{
					foreach (var officer in officers)
					{
						if (officer.ShiftBegin <= DateTime.Now.TimeOfDay && officer.ShiftEnd > DateTime.Now.TimeOfDay)
						{
							Officers.Add(officer);
						}
					}
					foreach (var officer in ehsOfficers)
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
}
