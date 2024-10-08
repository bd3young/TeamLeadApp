﻿using System;
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
	public class RotationsViewModel : BaseRotationViewModel
	{
		public Command PositionsListCommand { get; }
		public Command LoadRotationCommand { get; }
		public Command UpdateIsCompleteCommand { get; }
		public ObservableCollection<Rotation> Rotations { get; }
		public Command AddRotationCommand { get; }
		public Command EditRotationCommand { get; }
		public Command DeleteRotationCommand { get; }
		public Command ResetOfficersCommand { get; }
		public Command SearchCommand { get; }
		public Command LoadPositionsPageCommand { get; }
		public RotationsViewModel(INavigation _navigation)
		{
			PositionsListCommand = new Command(PositionList);
			LoadRotationCommand = new Command(async () => await ExecuteLoadRotationsCommand());
			UpdateIsCompleteCommand = new Command<Rotation>(OnUpdateIsComplete);
			Rotations = new ObservableCollection<Rotation>();
			AddRotationCommand = new Command(OnAddRotation);
			EditRotationCommand = new Command<Rotation>(OnEditRotation);
			DeleteRotationCommand = new Command<Rotation>(OnDeleteRotation);
			ResetOfficersCommand = new Command(async () => await App.OfficerService.ResetOfficers());
			SearchCommand = new Command(Search);
			LoadPositionsPageCommand = new Command<Rotation>(OnLoadPositionsPage);
			Navigation = _navigation;
		}

		private async void PositionList()
		{
			await Shell.Current.GoToAsync(nameof(PositionsListPage));
		}

		private async void OnLoadPositionsPage(Rotation rotation)
		{
			await Navigation.PushAsync(new RotationPositionsPage(rotation));
		}

		private async void Search(Object obj)
		{
			if (obj.ToString() == "")
			{
				await ExecuteLoadRotationsCommand();
			}
			else
			{
				var rotationList = await App.RotationService.GetProductsAsync();
				Rotations.Clear();
				foreach (var rotation in rotationList)
				{
					if (rotation.RotationTime.ToString().StartsWith(obj.ToString()))
					{

						Rotations.Add(rotation);
					}
				}
			}
		}

		private async void OnDeleteRotation(Rotation rotation)
		{

			if (rotation == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete - " + rotation.RotationTime, "Are you sure you would like to Delete the " + rotation.RotationTime + " Rotation", "Yes", "No"))
			{
				var rotationPositions = await App.RotationPositionService.GetProductsRPAsync(rotation.Id);
				foreach (var position in rotationPositions) 
				{
					await App.RotationPositionService.DeleteProductAsync(position.Id);
				}
				await App.RotationService.DeleteProductAsync(rotation.Id);
				Rotations.Remove(rotation);
			}
			else
			{
				return;
			}

		}
		private async void OnEditRotation(Rotation rotation)
		{
			await Navigation.PushAsync(new AddRotationPage(rotation));
		}
		private async void OnAddRotation(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddRotationPage));
		}

		private async void OnUpdateIsComplete(Rotation rotation)
		{
			if (rotation != null)
			{

				var CurrentRotation = await App.RotationService.GetProductAsync(rotation.Id);
				if (CurrentRotation.IsComplete != rotation.IsComplete)
				{
					await App.RotationService.AddProductAsync(rotation);
				}

			}

		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		async Task ExecuteLoadRotationsCommand()
		{
			Rotations.Clear();
			IsBusy = true;
			var rotations = await App.RotationService.GetProductsAsync();
			rotations = rotations.OrderBy(r => r.RotationTime);

			try
			{

				foreach (var rotation in rotations)
				{
					Rotations.Add(rotation);
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
