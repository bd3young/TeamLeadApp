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
    public class RotationViewModel : BaseOfficerViewModel
    {
		public Command LoadPmRotationCommand { get; }
		public Command LoadAmRotationCommand { get; }
		public Command UpdateBreakOneCommand { get; }
		public Command UpdateBreakTwoCommand { get; }
		public Command UpdateLunchCommand { get; }
		public Command UpdateLvCommand { get; }
		public Command UpdateEhsCommand { get; }
		public Command UpdateNotesCommand { get; }
		public Command UpdatePositionCommand { get; }
		public Command UpdateBreakNumberCommand { get; }
		public ObservableCollection<Officer> PmOfficers { get; }
		public ObservableCollection<Officer> AmOfficers { get; }
		public string CurrentDate { get; set; }
		public string SelectedBreakNumber { get; set; }

		public RotationViewModel(INavigation _navigation)
		{
			LoadPmRotationCommand = new Command(async () => await ExecuteLoadPmRotationCommand());
			LoadAmRotationCommand = new Command(async () => await ExecuteLoadAmRotationCommand());
			PmOfficers = new ObservableCollection<Officer>();
			AmOfficers = new ObservableCollection<Officer>();
			CurrentDate = "";
			SelectedBreakNumber = "";
			UpdateBreakOneCommand = new Command<Officer>(OnUpdateBreakOne);
			UpdateBreakTwoCommand = new Command<Officer>(OnUpdateBreakTwo);
			UpdateLunchCommand = new Command<Officer>(OnUpdateLunch);
			UpdateLvCommand = new Command<Officer>(OnUpdateLv);
			UpdateEhsCommand = new Command<Officer>(OnUpdateEhs);
			UpdateNotesCommand = new Command<Officer>(OnUpdateNotes);
			UpdateBreakNumberCommand = new Command<Officer>(OnUpdateBreakNumber);
			Navigation = _navigation;
		}

		private async void OnUpdateEhs(Officer officer)
		{
			if (await App.Current.MainPage.DisplayAlert("Extra Hours - " + officer.FirstName + " " + officer.LastName, "Are you sure you would like to put " + officer.FirstName + " " + officer.LastName + " on extra hours.", "Yes", "No")) 
			{
				officer.Ehs = true;

				await App.OfficerService.AddProductAsync(officer);
			}
			
		}

		private async void OnUpdateBreakNumber(Officer officer)
		{
			var CurrentOfficer = await App.OfficerService.GetProductAsync(officer.Id);
			if (SelectedBreakNumber != null) 
			{
				if (CurrentOfficer.BreakNumber != SelectedBreakNumber)
				{
					officer.BreakNumber = SelectedBreakNumber;
					await App.OfficerService.AddProductAsync(officer);
					SelectedBreakNumber = null;
				}
			}
			
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
		private async void OnUpdateNotes(Officer officer)
		{

			await App.OfficerService.AddProductAsync(officer);
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
			// update lv and ehs variables

			officer.Lv = true;
			officer.Ehs = false;

			// if statements to remove the officer from the proper list

			if (officer.Shift == "AM")
			{
				AmOfficers.Remove(officer);
			}
			if (officer.Shift == "PM")
			{
				PmOfficers.Remove(officer);
			}
			if (officer.Admin == true) 
			{
				AmOfficers.Remove(officer);
				PmOfficers.Remove(officer);
			}
			if (officer.Shift == "MID") 
			{
				AmOfficers.Remove(officer);
				PmOfficers.Remove(officer);
			}

			// if statements to return the officer to the proper shift and Fulltime status

			if (officer.ShiftEnd - officer.ShiftBegin == new System.TimeSpan(08, 30, 00))
			{
				officer.FullTime = true;
			}
			else
			{
				officer.FullTime = false;
			}
			if (officer.ShiftBegin >= new System.TimeSpan(03, 00, 00) && officer.ShiftEnd <= new System.TimeSpan(12, 30, 00) && officer.Admin == false)
			{
				officer.Shift = "AM";

			}
			else if (officer.ShiftBegin >= new System.TimeSpan(10, 30, 00) && officer.ShiftEnd <= new System.TimeSpan(20, 00, 00) && officer.Admin == false)
			{
				officer.Shift = "PM";
			}
			else
			{
				officer.Shift = "MID";
			}

			await App.OfficerService.AddProductAsync(officer);

			


		}
		private async Task ExecuteLoadPmRotationCommand()
		{
			
			PmOfficers.Clear();
			AmOfficers.Clear();
			UpDateTime();
			IsBusy = true;
			
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}
			var supPmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("PM", "SUP", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var supMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "SUP", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var supAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "SUP");
			var leadPmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("PM", "LEAD", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var leadMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "LEAD", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var leadAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "LEAD");
			var ftPmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("PM", "OFFICER", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ftMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "OFFICER", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ftAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "OFFICER");
			var ptPmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("PM", "OFFICER", false, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ptMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "OFFICER", false, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ehsMidOfficerList = await App.OfficerService.GetEhsOfficersAsync("MID");
			var ehsPmOfficerList = await App.OfficerService.GetEhsOfficersAsync("PM");

			try
			{

				foreach (var officer in supPmOfficerList) 
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in supMidOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in supAdminOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in leadPmOfficerList) 
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in leadMidOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in leadAdminOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ftPmOfficerList) 
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ftMidOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ftAdminOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ptPmOfficerList) 
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ptMidOfficerList)
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ehsMidOfficerList) 
				{
					PmOfficers.Add(officer);
				}
				foreach (var officer in ehsPmOfficerList)
				{
					PmOfficers.Add(officer);
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
		private async Task ExecuteLoadAmRotationCommand()
		{

			AmOfficers.Clear();
			PmOfficers.Clear();
			UpDateTime();
			IsBusy = true;
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}
			var supAmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("AM", "SUP", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var supMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "SUP", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var supAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "SUP");
			var leadAmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("AM", "LEAD", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var leadMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "LEAD", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var leadAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "LEAD");
			var ftAmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("AM", "OFFICER", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ftMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "OFFICER", true, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ftAdminOfficerList = await App.OfficerService.GetAdminRankOfficersAsync(true, "OFFICER");
			var ptAmOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("AM", "OFFICER", false, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ptMidOfficerList = await App.OfficerService.GetShiftRankOfficersAsync("MID", "OFFICER", false, Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ehsMidOfficerList = await App.OfficerService.GetEhsOfficersAsync("MID");
			var ehsAmOfficerList = await App.OfficerService.GetEhsOfficersAsync("AM");

			try
			{

				foreach (var officer in supAmOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in supMidOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in supAdminOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in leadAmOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in leadMidOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in leadAdminOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ftAmOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ftMidOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ftAdminOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ptAmOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ptMidOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ehsMidOfficerList)
				{
					AmOfficers.Add(officer);
				}
				foreach (var officer in ehsAmOfficerList)
				{
					AmOfficers.Add(officer);
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
