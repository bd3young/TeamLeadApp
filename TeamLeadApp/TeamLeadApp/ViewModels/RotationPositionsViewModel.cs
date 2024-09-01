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
			if (await App.Current.MainPage.DisplayAlert("Pull", "Are you sure you would like to pull the previous Rotation", "Yes", "No")) 
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
						if (position.OfficerOne != lastPosition.OfficerOne )
						{
							position.OfficerOneGender = lastPosition.OfficerOneGender;
						}
						if (position.OfficerTwo != lastPosition.OfficerTwo)
						{
							position.OfficerTwoGender = lastPosition.OfficerTwoGender;
						}
						if (lastPosition.OfficerOne == "")
						{
							position.OfficerOneGender = "";
						}
						if (lastPosition.OfficerTwo == "")
						{
							position.OfficerTwoGender = "";
						}

						await App.RotationPositionService.AddProductAsync(position);
					}
					IsBusy = true;
				}
			}
			
			
		}

		private async void OnResetPostion(RotationPosition rotationPosition)
		{
			if (await App.Current.MainPage.DisplayAlert("Reset - " + rotationPosition.Name, "Are you sure you would like reset this position.", "Yes", "No")) 
			{
				var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);		
			
				if (CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null) 
				{
					string[] officerOne = CurrentPosition.OfficerOne.Split(' ');
					var CurrentOfficerOne = await App.OfficerService.GetOfficersByNameAsync(officerOne[0], officerOne[1]);
					if (CurrentOfficerOne.ShiftEnd > Rotation.RotationTime && CurrentOfficerOne.Lv == false
						|| CurrentOfficerOne.Lv == true && CurrentOfficerOne.ShiftBegin <= Rotation.RotationTime && CurrentOfficerOne.LvBegin > Rotation.RotationTime
						|| CurrentOfficerOne.Lv == true && CurrentOfficerOne.LvEnd <= Rotation.RotationTime && CurrentOfficerOne.ShiftEnd > Rotation.RotationTime
						|| CurrentOfficerOne.Ehs == true && CurrentOfficerOne.EhsBegin <= Rotation.RotationTime && CurrentOfficerOne.EhsEnd > Rotation.RotationTime) 
					{
						CurrentOfficers.Add(CurrentPosition.OfficerOne);
						CurrentOfficers.Sort();
					}
				
				}
				if (CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null)
				{
					string[] officerTwo = CurrentPosition.OfficerTwo.Split(' ');
					var CurrentOfficerTwo = await App.OfficerService.GetOfficersByNameAsync(officerTwo[0], officerTwo[1]);
					if (CurrentOfficerTwo.ShiftEnd > Rotation.RotationTime && CurrentOfficerTwo.Lv == false
						|| CurrentOfficerTwo.Lv == true && CurrentOfficerTwo.ShiftBegin <= Rotation.RotationTime && CurrentOfficerTwo.LvBegin > Rotation.RotationTime
						|| CurrentOfficerTwo.Lv == true && CurrentOfficerTwo.LvEnd <= Rotation.RotationTime && CurrentOfficerTwo.ShiftEnd > Rotation.RotationTime
						|| CurrentOfficerTwo.Ehs == true && CurrentOfficerTwo.EhsBegin <= Rotation.RotationTime && CurrentOfficerTwo.EhsEnd > Rotation.RotationTime)
					{
						CurrentOfficers.Add(CurrentPosition.OfficerTwo);
						CurrentOfficers.Sort();
					}
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

			
		}

		private async void OnUpdateOfficerTwo(RotationPosition rotationPosition)
		{
			
			if (rotationPosition.OfficerTwo != null)
			{
				var CurrentPosition = await App.RotationPositionService.GetProductAsync(rotationPosition.Id);
				var LastPosition = new RotationPosition();
				if (LastRotation != null)
				{
					LastPosition = await App.RotationPositionService.GetProductRNAsync(LastRotation.Id, rotationPosition.Name);
				}
				string[] officer = rotationPosition.OfficerTwo.Split(' ');
				string[] lastOfficer = CurrentPosition.OfficerTwo.Split(' ');
				if (lastOfficer.Count() > 1)
				{
					var LastOfficer = await App.OfficerService.GetOfficersByNameAsync(lastOfficer[0], lastOfficer[1]);
					if (CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null && LastOfficer.ShiftEnd > Rotation.RotationTime && LastOfficer.Lv == false
						|| CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null && LastOfficer.Lv == true && LastOfficer.ShiftBegin <= Rotation.RotationTime && LastOfficer.LvBegin > Rotation.RotationTime
						|| CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null && LastOfficer.Lv == true && LastOfficer.LvEnd <= Rotation.RotationTime && LastOfficer.ShiftEnd > Rotation.RotationTime
						|| CurrentPosition.OfficerTwo != "" && CurrentPosition.OfficerTwo != null && LastOfficer.Ehs == true && LastOfficer.EhsBegin <= Rotation.RotationTime && LastOfficer.EhsEnd > Rotation.RotationTime)
					{
						CurrentOfficers.Add(CurrentPosition.OfficerTwo);
						CurrentOfficers.Sort();
					}
				}
				if (CurrentPosition.OfficerTwo != rotationPosition.OfficerTwo)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
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
				var LastPosition = new RotationPosition();
				if (LastRotation != null)
				{
					LastPosition = await App.RotationPositionService.GetProductRNAsync(LastRotation.Id, rotationPosition.Name);
				}		
				string[] officer = rotationPosition.OfficerOne.Split(' ');
				string[] lastOfficer = CurrentPosition.OfficerOne.Split(' ');
				if (lastOfficer.Count() > 1) 
				{
					var LastOfficer = await App.OfficerService.GetOfficersByNameAsync(lastOfficer[0], lastOfficer[1]);
					if (CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null && LastOfficer.ShiftEnd > Rotation.RotationTime && LastOfficer.Lv == false
						|| CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null && LastOfficer.Lv == true && LastOfficer.ShiftBegin <= Rotation.RotationTime && LastOfficer.LvBegin > Rotation.RotationTime 
						|| CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null && LastOfficer.Lv == true && LastOfficer.LvEnd <= Rotation.RotationTime && LastOfficer.ShiftEnd > Rotation.RotationTime
						|| CurrentPosition.OfficerOne != "" && CurrentPosition.OfficerOne != null && LastOfficer.Ehs == true && LastOfficer.EhsBegin <= Rotation.RotationTime && LastOfficer.EhsEnd > Rotation.RotationTime)
					{
						CurrentOfficers.Add(CurrentPosition.OfficerOne);
						CurrentOfficers.Sort();
					}
				}
				if (CurrentPosition.OfficerOne != rotationPosition.OfficerOne)
				{
					
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					
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

			var rotationPositionList = await App.RotationPositionService.GetProductsRPAsync(Rotation.Id);
			var officers = await App.OfficerService.GetDayOfficersAsync(Convert.ToString(DateTime.Now.DayOfWeek).ToUpper());
			var ehsOfficers = await App.OfficerService.GetEhsOfficersAsync();
			var lvOfficers = await App.OfficerService.GetLvOfficersAsync();
			var lvEhsOfficers = await App.OfficerService.GetLvEhsOfficersAsync();

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
					if (officer.RdoOne.ToUpper() == DateTime.Now.DayOfWeek.ToString().ToUpper()
						|| officer.RdoTwo.ToUpper() == DateTime.Now.DayOfWeek.ToString().ToUpper()
						|| officer.RdoThree.ToUpper() == DateTime.Now.DayOfWeek.ToString().ToUpper())
					{
						if (officer.EhsBegin <= Rotation.RotationTime && officer.EhsEnd > Rotation.RotationTime)
						{
							CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
						}
					}
					else 
					{
						if (officer.ShiftBegin <= Rotation.RotationTime && officer.ShiftEnd > Rotation.RotationTime 
							|| officer.EhsBegin <= Rotation.RotationTime && officer.EhsEnd > Rotation.RotationTime) 
						{ 
							CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
						}
					}
					
					
				}
				foreach (var officer in lvOfficers)
				{
					if (officer.ShiftBegin <= Rotation.RotationTime && officer.LvBegin > Rotation.RotationTime 
						|| officer.LvEnd <= Rotation.RotationTime && officer.ShiftEnd > Rotation.RotationTime) 
					{
						CurrentOfficers.Add(officer.FirstName + " " + officer.LastName);
					}
					
				}
				foreach (var officer in lvEhsOfficers)
				{
					if (officer.ShiftBegin <= Rotation.RotationTime && officer.LvBegin > Rotation.RotationTime 
						|| officer.LvEnd <= Rotation.RotationTime && officer.ShiftEnd > Rotation.RotationTime 
						|| officer.EhsBegin <= Rotation.RotationTime && officer.EhsEnd > Rotation.RotationTime)
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
