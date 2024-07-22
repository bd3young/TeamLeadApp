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
			var positions = await App.PositionService.GetProductsAsync();
			var currentPositions = new List<Position>();
			foreach (var position in positions) 
			{
				currentPositions.Add(position);
			}

			rotation.Positions = currentPositions;
			await App.RotationService.AddProductAsync(rotation);

			await Shell.Current.GoToAsync("..");
		}
	}
}
