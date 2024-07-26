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
		//public Command AddPositionCommand { get; }
		public Command PositionsListCommand { get; }
		//public Command EditPositionCommand { get; }
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
			//AddPositionCommand = new Command(OnAddPosition);
			PositionsListCommand = new Command(PositionsList);
			//EditPositionCommand = new Command<Position>(OnEditPosition);
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
			
			TimeSpan rotationTime = Rotation.RotationTime - new TimeSpan(00,30,00);
			var lastRotation = await App.RotationService.GetProductTAsync(rotationTime);
			var currentPositions = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
			

			foreach (var position in currentPositions) 
			{
				var lastPosition = await App.RotationPositionService.GetProductRNAsync(lastRotation.Id, position.Name);
				position.OfficerOne = lastPosition.OfficerOne;
				position.OfficerTwo = lastPosition.OfficerTwo;
				position.OfficerOneGender = lastPosition.OfficerOneGender;
				position.OfficerTwoGender = lastPosition.OfficerTwoGender;


				await App.RotationPositionService.AddProductAsync(position);
			}
			IsBusy = true;
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

			//IsBusy = true;
		}

		private async void OnUpdateOfficerTwo(RotationPosition rotationPosition)
		{
			var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);

			if (rotationPosition.OfficerTwo != null)
			{
				string[] officer = rotationPosition.OfficerTwo.Split(' ');
				if (CurrentPosition.OfficerTwo != rotationPosition.OfficerTwo)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					if (CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null) 
					{
						CurrentOfficers.Add(CurrentPosition.OfficerTwo);
						CurrentOfficers.Sort();
					}
					CurrentOfficers.Remove(rotationPosition.OfficerTwo);
					rotationPosition.OfficerTwoGender = CurrentOfficer.Gender;
					rotationPosition.OfficerOne = CurrentPosition.OfficerOne;
					await App.RotationPositionService.AddProductAsync(rotationPosition);
					var currentRotationPositionsList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
					RotationPositions.Clear();
					foreach (var p in currentRotationPositionsList)
					{
						RotationPositions.Add(p);
					}
					//IsBusy = true;
				}
			}
		}

		private async void OnUpdateOfficerOne(RotationPosition rotationPosition)
		{

			var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);

			if (rotationPosition.OfficerOne != null)
			{
				string[] officer = rotationPosition.OfficerOne.Split(' ');
				if (CurrentPosition.OfficerOne != rotationPosition.OfficerOne)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					if (CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null)
					{
						CurrentOfficers.Add(CurrentPosition.OfficerOne);
						CurrentOfficers.Sort();
					}
					CurrentOfficers.Remove(rotationPosition.OfficerOne);
					rotationPosition.OfficerOneGender = CurrentOfficer.Gender;
					rotationPosition.OfficerTwo = CurrentPosition.OfficerTwo;
					await App.RotationPositionService.AddProductAsync(rotationPosition);
					var currentRotationPositionsList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
					RotationPositions.Clear();
					foreach (var p in currentRotationPositionsList) 
					{
						RotationPositions.Add(p);
					}

					//var index = RotationPositions.IndexOf(rotationPosition);
					//RotationPositions.Remove(rotationPosition);
					//RotationPositions.Insert(index, rotationPosition);


					//RotationPositions[index] = rotationPosition;
					//IsBusy = true;
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

		//private async void OnEditPosition(Position position)
		//{
		//	await Navigation.PushAsync(new AddPositionPage(position));
		//}

		//private async void OnAddPosition(object obj)
		//{
		//	await Shell.Current.GoToAsync(nameof(AddPositionPage));
		//}

		async Task ExecuteLoadRotationPositionCommand()
		{
			
			RotationPositions.Clear();
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

			var rotationPositionList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);

			var AmOfficerList = await App.OfficerService.GetShiftOfficersAsync("AM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var MidOfficerList = await App.OfficerService.GetShiftOfficersAsync("MID", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var PmOfficerList = await App.OfficerService.GetShiftOfficersAsync("PM", Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var AdminOfficerList = await App.OfficerService.GetAdminOfficersAsync(true);
			var ehsMidOfficerList = await App.OfficerService.GetEhsOfficersAsync("MID");
			var ehsAmOfficerList = await App.OfficerService.GetEhsOfficersAsync("AM");
			var ehsPmOfficerList = await App.OfficerService.GetEhsOfficersAsync("PM");

			try
			{

				if (Rotation.RotationTime <= new TimeSpan(10, 59, 00))
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
				if (Rotation.RotationTime >= new TimeSpan(11, 00, 00) && Rotation.RotationTime <= new TimeSpan(12, 00, 00))
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
				if (Rotation.RotationTime >= new TimeSpan(12, 30, 00))
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
