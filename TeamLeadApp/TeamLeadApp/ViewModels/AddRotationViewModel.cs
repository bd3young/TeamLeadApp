using System;
using System.Collections.Generic;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
	public class AddRotationViewModel : BaseRotationViewModel
	{
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public AddRotationViewModel()
		{
			SaveCommand = new Command(OnSave);
			CancelCommand = new Command(OnCancel);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();

			Rotation = new Rotation();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			var rotation = Rotation;
			await App.RotationService.AddProductAsync(rotation);
			rotation = await App.RotationService.GetProductTAsync(rotation.RotationTime);
			var positions = await App.PositionService.GetProductsAsync();
			var currentPositions = new List<RotationPosition>();
			foreach (var position in positions) 
			{
				var rotationPosition = new RotationPosition();
				rotationPosition.Name = position.Name;
				rotationPosition.OfficerOne = position.OfficerOne;
				rotationPosition.OfficerTwo = position.OfficerTwo;
				rotationPosition.OfficerOneGender = position.OfficerOneGender;
				rotationPosition.OfficerTwoGender = position.OfficerTwoGender;
				rotationPosition.TwoOfficers = position.TwoOfficers;
				rotationPosition.RotationId = rotation.Id;

				currentPositions.Add(rotationPosition);
				await App.RotationPositionService.AddProductAsync(rotationPosition);
			}

			rotation.Positions = currentPositions;
			await App.RotationService.AddProductAsync(rotation);
			

			await Shell.Current.GoToAsync("..");
		}
	}
}
