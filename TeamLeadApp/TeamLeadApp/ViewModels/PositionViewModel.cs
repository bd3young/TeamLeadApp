﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
			Navigation = _navigation;
		}

		private async void OnResetPostion(Position position)
		{
			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			CurrentPosition.OfficerOne = "";
			CurrentPosition.OfficerTwo = "";
			CurrentPosition.OfficerOneGender = "";
			CurrentPosition.OfficerTwoGender = "";
			await App.PositionService.AddProductAsync(CurrentPosition);

			IsBusy = true;
		}

		private async void OnUpdateOfficerTwo(Position position)
		{
			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			
			if (position.OfficerTwo != null)
			{
				string[] officer = position.OfficerTwo.Split(' ');
				if (CurrentPosition.OfficerTwo != position.OfficerTwo)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					CurrentPosition.OfficerTwoGender = CurrentOfficer.Gender;
					CurrentPosition.OfficerTwo = position.OfficerTwo;
					await App.PositionService.AddProductAsync(CurrentPosition);
					IsBusy = true;
				}
			}
		}

		private async void OnUpdateOfficerOne(Position position)
		{

			var CurrentPosition = await App.PositionService.GetProductAsync(position.Id);
			
			if (position.OfficerOne != null)
			{
				string[] officer = position.OfficerOne.Split(' ');
				if (CurrentPosition.OfficerOne != position.OfficerOne)
				{
					var CurrentOfficer = await App.OfficerService.GetOfficersByNameAsync(officer[0], officer[1]);
					CurrentPosition.OfficerOne = position.OfficerOne;
					CurrentPosition.OfficerOneGender = CurrentOfficer.Gender;
					await App.PositionService.AddProductAsync(CurrentPosition);
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

		private async void OnDeletePosition(Position position)
		{
			

			if (position == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete - "  + position.Name, "Are you sure you would like to Delete " + position.Name, "Yes", "No"))
			{
				var rotations = await App.RotationService.GetProductsAsync();

				foreach (var rotation in rotations)
				{
					var rotationPosition = await App.RotationPositionService.GetProductRNAsync(rotation.Id, position.Name);

					if (rotationPosition != null) 
					{ 
						await App.RotationPositionService.DeleteProductAsync(rotationPosition.Id);
					}					
				}

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
			IsBusy = true;
			Positions.Clear();
			CurrentOfficers.Clear();

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
