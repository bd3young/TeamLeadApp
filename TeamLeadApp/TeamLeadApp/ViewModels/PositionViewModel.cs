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
    public class PositionViewModel : BasePositionViewModel
    {
		public Command LoadPositionCommand { get; }
		public ObservableCollection<Position> Positions { get; }
		public Command AddPositionCommand { get; }
		public Command PositionsListCommand { get; }
		public Command EditPositionCommand { get; }
		public Command DeletePositionCommand { get; }
		public Command UpdateOfficerOneCommand { get; }
		public Command UpdateOfficerTwoCommand { get; }
		public Command ResetPositionCommand { get; }
		public List<string> CurrentOfficers { get; }
		public string SelectedOfficerOne { get; set; }
		public string SelectedOfficerTwo { get; set; }

		public PositionViewModel(INavigation _navigation)
		{
			LoadPositionCommand = new Command(async () => await ExecuteLoadPositionCommand());
			Positions = new ObservableCollection<Position>();
			AddPositionCommand = new Command(OnAddPosition);
			PositionsListCommand = new Command(PositionsList);
			EditPositionCommand = new Command<Position>(OnEditPosition);
			DeletePositionCommand = new Command<Position>(OnDeletePosition);
			UpdateOfficerOneCommand = new Command<Position>(OnUpdateOfficerOne);
			UpdateOfficerTwoCommand = new Command<Position>(OnUpdateOfficerTwo);
			ResetPositionCommand = new Command<Position>(OnResetPostion);
			CurrentOfficers = new List<string>();
			SelectedOfficerOne = " ";
			SelectedOfficerTwo = " ";
			Navigation = _navigation;
		}

		private async void OnResetPostion(Position position)
		{
			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			CurrentPosition.OfficerOne = " ";
			CurrentPosition.OfficerTwo = " ";
			await App.PositionService.AddProductAsync(CurrentPosition);

			IsBusy = true;
		}

		private async void OnUpdateOfficerTwo(Position position)
		{
			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			if (SelectedOfficerTwo != null)
			{
				if (CurrentPosition.OfficerTwo != SelectedOfficerTwo)
				{
					CurrentPosition.OfficerTwo = SelectedOfficerTwo;
					await App.PositionService.AddProductAsync(CurrentPosition);
					SelectedOfficerTwo = null;
				}
			}
		}

		private async void OnUpdateOfficerOne(Position position)
		{

			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			if (SelectedOfficerOne != null)
			{
				if (CurrentPosition.OfficerOne != SelectedOfficerOne)
				{
					CurrentPosition.OfficerOne = SelectedOfficerOne;
					await App.PositionService.AddProductAsync(CurrentPosition);
					SelectedOfficerOne = null;
				}
			}
		}

		private async void PositionsList()
		{
			await Shell.Current.GoToAsync(nameof(PositionsListPage));
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private async void OnDeletePosition(Position position)
		{
			

			if (position == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Position", "Yes", "No"))
			{
				await App.PositionService.DeleteProductAsync(position.Id);
				Positions.Remove(position);
			}
			else
			{
				return;
			}
		}

		private async void OnEditPosition(Position position)
		{
			await Navigation.PushAsync(new AddPositionPage(position));
		}

		private async void OnAddPosition(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddPositionPage));
		}

		async Task ExecuteLoadPositionCommand()
		{
			Positions.Clear();
			CurrentOfficers.Clear();
			IsBusy = true;

			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}

			var positionList = await App.PositionService.GetProductsAsync();
			var officerList = await App.OfficerService.GetProductsAsync();

			var AmOfficerList = await App.OfficerService.GetShiftOfficersAsync("AM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var MidOfficerList = await App.OfficerService.GetShiftOfficersAsync("MID", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var PmOfficerList = await App.OfficerService.GetShiftOfficersAsync("PM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var AdminOfficerList = await App.OfficerService.GetAdminOfficersAsync(true);
			var ehsMidOfficerList = await App.OfficerService.GetEhsOfficersAsync("MID");
			var ehsAmOfficerList = await App.OfficerService.GetEhsOfficersAsync("AM");
			var ehsPmOfficerList = await App.OfficerService.GetEhsOfficersAsync("PM");

			try 
			{

				if (DateTime.Now.TimeOfDay <= new TimeSpan(11, 00, 00)) 
				{

					foreach (var officer in AmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in MidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in AdminOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsAmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsMidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
				}
				if (DateTime.Now.TimeOfDay >= new TimeSpan(11, 00, 00) && DateTime.Now.TimeOfDay <= new TimeSpan(12, 00, 00))
				{

					foreach (var officer in AmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in MidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in AdminOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in PmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsAmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsMidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsPmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}

				}
				if (DateTime.Now.TimeOfDay >= new TimeSpan(12, 00, 00))
				{

					foreach (var officer in PmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in MidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in AdminOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsPmOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					foreach (var officer in ehsMidOfficerList)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
				}

				//foreach (var officer in officerList) 
				//{
				//	if (DateTime.Now.TimeOfDay >= new TimeSpan(11, 00, 00) && DateTime.Now.TimeOfDay <= new TimeSpan(12, 30, 00)) 
				//	{
				//		if (officer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true && officer.Admin != true
				//		|| officer.Ehs == true && officer.Lv != true
				//		|| officer.Ehs == true && officer.Admin == true)
				//		{
				//			CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//		}
				//		else
				//		{
				//			continue;
				//		}
				//	}
				//	if (DateTime.Now.TimeOfDay <= new TimeSpan(11, 00, 00)) 
				//	{
				//		if (officer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true && officer.Admin != true && officer.Shift == "AM"
				//			|| officer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true && officer.Admin != true && officer.Shift == "MID"
				//			|| officer.Ehs == true && officer.Lv != true && officer.Shift == "AM"
				//			|| officer.Ehs == true && officer.Lv != true && officer.Shift == "MID"
				//			|| officer.Ehs == true && officer.Admin == true)
				//		{
				//			CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//		}
				//		else
				//		{
				//			continue;
				//		}
				//	}
				//	if (DateTime.Now.TimeOfDay >= new TimeSpan(12, 30, 00)) 
				//	{
				//		if (officer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true && officer.Admin != true && officer.Shift == "PM"
				//			|| officer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoThree.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true && officer.Admin != true && officer.Shift == "MID"
				//			|| officer.Ehs == true && officer.Lv != true && officer.Shift == "PM"
				//			|| officer.Ehs == true && officer.Lv != true && officer.Shift == "MID"
				//			|| officer.Ehs == true && officer.Admin == true)
				//		{
				//			CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//		}
				//		else
				//		{
				//			continue;
				//		}
				//	}
				//	else 
				//	{ 
				//		continue; 
				//	}


				//}
				CurrentOfficers.Sort();
				foreach (var position in positionList) 
				{
					Positions.Add(position);
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
