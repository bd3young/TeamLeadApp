using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.Views;
using Xamarin.Forms;
using static Android.Icu.Text.Transliterator;

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
			Rotation = rotation;
			await Navigation.PushAsync(new RotationPositionsPage(rotation));
		}

		private async void OnPreviousRotation()
		{
			var rotation = await App.RotationService.GetProductTAsync(Rotation.RotationTime - new TimeSpan(00, 30, 00));
			Rotation = rotation;
			await Navigation.PushAsync(new RotationPositionsPage(rotation));
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
			CurrentPosition.OfficerOne = "";
			CurrentPosition.OfficerTwo = "";
			CurrentPosition.OfficerOneGender = "";
			CurrentPosition.OfficerTwoGender = "";
			await App.RotationPositionService.AddProductAsync(CurrentPosition);

			IsBusy = true;
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
					CurrentPosition.OfficerTwoGender = CurrentOfficer.Gender;
					CurrentPosition.OfficerTwo = rotationPosition.OfficerTwo;
					await App.RotationPositionService.AddProductAsync(CurrentPosition);
					IsBusy = true;
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
					CurrentPosition.OfficerOne = rotationPosition.OfficerOne;
					CurrentPosition.OfficerOneGender = CurrentOfficer.Gender;
					await App.RotationPositionService.AddProductAsync(CurrentPosition);
					IsBusy = true;
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
				if (Rotation.RotationTime >= new TimeSpan(11, 00, 00) && Rotation.RotationTime <= new TimeSpan(11, 59, 00))
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
				if (Rotation.RotationTime >= new TimeSpan(12, 00, 00))
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

				CurrentOfficers.Sort();
				foreach (var position in rotationPositionList)
				{
					RotationPositions.Add(position);
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
