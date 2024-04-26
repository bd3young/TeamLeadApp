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
		public Command UpdateNotesCommand { get; }
		public Command UpdatePositionCommand { get; }
		public Command UpdateBreakNumberCommand { get; }
		public ObservableCollection<Officer> PmOfficers { get; }
		public ObservableCollection<Officer> AmOfficers { get; }
		public string CurrentDate { get; set; }
		public List<string> Positions { get; }
		public string SelectedPosition { get; set; }
		public string SelectedBreakNumber { get; set; }

		public RotationViewModel(INavigation _navigation)
		{
			LoadPmRotationCommand = new Command(async () => await ExecuteLoadPmRotationCommand());
			LoadAmRotationCommand = new Command(async () => await ExecuteLoadAmRotationCommand());
			PmOfficers = new ObservableCollection<Officer>();
			AmOfficers = new ObservableCollection<Officer>();
			CurrentDate = "";
			SelectedPosition = "";
			SelectedBreakNumber = "";
			Positions = new List<string>();
			UpdateBreakOneCommand = new Command<Officer>(OnUpdateBreakOne);
			UpdateBreakTwoCommand = new Command<Officer>(OnUpdateBreakTwo);
			UpdateLunchCommand = new Command<Officer>(OnUpdateLunch);
			UpdateLvCommand = new Command<Officer>(OnUpdateLv);
			UpdateNotesCommand = new Command<Officer>(OnUpdateNotes);
			UpdatePositionCommand = new Command<Officer>(OnUpdatePosition);
			UpdateBreakNumberCommand = new Command<Officer>(OnUpdateBreakNumber);
			Navigation = _navigation;
		}

		private async void OnUpdateBreakNumber(Officer officer)
		{
			var offList = await App.OfficerService.GetProductsAsync();
			if (SelectedBreakNumber != null) 
			{
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.BreakNumber != SelectedBreakNumber)
					{
						officer.BreakNumber = SelectedBreakNumber;
						await App.OfficerService.AddProductAsync(officer);
						SelectedBreakNumber = null;
					}
				}
			}
			
		}

		private async void UpDateTime()
		{
			var date = await App.DateService.GetProductAsync(1);
			//if (DateTime.Now.ToString().Length == 20) 
			//{
			//	date.Day = DateTime.Now.ToString().Remove(8);
			//}
			//if (DateTime.Now.ToString().Length == 21)
			//{
			//	date.Day = DateTime.Now.ToString().Remove(9);
			//}
			//else 
			//{
			//	date.Day = DateTime.Now.ToString().Remove(10);
			//}
			date.Day = DateTime.Today.ToString();
			await App.DateService.AddProductAsync(date);
			CurrentDate = date.Day;


			
		}

		private async void OnUpdatePosition(Officer officer)
		{
			var offList = await App.OfficerService.GetProductsAsync();
			if (SelectedPosition != null)
			{

				
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.Position != SelectedPosition)
					{
						if (SelectedPosition.ToUpper() == "NONE")
						{
							officer.Position = "";
						}
						else 
						{
							officer.Position = SelectedPosition;
						}
						await App.OfficerService.AddProductAsync(officer);
						SelectedPosition = null;
					}
				}
			}
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
			
				var offList = await App.OfficerService.GetProductsAsync();
				foreach (var product in offList) 
				{
					if (product.Id == officer.Id && product.BreakOne != officer.BreakOne) 
					{
						await App.OfficerService.AddProductAsync(officer);
					}
				}

			}
			
		}
		private async void OnUpdateBreakTwo(Officer officer)
		{
			if (officer != null)
			{

				var offList = await App.OfficerService.GetProductsAsync();
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.BreakTwo != officer.BreakTwo)
					{
						await App.OfficerService.AddProductAsync(officer);

					}
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

				var offList = await App.OfficerService.GetProductsAsync();
				
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.Lunch != officer.Lunch)
					{
						await App.OfficerService.AddProductAsync(officer);
					}
				}
				
			}
		}
		private async void OnUpdateLv(Officer officer)
		{
			// update lv and ehs variables

			officer.Lv = true;
			officer.Ehs = false;

			// variables to help get the officer to their proper shift

			int shiftBegin = Convert.ToInt32(officer.ShiftBegin);
			int shiftEnd = Convert.ToInt32(officer.ShiftEnd);

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
			if (shiftBegin >= 1030 && shiftEnd <= 2000 && officer.Admin == false)
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
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();

			try
			{
	

				Positions.Clear();
				foreach (var position in positionList)
				{
					Positions.Add(position.Name);
				}

				foreach (var sofficer in officerList)
				{
					if (sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "PM" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "MID" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.Ehs == true && sofficer.Lv != true && sofficer.Shift == "PM" && sofficer.Rank.ToUpper().Trim() == "SUP"
						|| sofficer.Ehs == true && sofficer.Admin == true && sofficer.Rank.ToUpper().Trim() == "SUP")
					{
						PmOfficers.Add(sofficer);	
					}
					else
					{
						continue;
					}

				}
				foreach (var lofficer in officerList)
				{
					if (lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "PM" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "MID" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.Ehs == true && lofficer.Lv != true && lofficer.Shift == "PM" && lofficer.Rank.ToUpper().Trim() == "LEAD"
						|| lofficer.Ehs == true && lofficer.Admin == true && lofficer.Rank.ToUpper().Trim() == "LEAD")
					{
						PmOfficers.Add(lofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "PM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "MID" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.Ehs == true && ftofficer.Lv != true && ftofficer.Shift == "PM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true
						|| ftofficer.Ehs == true && ftofficer.Admin == true && ftofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						PmOfficers.Add(ftofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ptofficer in officerList)
				{
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "PM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "MID" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.Ehs == true && ptofficer.Lv != true && ptofficer.Shift == "PM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false 
						|| ptofficer.Ehs == true && ptofficer.Admin == true && ptofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						PmOfficers.Add(ptofficer);

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
		private async Task ExecuteLoadAmRotationCommand()
		{

			AmOfficers.Clear();
			PmOfficers.Clear();
			UpDateTime();
			IsBusy = true;
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}


			try
			{
				

				Positions.Clear();
				foreach (var position in positionList)
				{
					Positions.Add(position.Name);
				}

				foreach (var sofficer in officerList)
				{
					if (sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "AM" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "MID" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.Ehs == true && sofficer.Lv != true && sofficer.Shift == "AM" && sofficer.Rank.ToUpper().Trim() == "SUP"
						|| sofficer.Ehs == true && sofficer.Admin == true && sofficer.Rank.ToUpper().Trim() == "SUP")
					{
						AmOfficers.Add(sofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var lofficer in officerList)
				{
					if (lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "AM" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "MID" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.Ehs == true && lofficer.Lv != true && lofficer.Shift == "AM" && lofficer.Rank.ToUpper().Trim() == "LEAD"
						|| lofficer.Ehs == true && lofficer.Admin == true && lofficer.Rank.ToUpper().Trim() == "LEAD")
					{
						AmOfficers.Add(lofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "AM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "MID" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.Ehs == true && ftofficer.Lv != true && ftofficer.Shift == "AM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true
						|| ftofficer.Ehs == true && ftofficer.Admin == true && ftofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						AmOfficers.Add(ftofficer);
					}
					else
					{
						continue;
					}

				}
				foreach (var ptofficer in officerList)
				{
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "AM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "MID" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.Ehs == true && ptofficer.Lv != true && ptofficer.Shift == "AM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false
						|| ptofficer.Ehs == true && ptofficer.Admin == true && ptofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						AmOfficers.Add(ptofficer);
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
