using Android.App;
using System;
using System.Collections.Generic;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
    public class AddPositionViewModel : BasePositionViewModel
    {
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public AddPositionViewModel()
		{
			SaveCommand = new Command(OnSave);
			CancelCommand = new Command(OnCancel);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();

			Position = new Position();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			

			var position = Position;
			var positions = await App.PositionService.GetProductsAsync();
			foreach (var product in positions) 
			{ 
				if (position.Name.ToUpper() == product.Name.ToUpper()) 
				{
					await App.Current.MainPage.DisplayAlert("Name Taken", "Each position is required to have a unique name", "Ok");

					return;
				}
			}

			var rotations = await App.RotationService.GetProductsAsync();

			foreach (var rotation in rotations) 
			{
				var rotationPosition = new RotationPosition();

				rotationPosition.Name = position.Name;
				rotationPosition.TwoOfficers = position.TwoOfficers;
				rotationPosition.RotationId = rotation.Id;

				await App.RotationPositionService.AddProductAsync(rotationPosition);
				
			}

			await App.PositionService.AddProductAsync(position);



			await Shell.Current.GoToAsync("..");
		}
	}
}
