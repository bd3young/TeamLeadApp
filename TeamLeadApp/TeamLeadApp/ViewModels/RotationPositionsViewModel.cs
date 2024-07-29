using Org.W3c.Dom.LS;
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
	public class RotationPositionsViewModel : BaseRotationPositionViewModel
	{
		public Command PreviousRotationCommand { get; set; }
		public Command NextRotationCommand { get; set; }
		public Command PullLastRotationCommand { get; set; }
		public Command LoadRotationPositionCommand { get; }
		public ObservableCollection<RotationPosition> RotationPositions { get; }
		public Rotation Rotation { get; set; } 
		public Rotation LastRotation { get; set; }
		public Command PositionsListCommand { get; }
		public Command DeletePositionCommand { get; }
		public Command UpdateOfficerOneCommand { get; }
		public Command UpdateOfficerTwoCommand { get; }
		public Command ResetPositionCommand { get; }
		public List<string> CurrentOfficers { get; }

		public RotationPositionsViewModel(INavigation _navigation)
		{
			PreviousRotationCommand = new Command(OnPreviousRotation);
			NextRotationCommand = new Command(OnNextRotation);
			PullLastRotationCommand = new Command(OnPullLastRotation);
			LoadRotationPositionCommand = new Command(async () => await ExecuteLoadRotationPositionCommand());
			RotationPositions = new ObservableCollection<RotationPosition>();
			PositionsListCommand = new Command(PositionsList);
			DeletePositionCommand = new Command<RotationPosition>(OnDeletePosition);
			UpdateOfficerOneCommand = new Command<RotationPosition>(OnUpdateOfficerOne);
			UpdateOfficerTwoCommand = new Command<RotationPosition>(OnUpdateOfficerTwo);
			ResetPositionCommand = new Command<RotationPosition>(OnResetPostion);
			CurrentOfficers = new List<string>();
			Navigation = _navigation;
		}

		private async void OnNextRotation()
		{
			var rotation = await App.RotationService.GetProductTAsync(Rotation.RotationTime + new TimeSpan(00, 30, 00));
			if (rotation != null)
			{
				Rotation = rotation;
				OnPropertyChanged(nameof(Rotation));
				IsBusy = true;
			}
		}

		private async void OnPreviousRotation()
		{
			
			var rotation = await App.RotationService.GetProductTAsync(Rotation.RotationTime - new TimeSpan(00, 30, 00));
			if (rotation != null) 
			{
				Rotation = rotation;
				OnPropertyChanged(nameof(Rotation));
				IsBusy = true;
			}
			
		}

		private async void OnPullLastRotation()
		{
			if (await App.Current.MainPage.DisplayAlert("Pull", "Are you sure you would like to Pull the previous Rotation", "Yes", "No")) 
			{
				var currentPositions = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
				if (LastRotation != null) 
				{
					foreach (var position in currentPositions) 
					{
						var lastPosition = await App.RotationPositionService.GetProductRNAsync(LastRotation.Id, position.Name);
						position.OfficerOne = lastPosition.OfficerOne;
						position.OfficerTwo = lastPosition.OfficerTwo;
						if (position.OfficerOne == lastPosition.OfficerOne && position.OfficerOne != "") 
						{
							position.OfficerOneGender = "SAME";
						}
						if (position.OfficerTwo == lastPosition.OfficerTwo && position.OfficerTwo != "")
						{
							position.OfficerTwoGender = "SAME";
						}
						if (position.OfficerOne != lastPosition.OfficerOne)
						{
							position.OfficerOneGender = lastPosition.OfficerOneGender;
						}
						if (position.OfficerTwo != lastPosition.OfficerTwo)
						{
							position.OfficerTwoGender = lastPosition.OfficerTwoGender;
						}

						await App.RotationPositionService.AddProductAsync(position);
					}
					IsBusy = true;
				}
			}
			
			
		}

		private async void OnResetPostion(RotationPosition rotationPosition)
		{
			var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);
			if (CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null) 
			{
				CurrentOfficers.Add(CurrentPosition.OfficerOne);
				CurrentOfficers.Sort();
			}
			if (CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null)
			{
				CurrentOfficers.Add(CurrentPosition.OfficerTwo);
				CurrentOfficers.Sort();
			}

			CurrentPosition.OfficerOne = "";
			CurrentPosition.OfficerTwo = "";
			CurrentPosition.OfficerOneGender = "";
			CurrentPosition.OfficerTwoGender = "";
			await App.RotationPositionService.AddProductAsync(CurrentPosition);
			var currentRotationPositionsList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
			RotationPositions.Clear();
			foreach (var p in currentRotationPositionsList)
			{
				RotationPositions.Add(p);
			}
		}

		private async void OnUpdateOfficerTwo(RotationPosition rotationPosition)
		{
			
			if (rotationPosition.OfficerTwo != null)
			{
				var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);
				var LastPosition = await App.RotationPositionService.GetProductRNAsync(LastRotation.Id, rotationPosition.Name);
				string[] officer = rotationPosition.OfficerTwo.Split(' ');
				if (CurrentPosition.OfficerTwo != rotationPosition.OfficerTwo)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					if (CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null) 
					{
						CurrentOfficers.Add(CurrentPosition.OfficerTwo);
						CurrentOfficers.Sort();
					}
					if (LastPosition.OfficerTwo != rotationPosition.OfficerTwo)
					{
						rotationPosition.OfficerTwoGender = CurrentOfficer.Gender;
					}
					if (LastPosition.OfficerTwo == rotationPosition.OfficerTwo || LastPosition.OfficerOne == rotationPosition.OfficerTwo) 
					{
						rotationPosition.OfficerTwoGender = "SAME";
					}
									
					rotationPosition.OfficerOne = CurrentPosition.OfficerOne;
					CurrentOfficers.Remove(rotationPosition.OfficerTwo);
					await App.RotationPositionService.AddProductAsync(rotationPosition);
					var currentRotationPositionsList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
					RotationPositions.Clear();
					foreach (var p in currentRotationPositionsList)
					{
						RotationPositions.Add(p);
					}
				}
			}
		}

		private async void OnUpdateOfficerOne(RotationPosition rotationPosition)
		{
			if (rotationPosition.OfficerOne != null)
			{
				var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);
				var LastPosition = await App.RotationPositionService.GetProductRNAsync(LastRotation.Id, rotationPosition.Name);
				string[] officer = rotationPosition.OfficerOne.Split(' ');
				if (CurrentPosition.OfficerOne != rotationPosition.OfficerOne)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					if (CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null)
					{
						CurrentOfficers.Add(CurrentPosition.OfficerOne);
						CurrentOfficers.Sort();
					}
					if (LastPosition.OfficerOne != rotationPosition.OfficerOne)
					{
						rotationPosition.OfficerOneGender = CurrentOfficer.Gender;
					}
					if (LastPosition.OfficerOne == rotationPosition.OfficerOne || LastPosition.OfficerTwo == rotationPosition.OfficerOne)
					{
						rotationPosition.OfficerOneGender = "SAME";
					}
					rotationPosition.OfficerTwo = CurrentPosition.OfficerTwo;
					CurrentOfficers.Remove(rotationPosition.OfficerOne);
					await App.RotationPositionService.AddProductAsync(rotationPosition);
					var currentRotationPositionsList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
					RotationPositions.Clear();
					foreach (var p in currentRotationPositionsList) 
					{
						RotationPositions.Add(p);
					}
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

		private async void OnDeletePosition(RotationPosition rotationPosition)
		{


			if (rotationPosition == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Rotation Position", "Yes", "No"))
			{
				await App.RotationPositionService.DeleteProductAsync(rotationPosition.Id);
				RotationPositions.Remove(rotationPosition);
			}
			else
			{
				return;
			}
		}

		async Task ExecuteLoadRotationPositionCommand()
		{
			
			RotationPositions.Clear();
			CurrentOfficers.Clear();
			IsBusy = true;

			LastRotation = await App.RotationService.GetProductTAsync(Rotation.RotationTime - new TimeSpan(00, 30, 00));
			var day = await App.DateService.GetProductAsync(1);
			var currentDay = DateTime.Today.ToString();

			if (currentDay != day.Day)
			{
				await App.OfficerService.ResetOfficers();
				day.Day = currentDay;

				await App.DateService.AddProductAsync(day);
			}

			var rotationPositionList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
			var officers = await App.OfficerService.GetDayOfficersAsync(Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ehsOfficers = await App.OfficerService.GetEhsOfficersAsync();
			//var AmOfficerList = await App.OfficerService.GetShiftOfficersAsync("AM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			//var MidOfficerList = await App.OfficerService.GetShiftOfficersAsync("MID", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			//var PmOfficerList = await App.OfficerService.GetShiftOfficersAsync("PM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			//var AdminOfficerList = await App.OfficerService.GetAdminOfficersAsync(true);
			//var ehsMidOfficerList = await App.OfficerService.GetEhsOfficersAsync("MID");
			//var ehsAmOfficerList = await App.OfficerService.GetEhsOfficersAsync("AM");
			//var ehsPmOfficerList = await App.OfficerService.GetEhsOfficersAsync("PM");

			try
			{
				foreach (var officer in officers) 
				{
					if (officer.ShiftBegin <= Rotation.RotationTime && officer.ShiftEnd > Rotation.RotationTime) 
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
				}
				foreach (var officer in ehsOfficers) 
				{
					if (officer.ShiftBegin <= Rotation.RotationTime && officer.ShiftEnd > Rotation.RotationTime)
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
				}
				//if (Rotation.RotationTime <= new TimeSpan(10, 59, 00))
				//{

				//	foreach (var officer in AmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in MidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in AdminOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsAmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsMidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//}
				//if (Rotation.RotationTime >= new TimeSpan(11, 00, 00) && Rotation.RotationTime <= new TimeSpan(12, 00, 00))
				//{

				//	foreach (var officer in AmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in MidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in AdminOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in PmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsAmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsMidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsPmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}

				//}
				//if (Rotation.RotationTime >= new TimeSpan(12, 30, 00))
				//{

				//	foreach (var officer in PmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in MidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in AdminOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsPmOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//	foreach (var officer in ehsMidOfficerList)
				//	{
				//		CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
				//	}
				//}

				foreach (var position in rotationPositionList)
				{
					CurrentOfficers.Remove(position.OfficerOne);
					CurrentOfficers.Remove(position.OfficerTwo);
					RotationPositions.Add(position);
				}
				CurrentOfficers.Sort();
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
